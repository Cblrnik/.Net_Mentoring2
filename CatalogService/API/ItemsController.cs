using System.Linq;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("categoryId={categoryId}&pageNumber={pageNumber}&count={count}")]
        public IActionResult GetItems(int categoryId, int pageNumber, int count)
        {
            var result = _service.GetAllEntities().
                Where(item => item.CategoryId == categoryId).
                Skip(pageNumber * count).
                Take(count);
            
            return this.Ok(new JsonResult(result));
        }

        [HttpGet]
        [Route("id={id:int}")]
        public IActionResult GetItem(int id)
        {
            var item = _service.GetEntity(id);
            if (!(item is null))
            {
                return this.Ok(new JsonResult(item));
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public ActionResult Create(Item item)
        {
            _service.CreateEntity(item);
            return this.Ok();
        }

        [HttpDelete]
        [Route("id={id:int}")]
        public ActionResult Delete(int id)
        {
            _service.DeleteEntity(id);
            return this.Ok();
        }

        [HttpPut]
        public ActionResult Update(Item product)
        {
            _service.UpdateEntity(product);
            return this.Ok();
        }
    }
}

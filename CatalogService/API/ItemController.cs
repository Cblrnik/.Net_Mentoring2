using System.Linq;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        readonly IItemService _service;

        public ItemController(IItemService service)
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
        [Route("find")]
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

        [HttpPost("create")]
        public ActionResult Create(Item item)
        {
            _service.CreateEntity(item);
            return this.Ok();
        }

        [HttpDelete("delete")]
        public ActionResult Delete(int id)
        {
            _service.DeleteEntity(id);
            return this.Ok();
        }

        [HttpPut("update")]
        public ActionResult Update(Item product)
        {
            _service.UpdateEntity(product);
            return this.Ok();
        }
    }
}

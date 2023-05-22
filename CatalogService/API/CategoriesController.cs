using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetCategories(int? count)
        {
            if (count != null)
            {
                var result = _service.GetEntities(count.Value);
                return this.Ok(new JsonResult(result));
            }
            else
            {
                var result = _service.GetAllEntities();
                return this.Ok(new JsonResult(result));
            }
        }

        [HttpGet]
        [Route("find")]
        public IActionResult GetCategory(int id)
        {
            var category = _service.GetEntity(id);
            if (!(category is null))
            {
                return this.Ok(new JsonResult(category));
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPost("create")]
        public IActionResult Create(Category category)
        {
            _service.CreateEntity(category);
            return this.Ok(category.CategoryId);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            _service.DeleteEntity(id);
            return this.Ok(id);
        }

        [HttpPut("update")]
        public IActionResult Update(Category category)
        {
            var updatedCategory = _service.UpdateEntity(category);
            if (updatedCategory is null)
            {
                return this.NotFound();
            }

            return this.Ok(updatedCategory.CategoryId);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;
using CatalogService.DataLayer.Context;

namespace CatalogService.DataLayer.Dao
{
    public class CategoryDao : IDaoProvider<Category>
    {
        readonly CatalogServiceContext _db;

        public CategoryDao(CatalogServiceContext context)
        {
            _db = context;
        }

        public async Task<Category> CreateEntityAsync(Category entity)
        {
            await _db.Categories.AddAsync(entity);

            await _db.SaveChangesAsync();

            return entity;
        }

        public void Delete(int id)
        {
            var category = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (category != null)
            {
                _db.Entry(category).Collection(x => x.Items).Load();
                _db.Categories.Remove(category);
            }

            _db.SaveChanges();
        }

        public IEnumerable<Category> ReadEntities()
        {
            return _db.Categories;
        }

        public async Task<Category> ReadEntityAsync(int id)
        {
            return await _db.Categories.FindAsync(id);
        }

        public Category Update(Category entity)
        {
            var toUpdate = _db.Categories.FirstOrDefault(x => x.CategoryId == entity.CategoryId);

            if (toUpdate is null)
            {
                return null;
            }

            _db.Entry(toUpdate).CurrentValues.SetValues(entity);

            _db.SaveChanges();

            return entity;
        }
    }
}

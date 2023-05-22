using System;
using System.Collections.Generic;
using System.Linq;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;

namespace CatalogService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IDaoProvider<Category> _categoryProvider;

        public CategoryService(IDaoProvider<Category> provider)
        {
            _categoryProvider = provider;
        }

        public Category CreateEntity(Category entity)
        {
            return _categoryProvider.CreateEntityAsync(entity).Result;
        }

        public void DeleteEntity(int id)
        {
            _categoryProvider.Delete(id);
        }

        public IEnumerable<Category> GetAllEntities()
        {
            return _categoryProvider.ReadEntities();
        }

        public IEnumerable<Category> GetEntities(int count)
        {
            return _categoryProvider.ReadEntities()
                .Take(count);
        }

        public Category GetEntity(int id)
        {
            return _categoryProvider.ReadEntityAsync(id).Result;
        }

        public Category UpdateEntity(Category entity)
        {
            return _categoryProvider.Update(entity);
        }
    }
}

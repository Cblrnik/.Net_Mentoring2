using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;

namespace CatalogService.Services
{
    public class ItemService : IItemService
    {
        private readonly IDaoProvider<Item> _productProvider;

        public ItemService(IDaoProvider<Item> provider)
        {
            _productProvider = provider;
        }

        public Item CreateEntity(Item entity)
        {
            return _productProvider.CreateEntityAsync(entity).Result;
        }

        public void DeleteEntity(int id)
        {
            _productProvider.Delete(id);
        }

        public IEnumerable<Item> GetAllEntities()
        {
            return _productProvider.ReadEntities();
        }

        public IEnumerable<Item> GetEntities(int count)
        {
            return _productProvider.ReadEntities()
                .Take(count);
        }

        public Item GetEntity(int id)
        {
            return _productProvider.ReadEntityAsync(id).Result;
        }

        public Item UpdateEntity(Item entity)
        {
            return _productProvider.Update(entity);
        }
    }
}

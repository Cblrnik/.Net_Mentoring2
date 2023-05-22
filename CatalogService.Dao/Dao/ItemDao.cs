using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;
using CatalogService.DataLayer.Context;

namespace CatalogService.DataLayer.Dao
{
    public class ItemDao : IDaoProvider<Item>
    {
        readonly CatalogServiceContext _db;

        public ItemDao(CatalogServiceContext context)
        {
            _db = context;
        }

        public async Task<Item> CreateEntityAsync(Item entity)
        {
            await _db.Items.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public void Delete(int id)
        {
            var product = _db.Items.FirstOrDefault(x => x.ItemId == id);
            if (product != null)
            {
                _db.Items.Remove(product);
            }

            _db.SaveChanges();
        }

        public IEnumerable<Item> ReadEntities()
        {
            return _db.Items;
        }

        public async Task<Item> ReadEntityAsync(int id)
        {
            return await _db.Items.FindAsync(id);
        }

        public Item Update(Item entity)
        {
            var toUpdate = _db.Items.FirstOrDefault(x => x.ItemId == entity.ItemId);
            _db.Entry(toUpdate).CurrentValues.SetValues(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}

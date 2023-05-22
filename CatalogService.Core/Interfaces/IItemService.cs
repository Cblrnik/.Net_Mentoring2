using System;
using System.Collections.Generic;
using System.Text;
using CatalogService.Core.Models;

namespace CatalogService.Core.Interfaces
{
    public interface IItemService
    {
        IEnumerable<Item> GetAllEntities();

        IEnumerable<Item> GetEntities(int count);

        Item CreateEntity(Item entity);

        void DeleteEntity(int id);

        Item GetEntity(int id);

        Item UpdateEntity(Item entity);
    }
}

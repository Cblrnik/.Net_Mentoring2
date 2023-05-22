using CatalogService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogService.Core.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllEntities();

        IEnumerable<Category> GetEntities(int count);

        Category CreateEntity(Category entity);

        void DeleteEntity(int id);

        Category GetEntity(int id);

        Category UpdateEntity(Category entity);
    }
}

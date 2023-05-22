using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Core.Interfaces
{
    public interface IDaoProvider<T>
    {
        Task<T> CreateEntityAsync(T entity);

        Task<T> ReadEntityAsync(int id);

        IEnumerable<T> ReadEntities();

        T Update(T entity);

        void Delete(int id);
    }
}

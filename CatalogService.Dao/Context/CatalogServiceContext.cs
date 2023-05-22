using CatalogService.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace CatalogService.DataLayer.Context
{
    public class CatalogServiceContext : DbContext
    {
        public CatalogServiceContext(DbContextOptions<CatalogServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Item> Items { get; set; }
        
    }
}

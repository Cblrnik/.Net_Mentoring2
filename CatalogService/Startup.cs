using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using CatalogService.Core.Interfaces;
using CatalogService.Core.Models;
using CatalogService.DataLayer.Dao;
using CatalogService.DataLayer.Context;
using CatalogService.Services;
using Microsoft.OpenApi.Models;

namespace CatalogService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("CatalogServiceDbConnection");
            services.AddMvc();
            services.AddDbContext<CatalogServiceContext>(opt => opt.UseSqlServer(connection));
            services.AddControllers();
            services.AddTransient<IDaoProvider<Item>, ItemDao>();
            services.AddTransient<IDaoProvider<Category>, CategoryDao>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

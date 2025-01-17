using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using WebshopDemo.ProductCatalog.Commands.RegisterNewProduct;
using WebshopDemo.Sales;
using WebshopDemo.Sales.Commands.SetPrice;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ProductCatalog.ProductCatalogContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductCatalogConnection")));
            services.AddDbContext<Sales.SalesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SalesConnection")));

            services.AddTransient(typeof(ProductRepository), typeof(SalesContext));
            services.AddTransient<CartRepository>(x => new CartRepositoryImp(Configuration.GetConnectionString("SalesConnection")));

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RegisterNewProductCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(SetPriceCommand).GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

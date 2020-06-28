using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.ProductCatalog.Commands.RegisterNewProduct;
using WebshopDemo.ProductCatalog.Domain;
using WebshopDemo.ProductCatalog.Events;

namespace WebshopDemo.ProductCatalog.IntegrationTests
{
    [TestFixture]
    public class RegisterNewProductTests
    {
        private IServiceProvider provider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configBuilder.GetConnectionString("ProductCatalogConnection");
            var services = new ServiceCollection();
            services.AddSingleton(typeof(IRequestHandler<RegisterNewProductCommand, Guid>), typeof(RegisterNewProductHandler));
            
            services.AddSingleton<ServiceFactory>(p => p.GetService);
            services.AddSingleton(typeof(Mediator), typeof(Mediator));
            services.AddSingleton(typeof(IMediator), typeof(CollectorMediator));
            services.AddDbContext<ProductCatalogContext>(options =>
                options.UseSqlServer(connectionString));
            provider = services.BuildServiceProvider();
            
        }

        [SetUp]
        public void SetUp()
        {
            var dbContext = provider.GetService<ProductCatalogContext>();
            dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Products");
        }

        [Test]
        public async Task RegisterNewProductCommandShouldPublishEvent()
        {
            var mediator = provider.GetService<IMediator>() as CollectorMediator;
            var name = "name";
            var desc = "desc";
            var newId = await mediator.Send(new RegisterNewProductCommand { Name = name, Description = desc });
            mediator.Notifications.Count.Should().Be(1);
            mediator.Notifications.First().Should().BeOfType(typeof(ProductRegistered));
            var noti = mediator.Notifications.First() as ProductRegistered;
            noti.ProductID.Should().Be(newId);
        }

        [Test]
        public async Task RegisterNewProductCommandShouldSaveProductToDB()
        {
            var mediator = provider.GetService<IMediator>();
            var name = "name";
            var desc = "desc";
            var newId = await mediator.Send(new RegisterNewProductCommand { Name = name, Description = desc });
            var db = provider.GetService<ProductCatalogContext>();
            db.Products.Count().Should().Be(1);
            db.Products.First().IsSameOrEqualTo(new Product(newId, name, desc));
        }
    }
}

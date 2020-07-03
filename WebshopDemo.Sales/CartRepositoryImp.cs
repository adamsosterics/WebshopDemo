using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales
{
    public class CartRepositoryImp : CartRepository
    {
        private DbContextOptions<SalesContext> options;

        public CartRepositoryImp(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SalesContext>().UseSqlServer(connectionString).UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
            options = optionsBuilder.Options;
        }

        public void Add(Cart cart)
        {
            using (var db = new SalesContext(options))
            {
                db.Carts.Add(cart);
                db.SaveChanges();
            }
        }

        public Cart GetByID(Guid cartID)
        {
            using (var db = new SalesContext(options))
            {
                return db.Carts.Include(x => x.Items).Where(x => x.Id == cartID).First();
            }
        }

        public List<Cart> GetCartsContainingProduct(Guid productID)
        {
            throw new NotImplementedException();
        }

        public void Save(Cart cart)
        {
            using (var db = new SalesContext(options))
            {
                var existingCart = db.Carts.Include(x => x.Items).First(x => x.Id == cart.Id);
                db.Entry(existingCart).CurrentValues.SetValues(cart);
                foreach (var item in cart.Items)
                {
                    if (item.ID == 0)
                    {
                        existingCart.Items.Add(item);
                    }
                    else
                    {
                        var existingItem = existingCart.Items.First(x => x.ID == item.ID);
                        db.Entry(existingItem).CurrentValues.SetValues(item);
                    }
                }
                foreach (var item in existingCart.Items)
                {
                    if (!cart.Items.Any(x => x.ID == item.ID))
                    {
                        db.Remove(item);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}

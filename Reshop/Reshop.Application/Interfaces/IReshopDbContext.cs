using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Models.User.Cart;

namespace Reshop.Application.Interfaces
{
    public interface IReshopDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<ProductToCategory> ProductToCategories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }

    }
}

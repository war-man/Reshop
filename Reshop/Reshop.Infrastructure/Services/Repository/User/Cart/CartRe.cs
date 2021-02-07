using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Services.ExtensionMethods;
using Reshop.Domain.Models.User.Cart;
using Reshop.Domain.Services.Interfaces.User.Cart;
using Reshop.Infrastructure.Context;

namespace Reshop.Infrastructure.Services.Repository.User.Cart
{
    public class CartRe : ICartRe
    {
        private readonly ReshopDbContext _context;

        public CartRe(ReshopDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int itemId, string userId)
        {
            // get product
            var product = await _context.Products
                .Include(p => p.Item)
                .SingleOrDefaultAsync(p => p.ItemId == itemId);


            if (product != null)
            {
                // get user order when product is not null
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinally);

                if (order != null)
                {
                    // get order order detail when order is not null
                    var orderDetail = await _context.OrderDetails
                            .FirstOrDefaultAsync(d => d.OrderId == order.OrderId && d.ProductId == product.Id);

                    // add to product counts when it was in user order
                    if (orderDetail != null)
                    {
                        orderDetail.Count += 1;
                    }

                    else
                    {
                        // create an order detail when user does not has any product else in his card
                        await _context.OrderDetails.AddAsync(new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = product.Id,
                            Price = product.Item.Price,
                            Count = 1
                        });
                    }
                }


                // when user do not has any order 
                else
                {
                    order = new Order()
                    {
                        IsFinally = false,
                        CreateDate = DateTime.Now.ToShamsi(),
                        UserId = userId
                    };
                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();
                    await _context.OrderDetails.AddAsync(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count = 1
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> ShowCartAsync(string userId)
         =>
             await _context.Orders
                 .Where(o => o.UserId == userId && !o.IsFinally) // find Unfinally user order
                 .Include(o => o.OrderDetails) //include order detail
                 .ThenInclude(c => c.Product) // include products in order
                  .FirstOrDefaultAsync();

        public async Task RemoveCartAsync(int detailId)
        {
            // find order detail
            var item = await _context.OrderDetails
                .FindAsync(detailId);

            // remove order detail when order count is 1 else remove 1 from counts
            if (item?.Count <= 1)
            {
                _context.OrderDetails.Remove(item);
            }
            else if (item != null)
            {
                item.Count -= 1;
            }

            await _context.SaveChangesAsync();
        }
    }
}
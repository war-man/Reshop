using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Reshop.Domain.Services.Interfaces;
using Reshop.Domain.Services.Interfaces.ProductAndCategory;
using Reshop.Domain.Services.Interfaces.User.Cart;
using Reshop.Infrastructure.Context;
using Reshop.Infrastructure.Services.Repository.ProductAndCategory;
using Reshop.Infrastructure.Services.Repository.User.Cart;


namespace Reshop.Infrastructure.Services.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReshopDbContext _context;

        public UnitOfWork(ReshopDbContext context)
        {
            _context = context;

            ProductRe = new ProductRe(_context);
            CartRe = new CartRe(_context);
            CategoryRe = new CategoryRe(_context);
        }


        public IProductRe ProductRe { get; }
        public ICartRe CartRe { get; }
        public ICategoryRe CategoryRe { get; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using System;
using System.Threading.Tasks;
using Reshop.Domain.Services.Interfaces.ProductAndCategory;
using Reshop.Domain.Services.Interfaces.User.Cart;

namespace Reshop.Domain.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRe ProductRe { get; }
        ICartRe CartRe { get; }
        ICategoryRe CategoryRe { get; }

        Task SaveChangesAsync();
    }
}

using System.Threading.Tasks;
using Reshop.Domain.Models.User.Cart;

namespace Reshop.Domain.Services.Interfaces.User.Cart
{
    public interface ICartRe
    {
        Task AddToCartAsync(int itemId, string userId);
        Task<Order> ShowCartAsync(string userId);
        Task RemoveCartAsync(int detailId);
    }
}
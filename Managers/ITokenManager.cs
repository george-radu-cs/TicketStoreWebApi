using System.Threading.Tasks;
using TicketStore.Entities;

namespace TicketStore.Managers
{
    public interface ITokenManager
    {
        Task<string> CreateToken(User user);
    }
}
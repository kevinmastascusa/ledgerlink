using LedgerLink.Core.Models;
using System.Threading.Tasks;

namespace LedgerLink.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(string email);
        Task<bool> ExistsByIdAsync(int id);
    }
} 
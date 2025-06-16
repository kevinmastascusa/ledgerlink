using System;
using System.Threading.Tasks;
using LedgerLink.Core.Models;

namespace LedgerLink.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);
    }
} 
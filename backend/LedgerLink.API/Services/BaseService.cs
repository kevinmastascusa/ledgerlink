using System;
using System.Threading.Tasks;

namespace LedgerLink.API.Services
{
    public abstract class BaseService
    {
        protected async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"An error occurred while executing the operation: {ex.Message}", ex);
            }
        }

        protected async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"An error occurred while executing the operation: {ex.Message}", ex);
            }
        }
    }
} 
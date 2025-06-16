using System.Threading.Tasks;

namespace LedgerLink.Core.Services;

public interface IValidationService
{
    Task ValidateAsync<T>(T instance);
} 
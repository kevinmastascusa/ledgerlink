using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LedgerLink.Core.Exceptions;

namespace LedgerLink.Core.Services;

public class ValidationService : IValidationService
{
    public async Task ValidateAsync<T>(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        var validationContext = new ValidationContext(instance);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(instance, validationContext, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults.Select(r => r.ErrorMessage).Where(e => !string.IsNullOrEmpty(e));
            throw new Core.Exceptions.ValidationException(string.Join(Environment.NewLine, errors));
        }

        await Task.CompletedTask;
    }
} 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LedgerLink.Core.Exceptions;

namespace LedgerLink.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static void ValidateModel<T>(this T model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            
            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                var errors = validationResults
                    .GroupBy(x => x.MemberNames.FirstOrDefault() ?? string.Empty)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage ?? string.Empty).ToArray()
                    );

                throw new Core.Exceptions.ValidationException(errors);
            }
        }

        public static void ValidateNotNull<T>(this T? value, string paramName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ValidateNotEmpty(this string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty", paramName);
            }
        }

        public static void ValidateGreaterThanZero(this decimal value, string paramName)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Value must be greater than zero", paramName);
            }
        }
    }
} 
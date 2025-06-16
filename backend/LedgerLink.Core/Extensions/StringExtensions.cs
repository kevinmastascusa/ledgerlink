using System;
using System.Text.RegularExpressions;

namespace LedgerLink.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var words = value.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Join(" ", words);
        }

        public static string MaskEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return email;

            var parts = email.Split('@');
            if (parts.Length != 2)
                return email;

            var username = parts[0];
            var domain = parts[1];

            if (username.Length <= 2)
                return email;

            var maskedUsername = username[0] + new string('*', username.Length - 2) + username[username.Length - 1];
            return $"{maskedUsername}@{domain}";
        }

        public static string MaskPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            // Remove all non-digit characters
            var digits = Regex.Replace(phoneNumber, @"[^\d]", "");
            
            if (digits.Length < 4)
                return phoneNumber;

            var lastFour = digits.Substring(digits.Length - 4);
            var masked = new string('*', digits.Length - 4);
            return $"{masked}{lastFour}";
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
} 
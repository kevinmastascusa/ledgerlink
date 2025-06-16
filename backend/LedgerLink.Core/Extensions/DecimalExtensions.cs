using System;

namespace LedgerLink.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal RoundToTwoDecimalPlaces(this decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculatePercentage(this decimal value, decimal percentage)
        {
            return (value * percentage / 100).RoundToTwoDecimalPlaces();
        }

        public static decimal AddPercentage(this decimal value, decimal percentage)
        {
            return (value + value.CalculatePercentage(percentage)).RoundToTwoDecimalPlaces();
        }

        public static decimal SubtractPercentage(this decimal value, decimal percentage)
        {
            return (value - value.CalculatePercentage(percentage)).RoundToTwoDecimalPlaces();
        }

        public static bool IsPositive(this decimal value)
        {
            return value > 0;
        }

        public static bool IsNegative(this decimal value)
        {
            return value < 0;
        }

        public static decimal Absolute(this decimal value)
        {
            return Math.Abs(value);
        }
    }
} 
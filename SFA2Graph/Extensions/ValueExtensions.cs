using System;
using System.Globalization;

namespace SFA2Graph.Extensions
{
    internal static class ValueExtensions
    {
        #region Public Methods

        public static string ToStringDecimal(this double value, int decimalPoints)
        {
            var format = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = decimalPoints,
            };

            var converted = Math.Round(
                value: value,
                digits: decimalPoints);

            var result = converted.ToString(
                provider: format);

            return result;
        }

        public static string ToStringInt(this double value)
        {
            var converted = Convert.ToInt32(value);

            var result = converted.ToString();

            return result;
        }

        #endregion Public Methods
    }
}
﻿using System;
using System.Windows.Controls;

namespace Client
{
    public class RangeValidationRule : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        
        public override ValidationResult Validate(
          object value, System.Globalization.CultureInfo cultureInfo)
        {
            int intValue;

            string text = String.Format("Must be between {0} and {1}",
                           MinValue, MaxValue);
            if (!Int32.TryParse(value.ToString(), out intValue))
                return new ValidationResult(false, "Not an integer");
            if (intValue < MinValue)
                return new ValidationResult(false, "To small. " + text);
            if (intValue > MaxValue)
                return new ValidationResult(false, "To large. " + text);
            return ValidationResult.ValidResult;
        }
    }
}

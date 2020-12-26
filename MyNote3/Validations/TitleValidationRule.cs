using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyNote3
{
    class TitleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string title = value.ToString();
            if (!string.IsNullOrEmpty(title))
            {                
                Regex illegal = new Regex(".*[\\\\/:*?\"<>\\|]+.*");
                if (!illegal.IsMatch(title))
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "Title illegal");
        }
    }
}

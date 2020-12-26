using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyNote3
{
    class FontSizeValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val;
            bool res = double.TryParse(value.ToString(), out val);
            if (res == true)
            {
                if (val > 0)
                    return new ValidationResult(true, null);
            }           
            return new ValidationResult(false, "FontSize Must be Larger than 0");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MyNote3
{
    class StringToFontFamilyConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            if (val != null)
            {
                FontFamily fontFamily = new FontFamily(val);
                return fontFamily;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FontFamily fontFamily = value as FontFamily;
            if (fontFamily != null)
                return fontFamily.Source;
            return null;
        }
    }
}

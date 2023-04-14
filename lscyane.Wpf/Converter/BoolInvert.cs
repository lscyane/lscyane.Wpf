using System;
using System.Windows.Data;

namespace lscyane.Wpf.Converter
{
    /// <summary>
    /// bool値を反転します。
    /// </summary>
    public class BoolInvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool?)value);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool?)value);
        }


        /// <summary> 値コンバーターの実体 </summary>
        public static BoolInvert Converter = new BoolInvert();
    }
}

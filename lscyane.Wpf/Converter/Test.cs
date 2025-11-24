using System;
using System.Windows.Data;

namespace lscyane.Wpf.Converter
{
    /// <summary>
    /// test
    /// </summary>
    public class Test : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }


        /// <summary> 値コンバーターの実体 </summary>
        public static Test Converter = new Test();
    }
}

using System;
using System.Windows;
using System.Windows.Data;

namespace lscyane.Wpf.Converter
{
    /// <summary>
    /// BoolとCollapsed又はHiddenとの変換を行います
    /// </summary>
    public class BoolToVisibility : IValueConverter
    {
        private bool invert;
        private bool toCollapsed;


        public BoolToVisibility(bool tocol ,bool inv = false)
        {
            this.toCollapsed = tocol;
            this.invert = inv;
        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (this.toCollapsed)
            {
                return ((bool?)value != this.invert) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return ((bool?)value != this.invert) ? Visibility.Visible : Visibility.Hidden;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((Visibility?)value == Visibility.Visible) ? true : false) != this.invert;
        }


        // 値コンバーターの実体
        /// <summary> Bool←→Collapsed変換 </summary>
        public static BoolToVisibility CollapsedConverter = new BoolToVisibility(true);
        /// <summary> Bool←→Collapsed変換(論理反転) </summary>
        public static BoolToVisibility CollapsedInvConverter = new BoolToVisibility(true, true);
        /// <summary> Bool←→Hidden変換 </summary>
        public static BoolToVisibility HiddenConverter = new BoolToVisibility(false);
        /// <summary> Bool←→Hidden変換(論理反転) </summary>
        public static BoolToVisibility HiddenInvConverter = new BoolToVisibility(false, true);
    }
}

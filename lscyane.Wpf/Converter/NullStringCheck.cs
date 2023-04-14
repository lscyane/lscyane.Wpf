using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace lscyane.Wpf.Converter
{
    /// <summary>
    /// stringがNullかどうかbool型で返します
    /// </summary>
    public class NullStringCheck : IValueConverter
    {
        private bool invert;


        public NullStringCheck(bool inv = false)
        {
            this.invert = inv;
        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool retval = false;
            string val = (string)value;
            if ((val != null) && (val != ""))
            {
                retval = true;
            }

            retval = (retval != this.invert);

            return retval;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotSupportedException();
        }


        #region 値コンバーターの実体
        public static NullStringCheck Converter = new NullStringCheck();
        public static NullStringCheck InvConverter = new NullStringCheck(true);
        #endregion
    }
}

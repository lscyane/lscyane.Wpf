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
        private Visibility? visibility;


        public NullStringCheck(bool inv = false, Visibility? visible = null)
        {
            this.invert = inv;
            this.visibility = visible;
        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string val = (string)value;

            if (this.visibility == null)
            {
                bool retval = false;
                if ((val != null) && (val != ""))
                {
                    retval = true;
                }

                retval = (retval != this.invert);

                return retval;
            }
            else
            {
                Visibility vretval =  this.visibility.Value;
                if ((val != null) && (val != ""))
                {
                    vretval = Visibility.Visible;
                }

                if (this.invert)
                {
                    if (vretval == Visibility.Visible)
                    {
                        vretval = this.visibility.Value;
                    }
                    else
                    {
                        vretval = Visibility.Visible;
                    }
                }

                return vretval;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotSupportedException();
        }


        #region 値コンバーターの実体
        public static NullStringCheck Converter = new NullStringCheck();        // null -> false
        public static NullStringCheck InvConverter = new NullStringCheck(true); // null -> true
        public static NullStringCheck HiddenConverter = new NullStringCheck(false, Visibility.Hidden);  // null -> Hidden
        public static NullStringCheck HiddenInvConverter = new NullStringCheck(true, Visibility.Hidden);   // null -> Visible
        public static NullStringCheck CollapsedConverter = new NullStringCheck(false, Visibility.Collapsed);  // null -> Collapsed
        public static NullStringCheck CollapsedInvConverter = new NullStringCheck(true, Visibility.Collapsed);   // null -> Visible
        #endregion
    }
}

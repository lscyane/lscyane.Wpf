using System;
using System.Windows.Data;

namespace lscyane.Wpf.Converter
{
    /// <summary>
    /// bool値をTaskbarItemProgressState値に変換します。
    /// </summary>
    public class BoolToProgressState : IValueConverter
    {
        private bool invert;
        public BoolToProgressState(bool inv = false)
        {
            this.invert = inv;
        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool retval = ((bool?)value != false ? true : false);
            if (this.invert) { retval = !retval; }
            return (retval ? System.Windows.Shell.TaskbarItemProgressState.Normal : System.Windows.Shell.TaskbarItemProgressState.None);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool retval = ((System.Windows.Shell.TaskbarItemProgressState?)value != System.Windows.Shell.TaskbarItemProgressState.None);
            if (this.invert) { retval = !retval; }
            return retval;
        }


        #region 値コンバーターの実体
        public static BoolToProgressState Converter = new BoolToProgressState(false);
        public static BoolToProgressState InvConverter = new BoolToProgressState(true);
        #endregion
    }
}

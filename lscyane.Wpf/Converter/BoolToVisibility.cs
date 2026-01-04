using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace lscyane.Wpf.Converter
{
    /// <summary>
    /// BoolとCollapsed又はHiddenとの変換を行います
    /// </summary>
    public class BoolToVisibility : IValueConverter, IMultiValueConverter
    {
        private readonly bool invert;
        private readonly bool toCollapsed;
        private readonly bool orJudge;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tocol"></param>
        /// <param name="inv"></param>
        /// <param name="or">MultiBinding用</param>
        public BoolToVisibility(bool tocol, bool inv = false, bool or = false)
        {
            this.toCollapsed = tocol;
            this.invert = inv;
            this.orJudge = or;
        }


        #region IValueConverter の実装
        /// <summary>
        /// Convert
        /// </summary>
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


        /// <summary>
        /// ConvertBack
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((Visibility?)value == Visibility.Visible) ? true : false) != this.invert;
        }
        #endregion


        #region IMultiValueConverter の実装
        /// <summary>
        /// 複数のboolをAnd/Or条件でまとめてから、Collapsed又はHiddenとの変換を行います
        /// </summary>
        /// <param name="values">bool配列</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            bool finalValue;
            if (this.orJudge)
            {
                // OR条件：Bindingしたいずれかのbool値がtrueの時、true
                finalValue = false;
                foreach (bool value in values)
                {
                    finalValue |= value;
                }
            }
            else
            {
                // AND条件：Bindingした全てのbool値がtrueの時、true
                finalValue = true;
                foreach (bool value in values)
                {
                    finalValue &= value;
                }
            }

            return Convert(finalValue, targetType, parameter, culture);
        }


        /// <summary>
        /// コンバートバック実装
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion


        // 値コンバーターの実体
        /// <summary> Bool←→Collapsed変換 </summary>
        public static BoolToVisibility CollapsedConverter = new BoolToVisibility(true);
        /// <summary> Bool←→Collapsed変換(論理反転) </summary>
        public static BoolToVisibility CollapsedInvConverter = new BoolToVisibility(true, true);
        /// <summary> Bool←→Hidden変換 </summary>
        public static BoolToVisibility HiddenConverter = new BoolToVisibility(false);
        /// <summary> Bool←→Hidden変換(論理反転) </summary>
        public static BoolToVisibility HiddenInvConverter = new BoolToVisibility(false, true);

        // MultiBinding用
        /// <summary> Bool←→Collapsed変換(And条件) </summary>
        public static BoolToVisibility CollapsedMultiAndConverter { get; } = CollapsedConverter;
        /// <summary> Bool←→Collapsed変換(And条件,論理反転) </summary>
        public static BoolToVisibility CollapsedInvMultiAndConverter { get; } = CollapsedInvConverter;
        /// <summary> Bool←→Hidden変換(And条件) </summary>
        public static BoolToVisibility HiddenMultiAndConverter { get; } = HiddenConverter;
        /// <summary> Bool←→Hidden変換(And条件,論理反転) </summary>
        public static BoolToVisibility HiddenInvMultiAndConverter { get; } = HiddenInvConverter;

        /// <summary> Bool←→Collapsed変換(OR条件) </summary>
        public static BoolToVisibility CollapsedMultiOrConverter { get; } = new(true, false, true);
        /// <summary> Bool←→Collapsed変換(OR条件,論理反転) </summary>
        public static BoolToVisibility CollapsedInvMultiOrConverter { get; } = new(true, true, true);
        /// <summary> Bool←→Hidden変換(OR条件) </summary>
        public static BoolToVisibility HiddenMultiOrConverter { get; } = new(false, false, true);
        /// <summary> Bool←→Hidden変換(OR条件,論理反転) </summary>
        public static BoolToVisibility HiddenInvMultiOrConverter { get; } = new(false, true, true);
    }
}

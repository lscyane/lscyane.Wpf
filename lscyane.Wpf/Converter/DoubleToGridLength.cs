using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace lscyane.Wpf.Converter;

/// <summary>
/// <see cref="double"/> を <see cref="GridLength"/> に変換するコンバーター
/// </summary>
[ValueConversion(typeof(double), typeof(GridLength))]
public class DoubleToGridLength : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            double.NaN => GridLength.Auto,
            double dVal => new GridLength(dVal, GridUnitType.Pixel),
            _ => throw new InvalidOperationException(),
        };
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            GridLength glVal when glVal.IsAuto => double.NaN,
            GridLength glVal when glVal.IsAbsolute => glVal.Value,
            _ => throw new InvalidOperationException(),
        };
    }

    // 値コンバーターの実体
    public static DoubleToGridLength Converter = new DoubleToGridLength();
}

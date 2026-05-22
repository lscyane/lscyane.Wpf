using System;
using System.Windows;
using System.Windows.Controls;

namespace lscyane.Wpf.Controls;


/// <summary>
/// 数値入力コントロール
/// </summary>
public class NumericUpDown : System.Windows.Controls.Control
{
    private TextBox? _textBox;

    static NumericUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
    }


    /// <summary>
    /// 実際の値
    /// </summary>
    public decimal Value
    {
        get { return (decimal)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    /// <summary>依存プロパティ定義</summary>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceValue));

    private static object CoerceValue(DependencyObject d, object baseValue)
    {
        var control = (NumericUpDown)d;
        var value = (decimal)baseValue;
        return Math.Clamp(value, control.MinValue, control.MaxValue);
    }


    /// <summary>
    /// 表示値の変化量
    /// </summary>
    public decimal ValueStep
    {
        get { return (decimal)GetValue(ValueStepProperty); }
        set { SetValue(ValueStepProperty, value); }
    }
    /// <summary>依存プロパティ定義</summary>
    public static readonly DependencyProperty ValueStepProperty =
        DependencyProperty.Register("ValueStep",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(1m));


    /// <summary>
    /// 値の最大値
    /// </summary>
    public decimal MaxValue
    {
        get { return (decimal)GetValue(MaxValueProperty); }
        set { SetValue(MaxValueProperty, value); }
    }
    /// <summary>依存プロパティ定義</summary>
    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register("MaxValue",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(decimal.MaxValue, (d, _) => ((NumericUpDown)d).CoerceValue(ValueProperty)));


    /// <summary>
    /// 値の最小値
    /// </summary>
    public decimal MinValue
    {
        get { return (decimal)GetValue(MinValueProperty); }
        set { SetValue(MinValueProperty, value); }
    }
    /// <summary>依存プロパティ定義</summary>
    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register("MinValue",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(decimal.MinValue, (d, _) => ((NumericUpDown)d).CoerceValue(ValueProperty)));


    /// <summary>
    /// 内部テキストボックスの最大入力文字数
    /// </summary>
    public int MaxLength
    {
        get { return (int)GetValue(MaxLengthProperty); }
        set { SetValue(MaxLengthProperty, value); }
    }
    /// <summary>依存プロパティ定義</summary>
    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register("MaxLength",
            typeof(int),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(0, OnMaxLengthChanged));

    private static void OnMaxLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (NumericUpDown)d;
        if (control._textBox is not null)
            control._textBox.MaxLength = (int)e.NewValue;
    }


    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_TextBox") is TextBox tb)
        {
            _textBox = tb;
            _textBox.MaxLength = this.MaxLength;
        }

        if (GetTemplateChild("PART_UpButton") is Button upBtn)
        {
            upBtn.Click += Up_Button_Click;
        }
        if (GetTemplateChild("PART_DownButton") is Button dnBtn)
        {
            dnBtn.Click += Down_Button_Click;
        }
    }


    /// <summary>
    /// Upボタン押下 - インクリメント
    /// </summary>
    private void Up_Button_Click(object sender, RoutedEventArgs e)
    {
        this.Value = Math.Min(this.Value + this.ValueStep, this.MaxValue);
    }


    /// <summary>
    /// Downボタン押下 - デクリメント
    /// </summary>
    private void Down_Button_Click(object sender, RoutedEventArgs e)
    {
        this.Value = Math.Max(this.Value - this.ValueStep, this.MinValue);
    }
}

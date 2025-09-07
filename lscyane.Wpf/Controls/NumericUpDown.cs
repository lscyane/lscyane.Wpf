using System;
using System.Windows;
using System.Windows.Controls;

namespace lscyane.Wpf.Controls;


/// <summary>
/// 数値入力コントロール
/// </summary>
public class NumericUpDown : System.Windows.Controls.Control
{
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
            new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


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
            new FrameworkPropertyMetadata(decimal.MaxValue));


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
            new FrameworkPropertyMetadata(decimal.MinValue));


    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

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

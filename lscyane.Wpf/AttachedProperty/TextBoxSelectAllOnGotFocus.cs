using System;
using System.Windows;
using System.Windows.Controls;

namespace lscyane.Wpf.AttachedProperty
{
    /// <summary>
    /// TextBoxにフォーカスが当たった時にテキストを全選択する
    /// </summary>
    /// <example>
    ///     <Control xmlns:lw="http://cyane.info/wpf">
    ///         <TextBox lw:TextBoxSelectAllOnGotFocus.Enabled="True" />
    ///     </Control>
    /// </example>
    public static class TextBoxSelectAllOnGotFocus
    {
        /// <summary>
        /// TextBoxにフォーカスが当たった時にテキストを全選択する設定の添付プロパティ定義
        /// </summary>
        public static readonly DependencyProperty EnabledProperty =
                DependencyProperty.RegisterAttached(
                        "Enabled",
                        typeof(bool),
                        typeof(TextBoxSelectAllOnGotFocus),
                        new UIPropertyMetadata(false, EnabledChanged));
        /// <summary> 依存関係プロパティのローカル値を取得します。 </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetEnabled(DependencyObject obj) => (bool)obj.GetValue(EnabledProperty);
        /// <summary> 依存関係プロパティのローカル値を設定します。 </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetEnabled(DependencyObject obj, bool value) => obj.SetValue(EnabledProperty, value);


        private static void EnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs evt)
        {
            if (sender is not TextBox textBox) return;

            textBox.GotFocus -= OnTextBoxGotFocus;
            if ((bool)evt.NewValue)
            {
                textBox.GotFocus += OnTextBoxGotFocus;
                textBox.PreviewMouseDown += TextBox_PreviewMouseDown;
            }
        }


        private static void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // GotFocus イベントの後にマウスのクリックによるカーソル位置の設定が行われてしまうので、Handled を設定してから手動でフォーカスを得る。
                if (!textBox.IsKeyboardFocused)
                {
                    e.Handled = true;
                    textBox.Focus();
                }
            }
        }

        private static void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) throw new ArgumentException($"引数{nameof(sender)}の型は{nameof(TextBox)}で有る必要があります。", nameof(sender));
            textBox.Dispatcher.BeginInvoke(() => textBox.SelectAll());
        }
    }
}


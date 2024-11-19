using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace lscyane.Wpf.AttachedProperty
{
    /// <summary>
    /// 右クリックメニューにラジオボタン動作をするチェック表示を提供します
    /// </summary>
    /// <example>
    ///     XAMLにてMenuItemにGroupNameを設定します
    ///     <MenuItem Header = "10%" IsChecked="{Binding Opacity10}" lwa:MenuItemExtension.GroupName="Opacity" IsCheckable="True"/>
    ///     <MenuItem Header = "20%" IsChecked="{Binding Opacity20}" lwa:MenuItemExtension.GroupName="Opacity" IsCheckable="True"/>
    ///     <MenuItem Header = "30%" IsChecked="{Binding Opacity30}" lwa:MenuItemExtension.GroupName="Opacity" IsCheckable="True"/>
    /// </example>
    public class ToggleableMenuItem : DependencyObject
    {
        public static Dictionary<MenuItem, string> ElementToGroupNames = new Dictionary<MenuItem, string>();


        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.RegisterAttached("GroupName",
                typeof(string),
                typeof(ToggleableMenuItem),
                new PropertyMetadata(string.Empty, OnGroupNameChanged));

        public static string? GetGroupName(MenuItem element)
        {
            return element.GetValue(GroupNameProperty).ToString();
        }

        public static void SetGroupName(MenuItem element, string value)
        {
            element.SetValue(GroupNameProperty, value);
        }


        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // グループ名コレクションにエントリを追加します
            if (d is MenuItem menuItem)
            {
                string? newGroupName = e.NewValue.ToString();
                string? oldGroupName = e.OldValue.ToString();
                if (string.IsNullOrEmpty(newGroupName))
                {
                    // グループ化からトグル ボタンを削除する
                    RemoveCheckboxFromGrouping(menuItem);
                }
                else
                {
                    // 新しいグループへの切り替え
                    if (newGroupName != oldGroupName)
                    {
                        if (!string.IsNullOrEmpty(oldGroupName))
                        {
                            // 古いグループ マッピングを削除する
                            RemoveCheckboxFromGrouping(menuItem);
                        }
                        ElementToGroupNames.Add(menuItem, e.NewValue.ToString() ?? string.Empty);
                        menuItem.Checked += MenuItemChecked;
                    }
                }
            }
        }


        private static void RemoveCheckboxFromGrouping(MenuItem checkBox)
        {
            ElementToGroupNames.Remove(checkBox);
            checkBox.Checked -= MenuItemChecked;
        }


        static void MenuItemChecked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem menuItem)
            {
                foreach (var item in ElementToGroupNames)
                {
                    if (item.Key != menuItem && item.Value == GetGroupName(menuItem))
                    {
                        item.Key.IsChecked = false;
                    }
                }
            }
        }
    }
}
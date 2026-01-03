using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lscyane.Wpf.Behavior;

/// <summary>
/// ListBox派生系にアイテムが追加されたときに自動でスクロールするBehavior
/// </summary>
public class ItemsControlAutoScroll : Behavior<ListBox>
{
    private INotifyCollectionChanged? _currentCollection;
    private DependencyPropertyDescriptor? _itemsSourceDescriptor;


    /// <inheritdoc/>
    protected override void OnAttached()
    {
        base.OnAttached();

        this.AssociatedObject.Loaded += OnAssociatedObjectLoaded;

        // ListBox は ItemsSource の差し替え時に CollectionChanged を自動でつなぎ替えないため、PropertyDescriptor で監視する。
        _itemsSourceDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
        _itemsSourceDescriptor?.AddValueChanged(this.AssociatedObject, OnItemsSourcePropertyChanged);

        SubscribeToCollection(this.AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }


    /// <summary>
    /// ロード後に実際の ItemsSource を再取得して安全に購読し直します。
    /// </summary>
    private void OnAssociatedObjectLoaded(object? sender, RoutedEventArgs e)
    {
        SubscribeToCollection(this.AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }


    /// <summary>
    /// ItemsSource 差し替え時にコレクション購読を更新します。
    /// </summary>
    private void OnItemsSourcePropertyChanged(object? sender, EventArgs e)
    {
        SubscribeToCollection(this.AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }


    /// <summary>
    /// ItemsSource が INotifyCollectionChanged なら CollectionChanged を購読します。
    /// </summary>
    private void SubscribeToCollection(INotifyCollectionChanged? newCollection)
    {
        // 既存購読を解除してから新しい購読を張ることで多重購読を防止。
        if (_currentCollection != null)
        {
            _currentCollection.CollectionChanged -= LogDatas_CollectionChanged;
        }

        _currentCollection = newCollection;

        if (_currentCollection != null)
        {
            _currentCollection.CollectionChanged += LogDatas_CollectionChanged;
        }
    }


    /// <summary>
    /// 追加イベントのみを捕捉し、末尾に近い場合にスクロールを実行します。
    /// </summary>
    private void LogDatas_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Add) return;

        // Dispatcher 経由で UI スレッドに戻し、ScrollViewer を安全に操作する。
        this.AssociatedObject.Dispatcher.Invoke(() =>
        {
            var scrollViewer = GetScrollViewer(this.AssociatedObject);
            if (scrollViewer == null) return;

            bool isAtBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;
            if (isAtBottom && e.NewItems != null)
            {
                // 末尾へスクロールすることで最新アイテムを表示。
                this.AssociatedObject.ScrollIntoView(e.NewItems[^1]);
            }
        });
    }


    /// <summary>
    /// VisualTree を辿って ScrollViewer を取得します。
    /// </summary>
    private ScrollViewer? GetScrollViewer(DependencyObject depObj)
    {
        if (depObj is ScrollViewer viewer) return viewer;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);
            var result = GetScrollViewer(child);
            if (result != null) return result;
        }

        return null;
    }


    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        SubscribeToCollection(null);

        if (_itemsSourceDescriptor != null)
        {
            _itemsSourceDescriptor.RemoveValueChanged(this.AssociatedObject, OnItemsSourcePropertyChanged);
            _itemsSourceDescriptor = null;
        }

        this.AssociatedObject.Loaded -= OnAssociatedObjectLoaded;

        base.OnDetaching();
    }
}

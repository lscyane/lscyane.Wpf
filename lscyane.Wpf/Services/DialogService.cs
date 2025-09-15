using lscyane.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace lscyane.Wpf.Services;


/// <summary>
/// IDialogService の実装
/// </summary>
public class DialogService : IDialogService
{
    public static DialogService? Main { get; set; } = null;


    Window Owner;


    /// <summary>
    /// 登録されたダイアログの管理。Key=View, Value=ViewModel
    /// </summary>
    Dictionary<Type,Type> DialogDefinitions = new Dictionary<Type, Type>();


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="owner">オーナーウインドウのインスタンス</param>
    public DialogService(Window owner)
    {
        this.Owner = owner;
    }


    /// <summary>
    /// ダイアログの登録
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public void RegisterDialog<TView, TViewModel>()
    {
        this.DialogDefinitions.Add(typeof(TView), typeof(TViewModel));
    }


    /// <summary>
    /// ダイアログの表示
    /// </summary>
    /// <param name="vm_type"></param>
    /// <param name="parameter"></param>
    /// <param name="result_action"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Show(Type vm_type, DialogParameters? parameter = null, Action<object>? result_action = null)
    {
        // name に対応する ViewModel を探す
        var dialog = DialogDefinitions.FirstOrDefault(kvp => kvp.Value == vm_type);
        if ((dialog.Value == null)
         || (dialog.Key == null)
        ) {
            throw new InvalidOperationException($"Dialog with name '{vm_type}' is not registered.");
        }

        // ダイアログのインスタンスを生成
        if (Activator.CreateInstance(dialog.Key) is not Window d_view)
        {
            throw new InvalidOperationException($"Failed to create dialog of type '{dialog.Key.FullName}'.");
        }
        if (Activator.CreateInstance(dialog.Value) is not DialogViewModelBase d_viewmodel)
        {
            throw new InvalidOperationException($"Failed to create dialog of type '{dialog.Value.FullName}'.");
        }
        d_view.DataContext = d_viewmodel;

        // ダイアログのオーナーを設定
        if (this.Owner.IsLoaded)
        {
            d_view.Owner = this.Owner;
        }

        d_viewmodel.OnDialogPreviewOpen(parameter);

        // ダイアログを表示
        d_view.Show();

        // 結果を処理
        result_action?.Invoke(new object());
    }
}

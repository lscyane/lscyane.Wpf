using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace lscyane.Wpf.Extension
{
    /// <summary>
    /// イベントをトリガーにコマンドを発行する機能を提供します
    /// </summary>
    /// <remarks>
    /// XAML上でイベントをトリガーとして、任意のコマンドを呼び出せるようにするマークアップ拡張。
    /// ViewとViewModelを疎結合に保ち、コードビハインドを減らす用途に使う。
    /// </remarks>
    [MarkupExtensionReturnType(typeof(EventHandler))]
    public sealed class InvokeCommand : MarkupExtension
    {
        /// <summary>
        /// イベント発生時の呼び出すコマンドのパスを取得または設定します。
        /// </summary>
        public string BindingCommandPath { get; set; }


        // 実際に実行するコマンドの参照
        private ICommand? _targetCommand;


        /// <summary>
        /// コマンドのパスを指定して初期化
        /// </summary>
        /// <param name="bindingCommandPath">イベント発生時に呼び出すコマンドのパス</param>
        public InvokeCommand(string bindingCommandPath)
        {
            this.BindingCommandPath = bindingCommandPath;
        }


        /// <summary>
        /// マークアップ拡張の本体。
        /// XAML解析時に呼ばれ、イベントハンドラを生成して返す。
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            // XAMLの対象オブジェクトやプロパティにアクセスするためのサービスを取得
            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (pvt != null)
            {
                // TargetProperty がイベントか、メソッドかを判定
                var ei = pvt.TargetProperty as EventInfo;
                var mi = pvt.TargetProperty as MethodInfo;

                // イベントハンドラの型（デリゲート型）を取得
                Type? type = null;
                if (ei != null)
                {
                    type = ei.EventHandlerType;
                }
                else if (mi != null)
                {
                    type = mi.GetParameters()[1].ParameterType;
                }

                if (type != null)
                {
                    var target = pvt.TargetObject as FrameworkElement;

                    // DataContextを基準にコマンドを取得
                    this._targetCommand = ParsePropertyPath(target?.DataContext, this.BindingCommandPath) as ICommand;

                    // PrivateHandlerGeneric<T> を反射で呼び出す準備
                    var nonGenericMethod = GetType().GetMethod("PrivateHandlerGeneric", BindingFlags.NonPublic | BindingFlags.Instance);
                    var argType = type.GetMethod("Invoke")?.GetParameters()[1].ParameterType;

                    if (argType != null)
                    {
                        var genericMethod = nonGenericMethod?.MakeGenericMethod(argType);
                        if (genericMethod != null)
                        {
                            // イベントに対応するデリゲートを生成して返す
                            return Delegate.CreateDelegate(type, this, genericMethod);
                        }
                    }
                }

            }

            // 取得に失敗した場合はnullを返す
            return null;
        }


        /// <summary>
        /// イベント発生時に呼ばれる汎用ハンドラ。
        /// ジェネリック型でイベント引数を受け取り、コマンドを実行する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrivateHandlerGeneric<T>(object sender, T e)
        {
            // コマンドを呼び出す
            if (this._targetCommand != null && this._targetCommand.CanExecute(e))
            {
                this._targetCommand.Execute(e);
            }
        }


        /// <summary>
        /// target引数で渡されたオブジェクトに対し、pathで示されたプロパティをリフレクションを用いて取得します。
        /// </summary>
        /// <param name="target">探索の起点となるオブジェクト（通常はViewModel）</param>
        /// <param name="path">プロパティ名を"."区切りで指定したパス</param>
        /// <returns>プロパティの値。存在しない場合や target が null の場合は null。</returns>
        static object? ParsePropertyPath(object? target, string path)
        {
            // DataContextがnullの場合の対処
            if (target == null)
            {
                return null;
            }

            // "." 区切りでプロパティを順に辿って値を取得
            var props = path.Split('.');
            foreach (var prop in props)
            {
                target = target?.GetType().GetProperty(prop)?.GetValue(target);
            }
            return target;
        }
    }
}

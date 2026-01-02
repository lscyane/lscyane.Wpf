using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace lscyane.Wpf.Controls;

/// <summary>
/// オーナーウィンドウの中央に表示するメッセージボックス
/// </summary>
public static class OwneredMessageBox
{
    /// <summary>
    /// オーナー未指定でメッセージを表示します。
    /// </summary>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(string messageBoxText) =>
        ShowCore(null, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);


    /// <summary>
    /// オーナー未指定でキャプション付きメッセージを表示します。
    /// </summary>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(string messageBoxText, string caption) =>
        ShowCore(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);


    /// <summary>
    /// オーナー未指定でボタン種類を指定したメッセージを表示します。
    /// </summary>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button) =>
        ShowCore(null, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);


    /// <summary>
    /// オーナー未指定でアイコンを指定したメッセージを表示します。
    /// </summary>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <param name="icon">ダイアログに表示するアイコン種別。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon) =>
        ShowCore(null, messageBoxText, caption, button, icon, MessageBoxResult.OK, MessageBoxOptions.None);


    /// <summary>
    /// オーナー未指定で既定ボタンを指定したメッセージを表示します。
    /// </summary>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <param name="icon">ダイアログに表示するアイコン種別。</param>
    /// <param name="defaultResult">初期選択状態とするボタン。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult) =>
        ShowCore(null, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);


    /// <summary>
    /// オーナー未指定で詳細オプションを指定したメッセージを表示します。
    /// </summary>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <param name="icon">ダイアログに表示するアイコン種別。</param>
    /// <param name="defaultResult">初期選択状態とするボタン。</param>
    /// <param name="options">表示に関する追加オプション。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options) =>
        ShowCore(null, messageBoxText, caption, button, icon, defaultResult, options);


    /// <summary>
    /// 明示的に指定したオーナーとともにメッセージを表示します。
    /// </summary>
    /// <param name="owner">ダイアログのオーナーとなる <see cref="Window"/>。</param>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(Window owner, string messageBoxText)
    {
        ArgumentNullException.ThrowIfNull(owner);
        return ShowCore(owner, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);
    }


    /// <summary>
    /// 明示的に指定したオーナーとキャプション付きメッセージを表示します。
    /// </summary>
    /// <param name="owner">ダイアログのオーナーとなる <see cref="Window"/>。</param>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
    {
        ArgumentNullException.ThrowIfNull(owner);
        return ShowCore(owner, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);
    }


    /// <summary>
    /// 明示的に指定したオーナーでボタン種類を指定したメッセージを表示します。
    /// </summary>
    /// <param name="owner">ダイアログのオーナーとなる <see cref="Window"/>。</param>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
    {
        ArgumentNullException.ThrowIfNull(owner);
        return ShowCore(owner, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);
    }


    /// <summary>
    /// 明示的に指定したオーナーでアイコンを指定したメッセージを表示します。
    /// </summary>
    /// <param name="owner">ダイアログのオーナーとなる <see cref="Window"/>。</param>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <param name="icon">ダイアログに表示するアイコン種別。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        ArgumentNullException.ThrowIfNull(owner);
        return ShowCore(owner, messageBoxText, caption, button, icon, MessageBoxResult.OK, MessageBoxOptions.None);
    }


    /// <summary>
    /// 明示的に指定したオーナーで既定ボタンを指定したメッセージを表示します。
    /// </summary>
    /// <param name="owner">ダイアログのオーナーとなる <see cref="Window"/>。</param>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <param name="icon">ダイアログに表示するアイコン種別。</param>
    /// <param name="defaultResult">初期選択状態とするボタン。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
    {
        ArgumentNullException.ThrowIfNull(owner);
        return ShowCore(owner, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
    }


    /// <summary>
    /// 明示的に指定したオーナーで詳細オプションを指定したメッセージを表示します。
    /// </summary>
    /// <param name="owner">ダイアログのオーナーとなる <see cref="Window"/>。</param>
    /// <param name="messageBoxText">表示するメッセージ本文。</param>
    /// <param name="caption">タイトルバーに表示する文字列。</param>
    /// <param name="button">表示するボタンの種類。</param>
    /// <param name="icon">ダイアログに表示するアイコン種別。</param>
    /// <param name="defaultResult">初期選択状態とするボタン。</param>
    /// <param name="options">表示に関する追加オプション。</param>
    /// <returns>ユーザーが選択した <see cref="MessageBoxResult"/>。</returns>
    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
    {
        ArgumentNullException.ThrowIfNull(owner);
        return ShowCore(owner, messageBoxText, caption, button, icon, defaultResult, options);
    }


    private static MessageBoxResult ShowCore(Window? owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
    {
        var resolvedOwner = ShouldSkipOwnerResolution(options) ? owner : owner ?? FindOwnerWindow();
        using var hook = new MessageBoxCenteringHook(resolvedOwner);
        var text = messageBoxText ?? string.Empty;
        var title = caption ?? string.Empty;

        return resolvedOwner != null
            ? MessageBox.Show(resolvedOwner, text, title, button, icon, defaultResult, options)
            : MessageBox.Show(text, title, button, icon, defaultResult, options);
    }


    private static bool ShouldSkipOwnerResolution(MessageBoxOptions options) =>
        (options & (MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification)) != 0;


    private static Window? FindOwnerWindow()
    {
        var application = Application.Current;
        if (application == null)
        {
            return null;
        }

        var activeWindow = application.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive && w.IsVisible);
        if (activeWindow != null)
        {
            return activeWindow;
        }

        if (application.MainWindow?.IsVisible == true)
        {
            return application.MainWindow;
        }

        return application.Windows.OfType<Window>().FirstOrDefault(w => w.IsVisible);
    }


    private sealed class MessageBoxCenteringHook : IDisposable
    {
        private const int WhCbt = 5;
        private const int HcbtActivate = 5;

        private readonly IntPtr _ownerHandle;
        private readonly HookProc _hookProc;
        private IntPtr _hookHandle;

        public MessageBoxCenteringHook(Window? owner)
        {
            _ownerHandle = owner != null ? EnsureHandle(owner) : IntPtr.Zero;
            _hookProc = HookProc;

            if (_ownerHandle != IntPtr.Zero)
            {
                _hookHandle = SetWindowsHookEx(WhCbt, _hookProc, IntPtr.Zero, GetCurrentThreadId());
            }
        }

        public void Dispose()
        {
            if (_hookHandle == IntPtr.Zero)
            {
                return;
            }

            UnhookWindowsHookEx(_hookHandle);
            _hookHandle = IntPtr.Zero;
        }

        private IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code == HcbtActivate && _hookHandle != IntPtr.Zero)
            {
                CenterWindow(wParam);
                Dispose();
            }

            return CallNextHookEx(_hookHandle, code, wParam, lParam);
        }

        private void CenterWindow(IntPtr messageBoxHandle)
        {
            if (_ownerHandle == IntPtr.Zero || messageBoxHandle == IntPtr.Zero)
            {
                return;
            }

            if (!GetWindowRect(_ownerHandle, out var ownerRect) || !GetWindowRect(messageBoxHandle, out var messageBoxRect))
            {
                return;
            }

            // オーナーウィンドウの中央位置を計算
            var ownerWidth = ownerRect.Right - ownerRect.Left;
            var ownerHeight = ownerRect.Bottom - ownerRect.Top;
            var boxWidth = messageBoxRect.Right - messageBoxRect.Left;
            var boxHeight = messageBoxRect.Bottom - messageBoxRect.Top;

            var targetX = ownerRect.Left + (ownerWidth - boxWidth) / 2;
            var targetY = ownerRect.Top + (ownerHeight - boxHeight) / 2;

            // 画面外に出ないように調整
            var workArea = GetWorkArea(_ownerHandle);
            var maxX = workArea.Right - boxWidth;
            var maxY = workArea.Bottom - boxHeight;
            var adjustedX = Math.Clamp(targetX, workArea.Left, Math.Max(workArea.Left, maxX));
            var adjustedY = Math.Clamp(targetY, workArea.Top, Math.Max(workArea.Top, maxY));

            MoveWindow(messageBoxHandle, adjustedX, adjustedY, boxWidth, boxHeight, false);
        }


        private static NativeRect GetWorkArea(IntPtr referenceHandle)
        {
            var monitor = MonitorFromWindow(referenceHandle, MonitorDefaultToNearest);
            if (monitor == IntPtr.Zero)
            {
                return new NativeRect
                {
                    Left = 0,
                    Top = 0,
                    Right = (int)SystemParameters.WorkArea.Width,
                    Bottom = (int)SystemParameters.WorkArea.Height
                };
            }

            var info = new MonitorInfo
            {
                cbSize = Marshal.SizeOf<MonitorInfo>()
            };

            return GetMonitorInfo(monitor, ref info) ? info.rcWork : new NativeRect
            {
                Left = 0,
                Top = 0,
                Right = (int)SystemParameters.WorkArea.Width,
                Bottom = (int)SystemParameters.WorkArea.Height
            };
        }


        private static IntPtr EnsureHandle(Window window)
        {
            var helper = new WindowInteropHelper(window);
            var handle = helper.Handle;
            return handle != IntPtr.Zero ? handle : helper.EnsureHandle();
        }
    }


    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);


    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern int GetCurrentThreadId();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowRect(IntPtr hWnd, out NativeRect lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

    private const uint MonitorDefaultToNearest = 0x00000002;

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MonitorInfo
    {
        public int cbSize;
        public NativeRect rcMonitor;
        public NativeRect rcWork;
        public int dwFlags;
    }
}

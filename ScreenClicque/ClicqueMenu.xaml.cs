using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ScreenClicque;

public partial class ClicqueMenu : Window
{
    public delegate void BackgroundClickHandler();
    public event BackgroundClickHandler? BackgroundClick;
    
    public ClicqueMenu()
    {
        InitializeComponent();
        QuitMenuItem.Click += (s, e) => Application.Current.Shutdown(0);
        SettingsMenuItem.Click += (s, e) => new SettingsWindow().Show();
        
        KeyDown += (s, e) =>
        {
            if (e.Key == Key.Escape)
            {
                Hide();
            }
        };

        IsKeyboardFocusedChanged += (s, e) =>
        {
            if (!IsKeyboardFocused)
            {
                Hide();
            }
        };
    }
    
    
    #region WinApi and base window functionality
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    
    
    private const int HOTKEY_ID = 9000;

    //Modifiers:
    private const uint MOD_NONE = 0x0000; //(none)
    private const uint MOD_ALT = 0x0001; //ALT
    private const uint MOD_CONTROL = 0x0002; //CTRL
    private const uint MOD_SHIFT = 0x0004; //SHIFT
    private const uint MOD_WIN = 0x0008; //WINDOWS
    //CAPS LOCK:
    private const uint VK_CAPITAL = 0x14;
    private const uint VK_SPACE = 0x20;
    
    private IntPtr _windowHandle;
    private HwndSource _source;
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        _windowHandle = new WindowInteropHelper(this).Handle;
        _source = HwndSource.FromHwnd(_windowHandle);
        _source.AddHook(HwndHook);

        RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_ALT, VK_SPACE);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;
        switch (msg)
        {
            case WM_HOTKEY:
                switch (wParam.ToInt32())
                {
                    case HOTKEY_ID:
                        handled = true;
                        BackgroundClick?.Invoke();
                        break;
                }
                break;
        }
        return IntPtr.Zero;
    }

    protected override void OnClosed(EventArgs e)
    {
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        base.OnClosed(e);
    }
    #endregion
}
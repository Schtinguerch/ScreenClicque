using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ScreenClicque
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            
            var clickMenu = new ClicqueMenu();
            clickMenu.Show();

            var controller = new HotkeyController(clickMenu);
            clickMenu.Hide();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) 
        {
            var errorMessage = $"Exception: {e.Exception.Message}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}
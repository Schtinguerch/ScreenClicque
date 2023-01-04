using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenClicque
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var clickMenu = new ClicqueMenu();
            clickMenu.Show();

            var controller = new HotkeyController(clickMenu);
            clickMenu.Hide();
        }
    }
}
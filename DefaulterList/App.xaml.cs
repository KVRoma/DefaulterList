using DefaulterList.ViewModels;
using DefaulterList.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DefaulterList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainViewModel, MainView>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainViewModel = new MainViewModel();
            displayRootRegistry.ShowPresentation(mainViewModel);

        }

        protected override void OnExit(ExitEventArgs e)
        {
            displayRootRegistry.UnregisterWindowType<MainViewModel>();

            base.OnExit(e);
        }
    }
}

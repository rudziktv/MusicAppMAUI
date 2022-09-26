using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    class SideViewModel : BaseViewModel
    {
        private bool _isDarkModeEnabled;

        public bool IsDarkModeEnabled
        {
            get { return _isDarkModeEnabled; }
            set
            { 
                _isDarkModeEnabled = value;
                //OnPropertyChanged(nameof(IsDarkModeEnabled));
                Application.Current.UserAppTheme = IsDarkModeEnabled ? AppTheme.Dark : AppTheme.Light;
            }
        }

        public Command GoBackCommand { get; set; }
        public Command SwitchThemeCommand { get; set; }

        public SideViewModel()
        {
            IsDarkModeEnabled = Application.Current.RequestedTheme == AppTheme.Dark;
            
            GoBackCommand = new(() => Shell.Current.GoToAsync("//HomePage"));
        }
    }
}

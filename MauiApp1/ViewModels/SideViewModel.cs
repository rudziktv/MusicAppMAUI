using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                try
                {
                    Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
                }
                catch (TargetInvocationException)
                {
                    Shell.Current.DisplayAlert("Error", "Theme error, i don't know why.", "Ok");
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                }
                finally
                {
                    _isDarkModeEnabled = Application.Current.RequestedTheme == AppTheme.Dark;
                    OnPropertyChanged(nameof(IsDarkModeEnabled));
                }
                //OnPropertyChanged(nameof(IsDarkModeEnabled));
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

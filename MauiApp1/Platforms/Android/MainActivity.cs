using Android.App;
using Android.Content.PM;
using Android.OS;
using MauiApp1.Views;

namespace MauiApp1;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public override void OnBackPressed()
    {
        if (Shell.Current.CurrentPage is SidePage)
        {
            Shell.Current.GoToAsync("//HomePage");
        }
        else if (Shell.Current.CurrentPage is PlayerPage)
        {
            Shell.Current.GoToAsync("//HomePage");
        }
        else if (Shell.Current.CurrentPage is PlaylistPage)
        {
            Shell.Current.GoToAsync("//LibraryPage");
        }
        else
        {
            base.OnBackPressed();
        }
    }
}

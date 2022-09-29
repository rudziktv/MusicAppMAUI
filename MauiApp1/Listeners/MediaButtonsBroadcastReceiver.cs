using Android.App;
using Android.Content;
using Android.Media;
using Android.Views;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Listeners
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionMediaButton, AudioManager.ActionAudioBecomingNoisy })]
    internal class MediaButtonsBroadcastReceiver : BroadcastReceiver
    {
        public string ComponentName { get { return Class.Name; } }

        public override async void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionMediaButton)
            {
                var keyEvent = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);

                switch (keyEvent.KeyCode)
                {
                    case Keycode.MediaPlay:
                        await Shell.Current.DisplayAlert("MediaPlay", "Button Detected", "Ok");
                        break;
                    case Keycode.MediaPlayPause:
                        await Shell.Current.DisplayAlert("MediaPlayPause", "Button Detected", "Ok");
                        break;
                    case Keycode.MediaNext:
                        await Shell.Current.DisplayAlert("MediaNext", "Button Detected", "Ok");
                        break;
                    case Keycode.MediaPrevious:
                        await Shell.Current.DisplayAlert("MediaPrevious", "Button Detected", "Ok");
                        break;
                }
            }
            else if (intent.Action == AudioManager.ActionAudioBecomingNoisy)
            {
                GlobalData.GlobalPlayer.Pause();
            }
        }
    }
}

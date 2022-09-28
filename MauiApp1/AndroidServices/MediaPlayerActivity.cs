using Android.Content;
using Android.Media;
using Android.OS;
using Android.OS.Strictmode;
using Android.Support.V4.Media;
using Android.Support.V4.Media.Session;
using AndroidX.AppCompat.App;
using AndroidX.Media;
using MauiApp1.Listeners;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MauiApp1.AndroidServices
{
    internal class MediaPlayerActivity : AppCompatActivity
    {
        private MediaBrowserCompat mediaBrowser;
        private MediaBrowserCompat.ConnectionCallback connectionCallbacks = new MyConnectionCallback();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mediaBrowser = new MediaBrowserCompat(this,
              new ComponentName(this, new MediaPlayerService().Class.Name),
                connectionCallbacks,
                null);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mediaBrowser.Connect();
        }

        protected override void OnResume()
        {
            base.OnResume();            
            VolumeControlStream = Android.Media.Stream.Music;
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (MediaControllerCompat.GetMediaController(this) != null)
            {
                MediaControllerCompat.GetMediaController(this).UnregisterCallback(controllerCallback);
            }
            mediaBrowser.Disconnect();
        }
    }
}

using Android.Media.Browse;
using Android.Support.V4.Media;
using Android.Support.V4.Media.Session;
using MauiApp1.AndroidServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Listeners
{
    internal class MyConnectionCallback : MediaBrowserCompat.ConnectionCallback
    {
        private MediaPlayerActivity context;
        private MediaBrowserCompat mediaBrowser;

        public MyConnectionCallback(MediaPlayerActivity context, MediaBrowserCompat mediaBrowser)
        {
            this.context = context;
            this.mediaBrowser = mediaBrowser;
        }

        public override void OnConnected()
        {
            MediaSessionCompat.Token token = mediaBrowser.SessionToken;
            MediaControllerCompat mediaController = new(context, token);
            MediaControllerCompat.SetMediaController(context, mediaController);
            BuildTransportControls();
        }
    }
}

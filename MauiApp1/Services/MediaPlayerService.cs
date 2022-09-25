using Android.Content;
using Android.OS;
using Android.Support.V4.Media.Session;
using Android.Text;
using AndroidX.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Media.Browse.MediaBrowser;

namespace MauiApp1.Services
{
    /*
    class MediaPlayerService : MediaBrowserServiceCompat
    {
        private const string MY_MEDIA_ROOT_ID = "media_root_id";
        private const string MY_EMPTY_MEDIA_ROOT_ID = "empty_root_id";

        private MediaSessionCompat mediaSession;
        private PlaybackStateCompat.Builder stateBuilder;

        public override BrowserRoot OnGetRoot(string clientPackageName, int clientUid, Bundle rootHints)
        {
            // (Optional) Control the level of access for the specified package name.
            // You'll need to write your own logic to do this.
            if (allowBrowsing(clientPackageName, clientUid))
            {
                // Returns a root ID that clients can use with onLoadChildren() to retrieve
                // the content hierarchy.
                return new BrowserRoot(MY_MEDIA_ROOT_ID, null);
            }
            else
            {
                // Clients can connect, but this BrowserRoot is an empty hierachy
                // so onLoadChildren returns nothing. This disables the ability to browse for content.
                return new BrowserRoot(MY_EMPTY_MEDIA_ROOT_ID, null);
            }
        }

        public override void OnLoadChildren(string parentId, Result result)
        {
            //  Browsing not allowed
            if (TextUtils.Equals(MY_EMPTY_MEDIA_ROOT_ID, parentId))
            {
                result.SendResult(null);
                return;
            }

            // Assume for example that the music catalog is already loaded/cached.

            List<MediaItem> mediaItems = new();

            // Check if this is the root menu:
            if (MY_MEDIA_ROOT_ID.Equals(parentId))
            {
                // Build the MediaItem objects for the top level,
                // and put them in the mediaItems list...
            }
            else
            {
                // Examine the passed parentMediaId to see which submenu we're at,
                // and put the children of that menu in the mediaItems list...
            }
            result.SendResult(mediaItems);
        }

        public override void OnCreate()
        {
            base.OnCreate();

            mediaSession = new MediaSessionCompat(MauiProgram.context, LOG_TAG);

            // Enable callbacks from MediaButtons and TransportControls
            mediaSession.SetFlags(
                  MediaSessionCompat.FLAG_HANDLES_MEDIA_BUTTONS |
                  MediaSessionCompat.FLAG_HANDLES_TRANSPORT_CONTROLS);

            // Set an initial PlaybackState with ACTION_PLAY, so media buttons can start the player
            stateBuilder = new PlaybackStateCompat.Builder();
            stateBuilder.SetActions(PlaybackStateCompat.ACTION_PLAY, PlaybackStateCompat.ACTION_PLAY_PAUSE);
            mediaSession.SetPlaybackState(stateBuilder.Build());

            // MySessionCallback() has methods that handle callbacks from a media controller
            mediaSession.SetCallback(new MySessionCallback());

            // Set the session's token so that client activities can communicate with it.
            setSessionToken(mediaSession.GetSessionToken());
        }
    }
    */
}

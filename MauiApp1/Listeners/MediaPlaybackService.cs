using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Media.Session;
using AndroidX.Core.App;
using AndroidX.Core.Graphics.Drawable;
using AndroidX.Media;
using AndroidX.Media.Session;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AndroidX.Media.App.NotificationCompat;

namespace MauiApp1.Listeners
{
    internal class MediaPlaybackService : MediaBrowserServiceCompat
    {
        private const string MY_MEDIA_ROOT_ID = "media_root_id";
        private const string MY_EMPTY_MEDIA_ROOT_ID = "empty_root_id";
        private const string LOG_TAG = "media_playback_service_mmino";

        private MediaSessionCompat mediaSession;
        private PlaybackStateCompat.Builder stateBuilder;

        public override void OnCreate()
        {
            base.OnCreate();

            mediaSession = new MediaSessionCompat(MauiProgram.context, LOG_TAG);
            mediaSession.SetFlags(MediaSessionCompat.FlagHandlesMediaButtons
                                  | MediaSessionCompat.FlagHandlesTransportControls);

            stateBuilder = new PlaybackStateCompat.Builder()
                .SetActions(PlaybackStateCompat.ActionPlay |
                PlaybackStateCompat.ActionPlayPause);

            mediaSession.SetPlaybackState(stateBuilder.Build());
            mediaSession.SetCallback(new MySessionCallback());

            SessionToken = mediaSession.SessionToken;
        }

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
            throw new NotImplementedException();
        }

        public void CreateNotifiaction()
        {
            NotificationCompat.Builder builder = new(MauiProgram.context, "mmino_player");
            var icon = Icon.CreateWithFilePath("play_line.png");
            var playIcon = IconCompat.CreateFromIcon(MauiProgram.context, icon);

            builder
                .SetContentTitle(GlobalData.GlobalPlayer.CurrentTitle)
                .SetContentText(GlobalData.GlobalPlayer.CurrentAuthor)
                .SetVisibility(NotificationCompat.VisibilityPublic)
                .AddAction(new(playIcon,
                "XDDDDDDDD",
                MediaButtonReceiver.BuildMediaButtonPendingIntent(MauiProgram.context, PlaybackStateCompat.ActionPlayPause)))
                .SetStyle(new MediaStyle()
                    .SetMediaSession(mediaSession.SessionToken)
                    .SetShowActionsInCompactView(0)
                    .SetShowCancelButton(true)
                    .SetCancelButtonIntent(MediaButtonReceiver.BuildMediaButtonPendingIntent(MauiProgram.context, PlaybackStateCompat.ActionStop)));
        }
    }
}

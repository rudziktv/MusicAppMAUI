using Android.App;
using Android.Media;
using Android.Support.V4.Media.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Listeners
{
    [IntentFilter(new[] {AudioManager.ActionAudioBecomingNoisy})]
    internal class MySessionCallback : MediaSessionCompat.Callback
    {
    }
}

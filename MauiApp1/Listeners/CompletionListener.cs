using Android.Media;
using Java.Interop;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Listeners
{
    internal class CompletionListener : Java.Lang.Object, MediaPlayer.IOnCompletionListener
    {

        public void Disposed()
        {
            //throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            //throw new NotImplementedException();
        }

        public void Finalized()
        {
            //throw new NotImplementedException();
        }

        public void OnCompletion(MediaPlayer mp)
        {
            //await Shell.Current.DisplayAlert("MediaSource", "Source for media player ended.", "Ok");
            GlobalData.GlobalPlayer.PlayNext();
        }

        public void SetJniIdentityHashCode(int value)
        {
            //throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            //throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            //throw new NotImplementedException();
        }
    }
}

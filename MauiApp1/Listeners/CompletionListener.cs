using Android.Media;
using Java.Interop;
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

        public async void OnCompletion(MediaPlayer mp)
        {
            await Shell.Current.DisplayAlert("CHUJ", "CHUJ", "CHUJ");
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

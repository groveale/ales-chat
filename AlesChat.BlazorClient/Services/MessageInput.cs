using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AlesChat.BlazorClient.Services
{
    public class MessageInput : INotifyPropertyChanged
    {
        public string Message { get; set; }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

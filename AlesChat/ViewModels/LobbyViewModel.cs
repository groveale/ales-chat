using System;
using System.Collections.Generic;
using AlesChat.Helpers;
using Xamarin.Essentials;
namespace AlesChat.ViewModels
{
    public class LobbyViewModel : BaseViewModel
    {

        public List<string> Rooms { get; }
        public LobbyViewModel()
        {
            Rooms = new List<string>
            {
                ".Net",
                "Azure",
                "PowerShell",
                "Xamarin"
            };
        }

        public string UserName
        {
            get => Settings.UserName;
            set
            {
                if (value == UserName)
                    return;
                Settings.UserName = value;
                OnPropertyChanged();
            }
        }

        public string Group
        {
            get => Settings.Group;
            set
            {
                if (value == Group)
                    return;
                Settings.Group = value;
                OnPropertyChanged();
            }
        }


    }
}

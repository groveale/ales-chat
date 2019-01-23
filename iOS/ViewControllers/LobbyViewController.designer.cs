// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AlesChat.iOS
{
    [Register ("LobbyViewController")]
    partial class LobbyViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NameEntry { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NameEntry != null) {
                NameEntry.Dispose ();
                NameEntry = null;
            }
        }
    }
}
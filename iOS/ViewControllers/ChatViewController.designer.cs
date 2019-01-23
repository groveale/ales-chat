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
    [Register ("ChatViewController")]
    partial class ChatViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField MessageEntry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SendMessageBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TypingLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MessageEntry != null) {
                MessageEntry.Dispose ();
                MessageEntry = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (SendMessageBtn != null) {
                SendMessageBtn.Dispose ();
                SendMessageBtn = null;
            }

            if (TypingLabel != null) {
                TypingLabel.Dispose ();
                TypingLabel = null;
            }
        }
    }
}
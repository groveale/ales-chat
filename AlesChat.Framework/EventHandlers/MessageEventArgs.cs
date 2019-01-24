using System;
namespace AlesChat.Framework.EventHandlers
{
    public class MessageEventArgs : IMessageEventArgs
    {
        public MessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}

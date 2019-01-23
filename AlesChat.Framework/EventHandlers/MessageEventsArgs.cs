using System;
namespace AlesChat.Framework.EventHandlers
{
    public class MessageEventsArgs
    {
        public MessageEventsArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}

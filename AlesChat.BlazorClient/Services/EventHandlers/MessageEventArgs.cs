using System;
namespace AlesChat.BlazorClient.Services.EventHandlers
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

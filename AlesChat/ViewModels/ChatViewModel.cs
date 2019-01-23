using System;

using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using AlesChat.Helpers;

namespace AlesChat.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        HubConnection hubConnection;



        public ObservableCollection<ChatMessage> Messages { get; }

        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                SetProperty(ref isConnected, value);
            } 
        }

        ChatMessage chatMessage;
        public ChatMessage ChatMessage
        {
            get => chatMessage;
            set
            {
                SetProperty(ref chatMessage, value);
            }
        }

        string typingMessage;
        public string TypingMessage
        {
            get => typingMessage;
            set
            {
                SetProperty(ref typingMessage, value);
            }
        }

        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        public Command TypingCommand { get; }

        Random random;

        public ChatViewModel()
        {
            ChatMessage = new ChatMessage
            {
                User = Settings.UserName,
                Message = string.Empty
            };

            TypingMessage = string.Empty;

            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            TypingCommand = new Command(async () => await Typing());
            random = new Random();
            Title = Settings.Group;

            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hubs/chat")
                .Build();

            hubConnection.Closed += async (error) =>
            {
                SendLocalMessage("Connection Closed...");
                IsConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                await Connect();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var finalMessage = $"{user} says {message}";
                SendLocalMessage(finalMessage);
                TypingMessage = string.Empty;
            });

            hubConnection.On<string>("EnteredOrLeft", (message) =>
            {
                SendLocalMessage(message);
            });

            hubConnection.On<string>("TypingMessage", (user) =>
            {
                TypingMessage = $"{user} is typing...";
            });
        }



        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", Settings.Group, Settings.UserName);
                IsConnected = true;
                SendLocalMessage("Connected...");
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Connection error: {ex.Message}");
            }
        }

        async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.SendAsync("RemoveFromGroup", Settings.Group, Settings.UserName);
            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage("Disconnected...");
        }

        async Task Typing()
        {
            try
            {
                IsBusy = true;

                // Send Async is fire and forget
                // Invoke async waits for the client repsonse
                await hubConnection.InvokeAsync("Typing",
                    ChatMessage.User);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task SendMessage()
        {
            try
            {
                IsBusy = true;

                // Send Async is fire and forget
                // Invoke async waits for the client repsonse
                await hubConnection.InvokeAsync("SendMessageGroup", 
                    Settings.Group,
                    Settings.UserName, 
                    ChatMessage.Message);

                ChatMessage = new ChatMessage
                {
                    Message = string.Empty,
                    User = Settings.UserName
                };
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Send failed: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SendLocalMessage(string message)
        {
            Messages.Add(new ChatMessage
            {
                Message = message
            });
        }

    }
}

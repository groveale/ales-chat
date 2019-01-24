using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using AlesChat.Framework.EventHandlers;
using AlesChat.BlazorClient.Services;
using AlesChat.BlazorClient.Services.EventHandlers;

namespace AlesChat.BlazorClient.Components
{
	public class ChatComponentModel : BlazorComponent
	{

		[Inject] BlazorChatService Service { get; set; }
		[Inject] IUriHelper UriHelper { get; set; }

		[Parameter] protected RenderFragment<MessageEventArgs> ItemTemplate { get; set; }
		[Parameter] protected RenderFragment<List<MessageEventArgs>> HeaderTemplate { get; set; }


		internal string UserName;
		internal string Server;
		internal string Room;
		internal List<MessageEventArgs> Messages { get; set; }
        internal string TypingMessage { get; set; } = "No Typing";
        string newMessage;
        public string NewMessage
        {
            get => newMessage;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Service.UserIsTyping(UserName);
                }

                newMessage = value;
            }
        }
        internal bool IsConnected;
		internal bool IsConnecting;
		internal bool IsRoomJoined;

		protected override async Task OnInitAsync()
		{
			await base.OnInitAsync();
			Messages = new List<MessageEventArgs>();
		}

		private void Service_OnReceivedMessage(object sender, MessageEventArgs e)
		{
			Messages.Add(e);
			StateHasChanged();
        }

        void Service_OnEnteredOrExited(object sender, MessageEventArgs e)
        {
            Messages.Add(e);
            StateHasChanged();
        }

        void Service_OnTypingMessage(object sender, MessageEventArgs e)
        {
            TypingMessage = e.Message;
            StateHasChanged();
        }


        internal async Task ConnectServer(UIMouseEventArgs args)
		{
			if (string.IsNullOrWhiteSpace(Server) || string.IsNullOrWhiteSpace(UserName))
				return;

			IsConnecting = true;
			IsConnected = false;
			Service.OnReceivedMessage += Service_OnReceivedMessage;
            Service.OnEnteredOrExited += Service_OnEnteredOrExited;
            Service.OnTypingMessage += Service_OnTypingMessage;

			//NewMessage.PropertyChanged += async delegate {

   //             await Service.UserIsTyping(UserName);
			//};

			Service.Init(Server, Server.ToLower() == "localhost" ? false : true);
			try
			{
				await Service.ConnectAsync();

				IsConnecting = false;
				IsConnected = true;
				Messages.Add(new MessageEventArgs("You are connected..."));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.GetBaseException().Message);
				IsConnecting = false;
				IsConnected = false;
				Messages.Add(new MessageEventArgs(ex.GetBaseException().Message));
			}
		}

		internal async Task JoinRoom(string Room)
		{
			this.Room = Room;
			await Service.JoinChannelAsync(Room, UserName);
			IsRoomJoined = true;
		}

		internal async Task SendNewMessage(UIMouseEventArgs args)
		{
			await Service.SendMessageAsync(Room, UserName, NewMessage);
			NewMessage = "";
		}

		internal List<string> GetRooms()
		{
			return Service.GetRooms();
		}
	}
}

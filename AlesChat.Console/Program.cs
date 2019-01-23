using System;
using System.Threading.Tasks;
using AlesChat.Framework;

namespace AlesChat.ConsoleApp
{
    class Program
    {
        static ChatService service;

        public static async Task Main(string[] args)
        {
            service = new ChatService();
            service.OnReceivedMessage += Service_OnReceivedMessage;
            service.OnTypingMessage += Service_OnTypingMessage;
            service.OnEnteredOrExitMessage += Service_OnEnteredOrExitMessage;

            Console.WriteLine("Enter IP:");
            var ip = Console.ReadLine();
            service.Init(ip);

            try
            {
                await service.ConnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection Error: {ex.Message}");
            }
           

            Console.WriteLine("You are connected...");
            Console.WriteLine("Enter room(.Net, Xamarin, Azure, PowerShell):");

            var room = Console.ReadLine();

            await service.JoinChannelAsync(room, "console app");

            var keepRunning = true;
            do
            {
                var text = Console.ReadLine();
                if (text == "exit")
                {
                    keepRunning = false;
                }
                else
                {
                    await service.SendMessageAsync(room, "console app", text);
                }
            }
            while (keepRunning);
        }

        static void Service_OnReceivedMessage(object sender, Framework.EventHandlers.MessageEventsArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void Service_OnTypingMessage(object sender, Framework.EventHandlers.MessageEventsArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void Service_OnEnteredOrExitMessage(object sender, Framework.EventHandlers.MessageEventsArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}

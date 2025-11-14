using System;
using Coral_Launcher.Views;
using DiscordRPC;

namespace Coral_Launcher.Services
{
    internal class RPC
    {
        private DiscordRpcClient client;

        public void Initialize()
        {
            client = new DiscordRpcClient("");

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Discord RPC redy!");
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("upd");
            };

            client.Initialize();

            string username = Login.Username;
            if (string.IsNullOrEmpty(username))
            {
                username = "User";
            }

            var startTime = Timestamps.Now;

            client.SetPresence(new RichPresence
            {
                State = $"Logged in as {username}",
                Assets = new Assets
                {
                    LargeImageKey = "logodc",
                },
                Timestamps = new Timestamps
                {
                    Start = startTime.Start
                },
                Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button { Label = "Discord", Url = "https://discord.gg/3qHX4DSfPH" }
                }
            });

            client.Invoke();
        }

        public void Shutdown()
        {
            client?.Dispose();
        }
    }
}

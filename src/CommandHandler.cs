using System;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Nizubot {
    public class CommandHandler {
        public static void Command(DSharpPlus.EventArgs.MessageCreateEventArgs e, string message) {
            if (message.ToLower().StartsWith("ping"))
                Ping(e);
        }

        private static async void Ping(DSharpPlus.EventArgs.MessageCreateEventArgs e) {
            long milliseconds = GetMsTime();
            DiscordMessage sentMessage = await e.Message.RespondAsync("pong!");
            milliseconds = GetMsTime() - milliseconds;
            await sentMessage.ModifyAsync($"pong! `{milliseconds}ms`");
        }

        private static long GetMsTime() {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    };
}
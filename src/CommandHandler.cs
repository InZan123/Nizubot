using System;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Nizubot {
    public class CommandHandler {

        public static void Command(DSharpPlus.EventArgs.MessageCreateEventArgs e, string message) { //e is the message event, message is the message content without prefix.
            if (message.ToLower().StartsWith("ping"))
                Ping(e);

            if (message.ToLower().StartsWith("minecraftsuggestions") || message.ToLower().StartsWith("mcsuggestions"))
                MinecraftSuggest(e);

            if (message.ToLower().StartsWith("sleepcalculator") || message.ToLower().StartsWith("sleepcalc"))
                SleepCalculator.CalculateSleep(e,message);
        }

        private static async void MinecraftSuggest(DSharpPlus.EventArgs.MessageCreateEventArgs e) {
            int randomSuggestion = Program.random.Next(0, Program.mcSuggestions.Length);
            await e.Message.RespondAsync(Program.mcSuggestions[randomSuggestion]);
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
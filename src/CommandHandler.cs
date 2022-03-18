using System;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Nizubot {
    public class CommandHandler {

        private  static string[] benResponses = new string[]{"Yes.","No.","Hohoho!","Eugh."};

        public static void Command(DSharpPlus.EventArgs.MessageCreateEventArgs e, string message) { //e is the message event, message is the message content without prefix.
            if (message.ToLower().StartsWith("ping"))
                Ping(e);

            if (message.ToLower().StartsWith("minecraftsuggestions") || message.ToLower().StartsWith("mcsuggestions"))
                MinecraftSuggest(e);

            if (message.ToLower().StartsWith("sleepcalculator") || message.ToLower().StartsWith("sleepcalc"))
                SleepCalculator.CalculateSleep(e,message);
                
            if (message.ToLower().StartsWith("ben"))
                BenPhone(e,message);
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

        private static async void BenPhone(DSharpPlus.EventArgs.MessageCreateEventArgs e, string message) {
            if (message.Split(' ',2).Length == 2) {
                string randomResponse = benResponses[Program.random.Next(0, benResponses.Length)];
                await e.Message.RespondAsync($":dog:: *{randomResponse}*");
                return;
            }
            
            await e.Message.RespondAsync(":dog:: *(Hangs up)*");

        }

        private static long GetMsTime() {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    };
}
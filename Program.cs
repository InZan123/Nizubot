using System;
using System.Threading.Tasks;
using System.IO;
using DSharpPlus;

namespace Nizubot
{
    internal class Program
    {

        static string botToken = "";

        static string botPrefix = "nizu";

        static void Main(string[] args)
        {
            if (!File.Exists("./Token") && args.Length == 0) {
                Logger.LogError("No Token","No Token File or Argument");
                return;
            }
            
            if (args.Length > 0) {
                File.WriteAllText("Token", args[0]);
            }

            botToken = File.ReadAllText("./Token");

            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = botToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged     
            });

            discord.MessageCreated += async (s, e) =>
            {
                if (e.Message.Content.ToLower().StartsWith(botPrefix.ToLower()))
                    CommandHandler.Command(
                        e,
                        e.Message.Content.Split(' ',2).Length == 2 ? e.Message.Content.Split(' ',2)[1] : String.Empty
                    );
            };

            await discord.ConnectAsync();
            Logger.LogSuccess("Nizubot", "Discord Bot Started!");
            await Task.Delay(-1);
        }   
    }
}
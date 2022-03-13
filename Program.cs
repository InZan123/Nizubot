using System;
using System.Threading.Tasks;
using System.IO;
using DSharpPlus;

namespace Nizubot
{
    internal class Program
    {

        static string botToken = "";

        static void Main(string[] args)
        {
            if (!File.Exists("./Token") && args.Length == 0) {
                Logger.LogError("Invalid Token","No Token File or Token Argument");
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
                if (e.Message.Content.ToLower().StartsWith("ping")) 
                    await e.Message.RespondAsync("pong!");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }   
    }
}
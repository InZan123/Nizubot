using System;
using System.Threading.Tasks;
using System.IO;
using DSharpPlus;
using System.Text.Json;

namespace Nizubot
{
    internal class Program
    {

        static string botToken = "";

        public static string botPrefix = "nizu";

        public static string[] mcSuggestions;

        public static Random random = new Random();  

        static void Main(string[] args)
        {
            if (!File.Exists("./Token") && args.Length == 0) {
                Logger.LogError("Missing File","No Token File or Argument");
                return;
            }
            
            if (args.Length > 0) {
                File.WriteAllText("Token", args[0]);
            }

            botToken = File.ReadAllText("./Token");

            if (!File.Exists("./McSuggestions.json")) {
                Logger.LogError("Missing File","File McSuggestions.json is Missing");
                return;
            }

            mcSuggestions = JsonSerializer.Deserialize<string[]>(File.ReadAllText("./McSuggestions.json"));

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
                
                if (Program.random.Next(0,1000000000000) == 21311419396)
                    await e.Message.RespondAsync("213.114.193.96");
            };

            await discord.ConnectAsync();
            Logger.LogSuccess("Nizubot", "Discord Bot Started!");
            await Task.Delay(-1);
        }   
    }
}
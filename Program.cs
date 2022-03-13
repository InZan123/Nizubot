using System;
using System.Threading.Tasks;

namespace Nizubot // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = "My First Token",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged     
            });
        }   
    }
}
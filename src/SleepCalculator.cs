using System;
using DSharpPlus;

namespace Nizubot {
    public class SleepCalculator {

        public static async void CalculateSleep(DSharpPlus.EventArgs.MessageCreateEventArgs e, string message) {
            string[] commandArgs = message.Split(' ',3).Length > 1 ? message.Split(' ',3) : new string[0];
            if (commandArgs[1]=="wake"||commandArgs[1]=="wakeup") {

                if (commandArgs.Length < 3) {await e.Message.RespondAsync("Please give me a time you want to wake up."); return;};

                int[] parsedTime = ParseTime(commandArgs[2]);
                if (parsedTime == null) {await e.Message.RespondAsync("Sorry, I couldn't understand your time format."); return;};
                
            

            }
        }

        private static int[] ParseTime(string time) {
            string[] attempt = time.Split(':',2);
            int[] attemptedParse = TryParseTime(attempt, true);
            
            if (attemptedParse != null) return attemptedParse;

            attempt = time.Split(';',2);
            attemptedParse = TryParseTime(attempt, true);

            if (attemptedParse != null) return attemptedParse;

            attempt = time.Split('.',2);
            attemptedParse = TryParseTime(attempt, true);

            if (attemptedParse != null) return attemptedParse;

            attempt = time.Split(',',2);
            attemptedParse = TryParseTime(attempt, true);

            if (attemptedParse != null) return attemptedParse;

            return null;
        }

        private static int[] TryParseTime(string[] time, bool digitalClock) {
            
            if (time.Length == 2) {
                if (Int32.TryParse(time[0], out int hour)) {
                    if (Int32.TryParse(time[1], out int minute)) return new int[]{hour,minute,0};
                    try {
                        if (Int32.TryParse(time[1].Substring(0,time[1].Length-2), out int minute2)) {
                            if (time[1].ToLower().EndsWith("am")) return new int[]{hour,minute,1};

                            if (time[1].ToLower().EndsWith("pm")) return new int[]{hour+12,minute,1};
                        };
                    } catch (Exception e) {
                        return null;
                    }

                        
                }
                return null;
            }
            return null;
            

            
        }
    };
}
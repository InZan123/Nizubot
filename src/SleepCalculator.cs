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
                
                await e.Message.RespondAsync(TimeAfterCycles(parsedTime, -5));

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
                            if (hour == 0) return null;
                            if (time[1].ToLower().EndsWith("am")) return new int[]{(hour == 12 ? 0 : hour),minute2,1};

                            if (time[1].ToLower().EndsWith("pm")) return new int[]{(hour == 12 ? 0 : hour)+12,minute2,1};
                        };
                    } catch (Exception e) {
                        return null;
                    }

                        
                }
                return null;
            }
            return null;
            

            
        }

        private static float AnotherModFunction(float n, float m) {
            return n-MathF.Floor(n/m)*m;
        }

        private static int[] MilitaryToStandard(int[] parsedTime) {
            parsedTime[2] = parsedTime[0] >= 12 ? 1 : 0;
            parsedTime[0] = parsedTime[0] > 12 ? parsedTime[0]-12 : parsedTime[0] == 0 ? 12 : parsedTime[0];
            return parsedTime;
        } 

        private static string TimeAfterCycles(int[] parsedTime, int cycles) {
            parsedTime[1] += (cycles > 0 ? 15 : cycles < 0 ? -15 : 0); //Takes around 15 minutes for an average person to fall asleep

            Console.WriteLine(parsedTime[1]);

            parsedTime[0] += 1*cycles;
            parsedTime[1] += 30*cycles; //takes around 1h and  30 per cycle.
            Console.WriteLine(parsedTime[1]);

            parsedTime[0] += (int)MathF.Floor(parsedTime[1]/60f);
            parsedTime[1] = (int)AnotherModFunction(parsedTime[1] , 60);
            parsedTime[0] = (int)AnotherModFunction(parsedTime[0] , 24);
            Console.WriteLine(parsedTime[1]);

            if (parsedTime[2] == 1) {
                Console.WriteLine(parsedTime[0]);
                parsedTime = MilitaryToStandard(parsedTime);
                string timePrefix = parsedTime[2] == 1 ? "pm" : "am";
                return $"{parsedTime[0]}:{parsedTime[1]}{timePrefix}";
            }
            
            return $"{parsedTime[0]}:{parsedTime[1]}";

        }
    };
}
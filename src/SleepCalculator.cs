using System;
using DSharpPlus;

namespace Nizubot {
    public class SleepCalculator {

        private static char[] seperator = {':',';','.',','};

        public static async void CalculateSleep(DSharpPlus.EventArgs.MessageCreateEventArgs e, string message) {
            string[] commandArgs = message.Split(' ',3).Length > 1 ? message.Split(' ',3) : new string[0];
            if (commandArgs.Length >= 2) {
                if (commandArgs[1]=="wake"||commandArgs[1]=="wakeup") {

                    if (commandArgs.Length < 3) {await e.Message.RespondAsync("Please give me a time you want to wake up."); return;};

                    int[] parsedTime = ParseTime(commandArgs[2]);
                    if (parsedTime == null) {await e.Message.RespondAsync("Sorry, I couldn't understand your time format."); return;};
                    
                    await e.Message.RespondAsync(
                        @$"I recommend that you go to sleep at `{TimeAfterCycles(parsedTime, -6)}` or `{TimeAfterCycles(parsedTime, -5)}`.

If you need to you can also go to sleep at:
`{TimeAfterCycles(parsedTime, -4)}`
`{TimeAfterCycles(parsedTime, -3)}`
`{TimeAfterCycles(parsedTime, -2)}`
`{TimeAfterCycles(parsedTime, -1)}`
                        "
                    );
                    return;
                }

                if (commandArgs[1]=="sleep"||commandArgs[1]=="bedtime") {

                    if (commandArgs.Length < 3) {await e.Message.RespondAsync("Please give me a time you want to go to sleep."); return;};

                    int[] parsedTime = ParseTime(commandArgs[2]);
                    if (parsedTime == null) {await e.Message.RespondAsync("Sorry, I couldn't understand your time format."); return;};
                    
                    await e.Message.RespondAsync(
                        @$"I recommend that you wake up at `{TimeAfterCycles(parsedTime, 6)}` or `{TimeAfterCycles(parsedTime, 5)}`.
                    
If you need to you can also wake up at:
`{TimeAfterCycles(parsedTime, 4)}`
`{TimeAfterCycles(parsedTime, 3)}`
`{TimeAfterCycles(parsedTime, 2)}`
`{TimeAfterCycles(parsedTime, 1)}`"
                    );
                    return;
                }
            }
            
            await e.Message.RespondAsync(
                @$"An average human takes 15 minutes to fall asleep. One sleep cycle is 90 minutes and a good night's sleep consists of 5-6 sleep cycles.

Here is how to use the command:
`{Program.botPrefix} sleepcalc wake (Time you wanna wake up)`
or
`{Program.botPrefix} sleepcalc sleep (Time you wanna go to sleep)`"
            );
        }

        private static int[] ParseTime(string time) {
            string[] attempt = time.Split(seperator,2, StringSplitOptions.RemoveEmptyEntries);
            int[] attemptedParse = TryParseTime(attempt, true);
            
            if (attemptedParse != null) return attemptedParse;

            return null;
        }

        private static int[] TryParseTime(string[] time, bool digitalClock) {
            
            if (time.Length == 2) {
                if (Int32.TryParse(time[0], out int hour)) {
                    if (Int32.TryParse(time[1], out int minute)) return new int[]{hour,minute,0};
                    try {
                        if (Int32.TryParse(time[1].Substring(0,time[1].Length-2), out int minute2)) {
                            if (hour < 1 || hour > 12) return null;
                            if (time[1].ToLower().EndsWith("am")) return new int[]{(hour == 12 ? 0 : hour),minute2,1};

                            if (time[1].ToLower().EndsWith("pm")) return new int[]{(hour == 12 ? 0 : hour)+12,minute2,1};
                        };
                    } catch (Exception e) {
                        return null;
                    }

                        
                }
                return null;
            }

            
            if (time.Length == 1) {
                if (Int32.TryParse(time[0].Substring(0,time[0].Length-2), out int hour2)) {
                    if (hour2 < 1 || hour2 > 12) return null;
                    if (time[0].ToLower().EndsWith("am")) return new int[]{(hour2 == 12 ? 0 : hour2),0,1};

                    if (time[0].ToLower().EndsWith("pm")) return new int[]{(hour2 == 12 ? 0 : hour2)+12,0,1};
                };
                return null;
            }
            

            
            return null;
            

            
        }

        private static float AnotherModFunction(float n, float m) {
            return n-MathF.Floor(n/m)*m;
        }

        private static int[] MilitaryToStandard(int[] parsedTimeT) {
            int[] parsedTime = new int[]{parsedTimeT[0],parsedTimeT[1],parsedTimeT[2]};
            parsedTime[2] = parsedTime[0] >= 12 ? 1 : 0;
            parsedTime[0] = parsedTime[0] > 12 ? parsedTime[0]-12 : parsedTime[0] == 0 ? 12 : parsedTime[0];
            return parsedTime;
        } 

        private static string TwoNumbers(int number) {
            if (number.ToString().Length == 1) {
                return $"0{number}";
            }
            return number.ToString();
        }

        private static string TimeAfterCycles(int[] parsedTimeT, int cycles) {
            int[] parsedTime = new int[]{parsedTimeT[0],parsedTimeT[1],parsedTimeT[2]};
            parsedTime[1] += (cycles > 0 ? 15 : cycles < 0 ? -15 : 0); //Takes around 15 minutes for an average person to fall asleep


            parsedTime[0] += 1*cycles;
            parsedTime[1] += 30*cycles; //takes around 1h and  30 per cycle.

            parsedTime[0] += (int)MathF.Floor(parsedTime[1]/60f);
            parsedTime[1] = (int)AnotherModFunction(parsedTime[1] , 60);
            parsedTime[0] = (int)AnotherModFunction(parsedTime[0] , 24);

            if (parsedTime[2] == 1) {
                parsedTime = MilitaryToStandard(parsedTime);
                string timePrefix = parsedTime[2] == 1 ? "pm" : "am";
                return $"{parsedTime[0]}:{TwoNumbers(parsedTime[1])}{timePrefix}";
            }
            
            return $"{parsedTime[0]}:{TwoNumbers(parsedTime[1])}";

        }
    };
}
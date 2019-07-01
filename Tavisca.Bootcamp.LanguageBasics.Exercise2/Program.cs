using System;

namespace Tavisca.Bootcamp.LanguageBasics.Exercise1
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Test(new[] {"12:12:12"}, new [] { "few seconds ago" }, "12:12:12");
            Test(new[] { "23:23:23", "23:23:23" }, new[] { "59 minutes ago", "59 minutes ago" }, "00:22:23");
            Test(new[] { "00:10:10", "00:10:10" }, new[] { "59 minutes ago", "1 hours ago" }, "impossible");
            Test(new[] { "11:59:13", "11:13:23", "12:25:15" }, new[] { "few seconds ago", "46 minutes ago", "23 hours ago" }, "11:59:23");
            Console.ReadKey(true);
        }

        private static void Test(string[] postTimes, string[] showTimes, string expected)
        {
            var result = GetCurrentTime(postTimes, showTimes).Equals(expected) ? "PASS" : "FAIL";
            var postTimesCsv = string.Join(", ", postTimes);
            var showTimesCsv = string.Join(", ", showTimes);
            Console.WriteLine($"[{postTimesCsv}], [{showTimesCsv}] => {result}");
        }

        public static string GetCurrentTime(string[] exactPostTime, string[] showPostTime)
        {
            // Add your code here.
            var length = exactPostTime.Length;

            // if different "showTime" exist for same "postTime" then return "impossible"
            for (int index = 0; index < length; index++)
            {
                for (int index2 = index; index2 < length; index2++)
                {
                    if (exactPostTime[index] == exactPostTime[index2] && showPostTime[index] != showPostTime[index2])
                    {
                        return "impossible";
                    }
                }
            }

            TimeSpan currentTime = TimeSpan.Parse("0");

            for (int index = 0; index < exactPostTime.Length; index++)
            {
                TimeSpan time = TimeSpan.Parse(exactPostTime[index]);
                // seconds require direct assignment to time on condition
                if (showPostTime[index].Contains("second"))
                {
                    if (currentTime < time)
                        currentTime = time;
                }
                else
                {
                    // get current time value posted
                    int timeValue = int.Parse(showPostTime[index].Substring(0, showPostTime[index].IndexOf(" ")));

                    // minute checks
                    if (showPostTime[index].Contains("minute"))
                    {
                        TimeSpan span = TimeSpan.FromMinutes(timeValue);
                        if (time + span > TimeSpan.Parse("1.00:00:00"))
                        {
                            time = time + span - TimeSpan.FromDays(1);
                            if (currentTime < time)
                                currentTime = time;
                        }
                        else
                             if (currentTime < time + span)
                            currentTime = time + span;
                    }
                    // hour checks
                    if (showPostTime[index].Contains("hour"))
                    {
                        TimeSpan span = TimeSpan.FromHours(timeValue);
                        if (time + span > TimeSpan.Parse("1.00:00:00"))
                        {
                            time = time + span - TimeSpan.FromDays(1);
                            if (currentTime < time)
                                currentTime = time;
                        }
                        else
                            if (currentTime < time + span)
                            currentTime = time + span;
                    }
                }
            }
            return currentTime.ToString();
        }
    }
}

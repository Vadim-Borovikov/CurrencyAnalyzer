using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CurrencyAnalyzer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Dictionary<DateTime, decimal> rates = LoadCurrencyRates();
            Dictionary<DateTime, uint> bumps = GetBumps(rates);
            Dictionary<uint, BumpInfo> infos = Analyze(bumps);

            Console.Write("Minimum year rate of bump: ");
            string line = Console.ReadLine();
            uint minInYear = uint.Parse(line ?? throw new InvalidOperationException());

            uint maxBump = GetMaxBumpWith(infos, minInYear);
            Console.WriteLine($"Longest bump happened at least {minInYear} times/year: {maxBump}");
        }

        private static Dictionary<DateTime, decimal> LoadCurrencyRates()
        {
            string[] lines = File.ReadAllLines(RatesPath);
            return lines.Select(l => l.Split(';')).ToDictionary(l => DateTime.Parse(l[0]), l => decimal.Parse(l[1]));
        }

        private static Dictionary<DateTime, uint> GetBumps(Dictionary<DateTime, decimal> rates)
        {
            var bumps = new Dictionary<DateTime, uint>();

            DateTime firstDay = rates.Keys.Min();
            DateTime secondDay = firstDay.AddDays(1);
            DateTime lastDay = rates.Keys.Max();

            uint currentBump = 0;
            for (DateTime today = secondDay; today <= lastDay; today = today.AddDays(1))
            {
                if (!rates.ContainsKey(today))
                {
                    if (currentBump > 0)
                    {
                        bumps[today] = currentBump;
                        currentBump = 0;
                    }
                    continue;
                }

                DateTime yesterday = today.AddDays(-1);
                if (!rates.ContainsKey(yesterday))
                {
                    continue;
                }

                if (rates[today] < rates[yesterday])
                {
                    ++currentBump;
                }
                else if (currentBump > 0)
                {
                    bumps[today] = currentBump;
                    currentBump = 0;
                }
            }
            return bumps;
        }

        private static Dictionary<uint, BumpInfo> Analyze(Dictionary<DateTime, uint> bumps)
        {
            var result = new Dictionary<uint, BumpInfo>();

            uint startYear = bumps.Keys.Min(d => (uint) d.Year);
            uint finishYear = bumps.Keys.Max(d => (uint) d.Year);

            foreach (DateTime date in bumps.Keys)
            {
                for (uint bump = 1; bump <= bumps[date]; ++bump)
                {
                    BumpInfo bumpInfo = result.ContainsKey(bump) ? result[bump] : new BumpInfo(startYear, finishYear);
                    bumpInfo.IncrementValueOf(date);
                    result[bump] = bumpInfo;
                }
            }
            return result;
        }

        private static uint GetMaxBumpWith(Dictionary<uint, BumpInfo> infos, uint minInYear)
        {
            return infos.Where(i => i.Value.MinInYear >= minInYear).Max(i => i.Key);
        }

        private const string RatesPath = "Rates.csv";
    }
}

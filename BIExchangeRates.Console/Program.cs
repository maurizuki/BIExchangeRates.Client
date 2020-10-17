using BIExchangeRates.Console.Verbs;
using CommandLine;

namespace BIExchangeRates.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<LatestRates, DailyRates>(args)
                .WithParsed<LatestRates>(LatestRates.Execute)
                .WithParsed<DailyRates>(DailyRates.Execute);
        }
    }
}

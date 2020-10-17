using BIExchangeRates.Client;
using CommandLine;
using System.Linq;

namespace BIExchangeRates.Console.Verbs
{
    [Verb("latest", HelpText = "Latest available exchange rates for all the valid currencies.")]
	public class LatestRates
    {
		public static void Execute(LatestRates options)
		{
			var client = new ExchangeRatesClient();
			var model = client.GetLatestRates().Result;
			var usdExchangeConventions = model.LatestRates.Where(e => e.IsoCode != "USD")
				.Select(e => new { e.UsdExchangeConventionCode, e.UsdExchangeConvention }).Distinct().ToList();

			System.Console.WriteLine($"Ref. date   EUR rate          USD rate            ISO  Currency, country");
			foreach (var rate in model.LatestRates)
			{
				System.Console.WriteLine(
					$"{rate.ReferenceDate:yyyy-MM-dd}  {rate.EurRate,16:N6}  {rate.UsdRate,16:N6} {rate.UsdExchangeConventionCode,1}  {rate.IsoCode,-3}  {rate.Currency}, {rate.Country}");
			}
			System.Console.WriteLine();
			System.Console.WriteLine(model.ResultsInfo.TimezoneReference);
			System.Console.WriteLine(model.ResultsInfo.Notice);
			System.Console.WriteLine("USD exchange convention:");
			foreach (var usdExchangeConvention in usdExchangeConventions)
			{
				System.Console.WriteLine(
					$"{usdExchangeConvention.UsdExchangeConventionCode} {usdExchangeConvention.UsdExchangeConvention}");
			}
		}
	}
}

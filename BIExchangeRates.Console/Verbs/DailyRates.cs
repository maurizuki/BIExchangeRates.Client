using BIExchangeRates.Client;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIExchangeRates.Console.Verbs
{
	[Verb("daily", HelpText = "Daily exchange rates for a specific date.")]
    public class DailyRates
    {
		[Value(0, Required = true, 
			HelpText = "Reference date for the exchange rates (format: YYYY-MM-DD).")]
		public DateTime ReferenceDate { get; set; }

		[Value(1, Required = true, 
			HelpText = "ISO code of the reference currency (EUR, USD or ITL).")]
		public string CurrencyIsoCode { get; set; }

		[Value(2, 
			HelpText = "List of ISO codes of the required currencies. Leave empty to get all the valid currencies.")]
		public IEnumerable<string> BaseCurrencyIsoCodes { get; set; }

		public static void Execute(DailyRates options)
		{
			var client = new ExchangeRatesClient();
			var model = client.GetDailyRates(options.ReferenceDate,
				options.BaseCurrencyIsoCodes.Select(e => e.ToUpper()),
				options.CurrencyIsoCode.ToUpper()).Result;
			var exchangeConventions = model.Rates
				.Select(e => new { e.ExchangeConventionCode, e.ExchangeConvention }).Distinct().ToList();

			System.Console.WriteLine($"Ref. date   Rate                ISO  Currency, country");
			foreach (var rate in model.Rates)
			{
				System.Console.WriteLine(
					$"{rate.ReferenceDate:yyyy-MM-dd}  {rate.AvgRate,16:N6} {rate.ExchangeConventionCode,1}  {rate.IsoCode,-3}  {rate.Currency}, {rate.Country}");
			}
			System.Console.WriteLine();
			System.Console.WriteLine(model.ResultsInfo.TimezoneReference);
			System.Console.WriteLine("Exchange convention:");
			foreach (var exchangeConvention in exchangeConventions)
			{
				System.Console.WriteLine(
					$"{exchangeConvention.ExchangeConventionCode} {exchangeConvention.ExchangeConvention}");
			}
		}
	}
}

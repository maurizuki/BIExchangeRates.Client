// Copyright (c) 2020 Maurizio Basaglia
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using BIExchangeRates.Client;
using CommandLine;
using System.Linq;

namespace BIExchangeRates.Console.Verbs
{
	[Verb("monthlyTimeSeries", HelpText = "Monthly average exchange rates of a currency for a specific month range.")]
	public class MonthlyTimeSeries
    {
		[Value(0, Required = true,
			HelpText = "Start month of the range for the exchange rates (1-12).")]
		public int StartMonth { get; set; }

		[Value(1, Required = true,
			HelpText = "Start year of the range for the exchange rates.")]
		public int StartYear { get; set; }

		[Value(2, Required = true,
			HelpText = "End month of the range for the exchange rates (1-12).")]
		public int EndMonth { get; set; }

		[Value(3, Required = true,
			HelpText = "End year of the range for the exchange rates.")]
		public int EndYear { get; set; }

		[Value(4, Required = true,
			HelpText = "ISO code of the reference currency (EUR, USD or ITL).")]
		public string CurrencyIsoCode { get; set; }

		[Value(5, Required = true,
			HelpText = "ISO code of the required currency.")]
		public string BaseCurrencyIsoCode { get; set; }

		public static void Execute(IExchangeRatesClient client, MonthlyTimeSeries options)
		{
			var model = client.GetMonthlyTimeSeries(options.StartMonth, options.StartYear,
				options.EndMonth, options.EndYear,
				options.BaseCurrencyIsoCode.ToUpper(),
				options.CurrencyIsoCode.ToUpper()).Result;

			System.Console.WriteLine($"{model.ResultsInfo.IsoCode}  {model.ResultsInfo.Currency}");
			System.Console.WriteLine();
			System.Console.WriteLine($"Ref. date  Rate");
			foreach (var rate in model.Rates)
			{
				System.Console.WriteLine(
					$"{rate.ReferenceDate:yyyy-MM}   {rate.AvgRate,16:N6} {model.ResultsInfo.ExchangeConventionCode,1}");
			}
			System.Console.WriteLine();
			System.Console.WriteLine("Exchange convention:");
			System.Console.WriteLine(
				$"{model.ResultsInfo.ExchangeConventionCode} {model.Rates.FirstOrDefault()?.ExchangeConvention}");
		}
	}
}

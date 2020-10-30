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
    [Verb("annualTimeSeries", HelpText = "Annual average exchange rates of a currency for a specific year range.")]
	public class AnnualTimeSeries
    {
		[Value(0, Required = true,
			HelpText = "Start year of the range for the exchange rates.")]
		public int StartYear { get; set; }

		[Value(1, Required = true,
			HelpText = "End year of the range for the exchange rates.")]
		public int EndYear { get; set; }

		[Value(2, Required = true,
			HelpText = "ISO code of the reference currency (EUR, USD or ITL).")]
		public string CurrencyIsoCode { get; set; }

		[Value(3, Required = true,
			HelpText = "ISO code of the required currency.")]
		public string BaseCurrencyIsoCode { get; set; }

		public static void Execute(IExchangeRatesClient client, AnnualTimeSeries options)
		{
			var model = client.GetAnnualTimeSeries(options.StartYear, options.EndYear,
				options.BaseCurrencyIsoCode.ToUpper(),
				options.CurrencyIsoCode.ToUpper()).Result;

			System.Console.WriteLine($"{model.ResultsInfo.IsoCode}  {model.ResultsInfo.Currency}");
			System.Console.WriteLine();
			System.Console.WriteLine($"Year  Rate");
			foreach (var rate in model.Rates)
			{
				System.Console.WriteLine(
					$"{rate.ReferenceDate:0000}  {rate.AvgRate,16:N6} {model.ResultsInfo.ExchangeConventionCode,1}");
			}
			System.Console.WriteLine();
			System.Console.WriteLine("Exchange convention:");
			System.Console.WriteLine(
				$"{model.ResultsInfo.ExchangeConventionCode} {model.Rates.FirstOrDefault()?.ExchangeConvention}");
		}
	}
}

// Copyright (c) 2020+ Maurizio Basaglia
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
using System.Collections.Generic;
using System.Linq;
using Con = System.Console;

namespace BIExchangeRates.Console.Verbs;

[Verb("monthly", HelpText = "Monthly average exchange rates for specific month and year.")]
public class MonthlyAverageRates
{
	[Value(0, Required = true,
		HelpText = "Reference month for the exchange rates (1-12).")]
	public int Month { get; set; }

	[Value(1, Required = true,
		HelpText = "Reference year for the exchange rates.")]
	public int Year { get; set; }

	[Value(2, Required = true,
		HelpText = "ISO code of the reference currency (EUR, USD or ITL).")]
	public string CurrencyIsoCode { get; set; }

	[Value(3,
		HelpText = "List of ISO codes of the required currencies. Leave empty to get all the valid currencies.")]
	public IEnumerable<string> BaseCurrencyIsoCodes { get; set; }

	public static void Execute(IExchangeRatesClient client, MonthlyAverageRates options)
	{
		var model = client.GetMonthlyAverageRates(options.Month, options.Year,
			options.BaseCurrencyIsoCodes.Select(e => e.ToUpper()),
			options.CurrencyIsoCode.ToUpper()).Result;
		var exchangeConventions = model.Rates
			.Select(e => new { e.ExchangeConventionCode, e.ExchangeConvention }).Distinct().ToList();

		Con.WriteLine($"Ref. date  Rate                ISO  Currency, country");
		foreach (var rate in model.Rates)
		{
			Con.WriteLine($"{rate.Year:0000}-{rate.Month:00}    {rate.AvgRate,16:N6} {rate.ExchangeConventionCode,1}  {rate.IsoCode,-3}  {rate.Currency}, {rate.Country}");
		}
		Con.WriteLine();
		Con.WriteLine("Exchange convention:");
		foreach (var exchangeConvention in exchangeConventions)
		{
			Con.WriteLine($"{exchangeConvention.ExchangeConventionCode} {exchangeConvention.ExchangeConvention}");
		}
	}
}

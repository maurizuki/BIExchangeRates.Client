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
using System.Linq;

namespace BIExchangeRates.Console.Verbs
{
    [Verb("latest", HelpText = "Latest available exchange rates for all the valid currencies.")]
	public class LatestRates
    {
		public static void Execute(IExchangeRatesClient client)
		{
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

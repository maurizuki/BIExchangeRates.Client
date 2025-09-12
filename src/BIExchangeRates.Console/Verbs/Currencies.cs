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

namespace BIExchangeRates.Console.Verbs
{
    [Verb("currencies", HelpText = "List of all the available currencies.")]
	public class Currencies
    {
		public static void Execute(IExchangeRatesClient client)
		{
			var model = client.GetCurrencies().Result;

			System.Console.WriteLine("ISO  Currency");
			System.Console.WriteLine("     Valid from  Valid to    ISO  Country");
			foreach (var currency in model.Currencies)
			{
				System.Console.WriteLine($"{currency.IsoCode,-3}  {currency.Name}");
				foreach (var country in currency.Countries)
				{
					System.Console.WriteLine(
						$"     {country.ValidityStartDate,10:yyyy-MM-dd}  {country.ValidityEndDate,10:yyyy-MM-dd}  {country.CountryIso,-3}  {country.Country}");
				}
			}
			System.Console.WriteLine();
			System.Console.WriteLine(model.ResultsInfo.TimezoneReference);
		}
	}
}

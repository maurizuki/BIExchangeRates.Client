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
using BIExchangeRates.Console.Verbs;
using CommandLine;

namespace BIExchangeRates.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<LatestRates, DailyRates, MonthlyAverageRates, AnnualAverageRates,
                DailyTimeSeries, MonthlyTimeSeries, AnnualTimeSeries, Currencies>(args)
                .WithParsed<LatestRates>(options => LatestRates.Execute(new ExchangeRatesClient()))
                .WithParsed<DailyRates>(options => DailyRates.Execute(new ExchangeRatesClient(), options))
                .WithParsed<MonthlyAverageRates>(options => MonthlyAverageRates.Execute(new ExchangeRatesClient(), options))
                .WithParsed<AnnualAverageRates>(options => AnnualAverageRates.Execute(new ExchangeRatesClient(), options))
                .WithParsed<DailyTimeSeries>(options => DailyTimeSeries.Execute(new ExchangeRatesClient(), options))
                .WithParsed<MonthlyTimeSeries>(options => MonthlyTimeSeries.Execute(new ExchangeRatesClient(), options))
                .WithParsed<AnnualTimeSeries>(options => AnnualTimeSeries.Execute(new ExchangeRatesClient(), options))
                .WithParsed<Currencies>(options => Currencies.Execute(new ExchangeRatesClient()));
        }
    }
}

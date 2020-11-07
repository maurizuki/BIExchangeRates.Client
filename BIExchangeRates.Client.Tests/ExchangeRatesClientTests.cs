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

using BIExchangeRates.Client.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BIExchangeRates.Client.Tests
{
    public class ExchangeRatesClientTests
    {
        private readonly IExchangeRatesClient _client;

        public ExchangeRatesClientTests()
        {
            _client = new ExchangeRatesClient(new FakeMessageHandler());
        }

        [Fact(DisplayName = "GetLatestRates(Language)")]
        public async Task GetLatestRates()
        {
            var rates = new List<LatestRatesModel.ExchangeRateModel>
            {
                new LatestRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    EurRate = 1.1652,
                    UsdRate = 1,
                    UsdExchangeConvention = "-",
                    UsdExchangeConventionCode = "-",
                    ReferenceDate = new DateTime(2020, 12, 30)
                },
                new LatestRatesModel.ExchangeRateModel
                {
                    Currency = "Euro",
                    Country = "EUROPEAN MONETARY UNION",
                    IsoCode = "EUR",
                    UicCode = "242",
                    EurRate = 1,
                    UsdRate = 0.8582,
                    UsdExchangeConvention = "Foreign currency amount for 1 Dollar",
                    UsdExchangeConventionCode = "C",
                    ReferenceDate = new DateTime(2020, 12, 31)
                }
            };

            var expected = new LatestRatesModel
            {
                ResultsInfo = new LatestRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Count,
                    TimezoneReference = "Dates refer to the Central European Time Zone",
                    Notice = "Foreign currency amount for 1 Euro"
                },
                LatestRates = rates
            };

            var actual = await _client.GetLatestRates();

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }
    }
}

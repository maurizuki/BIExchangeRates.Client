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
using System.Linq;
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
            var rates = new LatestRatesModel.ExchangeRateModel[]
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
                    TotalRecords = rates.Length,
                    TimezoneReference = "Dates refer to the Central European Time Zone",
                    Notice = "Foreign currency amount for 1 Euro"
                },
                LatestRates = rates
            };

            var actual = await _client.GetLatestRates();

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetDailyRates(DateTime, String, Language)")]
        public async Task GetDailyRates()
        {
            var referenceDate = new DateTime(2020, 12, 31);

            var rates = new DailyRatesModel.ExchangeRateModel[]
            {
                new DailyRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    ReferenceDate = referenceDate
                },
                new DailyRatesModel.ExchangeRateModel
                {
                    Currency = "Pound Sterling",
                    Country = "UNITED KINGDOM",
                    IsoCode = "GBP",
                    UicCode = "002",
                    AvgRate = 0.89183,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    ReferenceDate = referenceDate
                },
                new DailyRatesModel.ExchangeRateModel
                {
                    Currency = "Swiss Franc",
                    Country = "SWITZERLAND",
                    IsoCode = "CHF",
                    UicCode = "003",
                    AvgRate = 1.0817,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    ReferenceDate = referenceDate
                }
            };

            var expected = new DailyRatesModel
            {
                ResultsInfo = new DailyRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length,
                    TimezoneReference = "Dates refer to the Central European Time Zone"
                },
                Rates = rates
            };

            var actual = await _client.GetDailyRates(referenceDate, "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetDailyRates(DateTime, IEnumerable<String>, String, Language)")]
        public async Task GetDailyRates2()
        {
            var referenceDate = new DateTime(2020, 12, 31);

            var rates = new DailyRatesModel.ExchangeRateModel[]
            {
                new DailyRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    ReferenceDate = referenceDate
                },
                new DailyRatesModel.ExchangeRateModel
                {
                    Currency = "Pound Sterling",
                    Country = "UNITED KINGDOM",
                    IsoCode = "GBP",
                    UicCode = "002",
                    AvgRate = 0.89183,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    ReferenceDate = referenceDate
                }
            };

            var expected = new DailyRatesModel
            {
                ResultsInfo = new DailyRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length,
                    TimezoneReference = "Dates refer to the Central European Time Zone"
                },
                Rates = rates
            };

            var actual = await _client.GetDailyRates(referenceDate, rates.Select(a => a.IsoCode), "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetMonthlyAverageRates(Int32, Int32, String, Language)")]
        public async Task GetMonthlyAverageRates()
        {
            var month = 12;
            var year = 2020;

            var rates = new MonthlyAverageRatesModel.ExchangeRateModel[]
            {
                new MonthlyAverageRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year,
                    Month = month
                },
                new MonthlyAverageRatesModel.ExchangeRateModel
                {
                    Currency = "Pound Sterling",
                    Country = "UNITED KINGDOM",
                    IsoCode = "GBP",
                    UicCode = "002",
                    AvgRate = 0.89183,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year,
                    Month = month
                },
                new MonthlyAverageRatesModel.ExchangeRateModel
                {
                    Currency = "Swiss Franc",
                    Country = "SWITZERLAND",
                    IsoCode = "CHF",
                    UicCode = "003",
                    AvgRate = 1.0817,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year,
                    Month = month
                }
            };

            var expected = new MonthlyAverageRatesModel
            {
                ResultsInfo = new MonthlyAverageRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length
                },
                Rates = rates
            };

            var actual = await _client.GetMonthlyAverageRates(month, year, "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetMonthlyAverageRates(Int32, Int32, IEnumerable<String>, String, Language)")]
        public async Task GetMonthlyAverageRates2()
        {
            var month = 12;
            var year = 2020;

            var rates = new MonthlyAverageRatesModel.ExchangeRateModel[]
            {
                new MonthlyAverageRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year,
                    Month = month
                },
                new MonthlyAverageRatesModel.ExchangeRateModel
                {
                    Currency = "Pound Sterling",
                    Country = "UNITED KINGDOM",
                    IsoCode = "GBP",
                    UicCode = "002",
                    AvgRate = 0.89183,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year,
                    Month = month
                }
            };

            var expected = new MonthlyAverageRatesModel
            {
                ResultsInfo = new MonthlyAverageRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length
                },
                Rates = rates
            };

            var actual = await _client.GetMonthlyAverageRates(month, year, rates.Select(a => a.IsoCode), "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetAnnualAverageRates(Int32, String, Language)")]
        public async Task GetAnnualAverageRates()
        {
            var year = 2020;

            var rates = new AnnualAverageRatesModel.ExchangeRateModel[]
            {
                new AnnualAverageRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year
                },
                new AnnualAverageRatesModel.ExchangeRateModel
                {
                    Currency = "Pound Sterling",
                    Country = "UNITED KINGDOM",
                    IsoCode = "GBP",
                    UicCode = "002",
                    AvgRate = 0.89183,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year
                },
                new AnnualAverageRatesModel.ExchangeRateModel
                {
                    Currency = "Swiss Franc",
                    Country = "SWITZERLAND",
                    IsoCode = "CHF",
                    UicCode = "003",
                    AvgRate = 1.0817,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year
                }
            };

            var expected = new AnnualAverageRatesModel
            {
                ResultsInfo = new AnnualAverageRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length
                },
                Rates = rates
            };

            var actual = await _client.GetAnnualAverageRates(year, "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetAnnualAverageRates(Int32, IEnumerable<String>, String, Language)")]
        public async Task GetAnnualAverageRates2()
        {
            var year = 2020;

            var rates = new AnnualAverageRatesModel.ExchangeRateModel[]
            {
                new AnnualAverageRatesModel.ExchangeRateModel
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year
                },
                new AnnualAverageRatesModel.ExchangeRateModel
                {
                    Currency = "Pound Sterling",
                    Country = "UNITED KINGDOM",
                    IsoCode = "GBP",
                    UicCode = "002",
                    AvgRate = 0.89183,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                    ExchangeConventionCode = "C",
                    Year = year
                }
            };

            var expected = new AnnualAverageRatesModel
            {
                ResultsInfo = new AnnualAverageRatesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length
                },
                Rates = rates
            };

            var actual = await _client.GetAnnualAverageRates(year, rates.Select(a => a.IsoCode), "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetDailyTimeSeries(DateTime, DateTime, String, String, Language)")]
        public async Task GetDailyTimeSeries()
        {
            var startDate = new DateTime(2020, 12, 30);
            var endDate = new DateTime(2020, 12, 31);

            var rates = new DailyTimeSeriesModel.ExchangeRateModel[]
            {
                new DailyTimeSeriesModel.ExchangeRateModel
                {
                    ReferenceDate = startDate,
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                },
                new DailyTimeSeriesModel.ExchangeRateModel
                {
                    ReferenceDate = endDate,
                    AvgRate = 1.1625,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                }
            };

            var expected = new DailyTimeSeriesModel
            {
                ResultsInfo = new DailyTimeSeriesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length,
                    TimezoneReference = "Dates refer to the Central European Time Zone",
                    Currency = "U.S. Dollar",
                    IsoCode = "USD",
                    UicCode = "001",
                    ExchangeConventionCode = "C"
                },
                Rates = rates
            };

            var actual = await _client.GetDailyTimeSeries(startDate, endDate, "USD", "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetMonthlyTimeSeries(Int32, Int32, Int32, Int32, String, String, Language)")]
        public async Task GetMonthlyTimeSeries()
        {
            var startMonth = 1;
            var startYear = 2019;
            var endMonth = 12;
            var endYear = 2020;

            var rates = new MonthlyTimeSeriesModel.ExchangeRateModel[]
            {
                new MonthlyTimeSeriesModel.ExchangeRateModel
                {
                    ReferenceDate = new DateTime(startYear, startMonth, 1),
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                },
                new MonthlyTimeSeriesModel.ExchangeRateModel
                {
                    ReferenceDate = new DateTime(endYear, endMonth, 1),
                    AvgRate = 1.1625,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                }
            };

            var expected = new MonthlyTimeSeriesModel
            {
                ResultsInfo = new MonthlyTimeSeriesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length,
                    Currency = "U.S. Dollar",
                    IsoCode = "USD",
                    UicCode = "001",
                    ExchangeConventionCode = "C"
                },
                Rates = rates
            };

            var actual = await _client.GetMonthlyTimeSeries(startMonth, startYear, endMonth, endYear, "USD", "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact(DisplayName = "GetAnnualTimeSeries(Int32, Int32, String, String, Language)")]
        public async Task GetAnnualTimeSeries()
        {
            var startYear = 2019;
            var endYear = 2020;

            var rates = new AnnualTimeSeriesModel.ExchangeRateModel[]
            {
                new AnnualTimeSeriesModel.ExchangeRateModel
                {
                    ReferenceDate = startYear,
                    AvgRate = 1.1652,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                },
                new AnnualTimeSeriesModel.ExchangeRateModel
                {
                    ReferenceDate = endYear,
                    AvgRate = 1.1625,
                    ExchangeConvention = "Foreign currency amount for 1 Euro",
                }
            };

            var expected = new AnnualTimeSeriesModel
            {
                ResultsInfo = new AnnualTimeSeriesModel.ResultsInfoModel
                {
                    TotalRecords = rates.Length,
                    Currency = "U.S. Dollar",
                    IsoCode = "USD",
                    UicCode = "001",
                    ExchangeConventionCode = "C"
                },
                Rates = rates
            };

            var actual = await _client.GetAnnualTimeSeries(startYear, endYear, "USD", "EUR");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }
    }
}

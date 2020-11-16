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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIExchangeRates.Client.Tests
{
    class FakeMessageHandler : HttpMessageHandler
    {
        private static readonly Uri BaseAddress = new Uri("https://tassidicambio.bancaditalia.it/terzevalute-wf-web/rest/v1.0/");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var queryParameters = HttpUtility.ParseQueryString(request.RequestUri.Query);

            bool isEndpoint(HttpMethod method, string relativeUri) =>
                request.Method == method &&
                Uri.Compare(new Uri(BaseAddress, relativeUri), request.RequestUri,
                    UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path,
                    UriFormat.Unescaped, StringComparison.InvariantCultureIgnoreCase) == 0;

            if (isEndpoint(HttpMethod.Get, "latestRates"))
                return await Task.FromResult(GetLatestRates(queryParameters));

            if (isEndpoint(HttpMethod.Get, "dailyRates"))
                return await Task.FromResult(GetDailyRates(queryParameters));

            if (isEndpoint(HttpMethod.Get, "monthlyAverageRates"))
                return await Task.FromResult(GetMonthlyAverageRates(queryParameters));

            if (isEndpoint(HttpMethod.Get, "annualAverageRates"))
                return await Task.FromResult(GetAnnualAverageRates(queryParameters));

            if (isEndpoint(HttpMethod.Get, "dailyTimeSeries"))
                return await Task.FromResult(GetDailyTimeSeries(queryParameters));

            if (isEndpoint(HttpMethod.Get, "monthlyTimeSeries"))
                return await Task.FromResult(GetMonthlyTimeSeries(queryParameters));

            if (isEndpoint(HttpMethod.Get, "annualTimeSeries"))
                return await Task.FromResult(GetAnnualTimeSeries(queryParameters));

            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent($"Requested resource non found ({request.Method} {request.RequestUri}).")
            });
        }

        private HttpResponseMessage GetLatestRates(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out var response)) return response;

            var rates = new object[]
            {
                new
                {
                    Currency = "U.S. Dollar",
                    Country = "UNITED STATES",
                    IsoCode = "USD",
                    UicCode = "001",
                    EurRate = 1.1652,
                    UsdRate = 1,
                    UsdExchangeConvention = "-",
                    UsdExchangeConventionCode = "-",
                    ReferenceDate = "2020-12-30"
                },
                new
                {
                    Currency = "Euro",
                    Country = "EUROPEAN MONETARY UNION",
                    IsoCode = "EUR",
                    UicCode = "242",
                    EurRate = 1,
                    UsdRate = 0.8582,
                    UsdExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Dollaro"
                        : "Foreign currency amount for 1 Dollar",
                    UsdExchangeConventionCode = "C",
                    ReferenceDate = "2020-12-31"
                }
            };

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new
                    {
                        TotalRecords = rates.Length,
                        TimezoneReference = lang == Language.It
                            ? "Le date sono riferite al fuso orario dell'Europa Centrale"
                            : "Dates refer to the Central European Time Zone",
                        Notice = lang == Language.It
                            ? "Quantita' di valuta estera per 1 Euro"
                            : "Foreign currency amount for 1 Euro"
                    },
                    LatestRates = rates
                }))
            };
            return response;
        }

        private HttpResponseMessage GetDailyRates(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetDateTime("referenceDate", true, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var referenceDate, out var response))
                return response;

            if (!queryParameters.TryGetStrings("baseCurrencyIsoCode", false, out var baseCurrencyIsoCodes, out response))
                return response;

            if (!queryParameters.TryGetString("currencyIsoCode", true, out var currencyIsoCode, out response)) 
                return response;

            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out response)) 
                return response;

            var rates = new List<object>();

            if (currencyIsoCode == "EUR")
            {
                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("USD"))
                {
                    rates.Add(new
                    {
                        Currency = "U.S. Dollar",
                        Country = "UNITED STATES",
                        IsoCode = "USD",
                        UicCode = "001",
                        AvgRate = 1.1652,
                        ExchangeConvention = "Foreign currency amount for 1 Euro",
                        ExchangeConventionCode = "C",
                        ReferenceDate = referenceDate.ToString("yyyy-MM-dd")
                    });
                }

                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("GBP"))
                {
                    rates.Add(new
                    {
                        Currency = "Pound Sterling",
                        Country = "UNITED KINGDOM",
                        IsoCode = "GBP",
                        UicCode = "002",
                        AvgRate = 0.89183,
                        ExchangeConvention = "Foreign currency amount for 1 Euro",
                        ExchangeConventionCode = "C",
                        ReferenceDate = referenceDate.ToString("yyyy-MM-dd")
                    });
                }

                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("CHF"))
                {
                    rates.Add(new
                    {
                        Currency = "Swiss Franc",
                        Country = "SWITZERLAND",
                        IsoCode = "CHF",
                        UicCode = "003",
                        AvgRate = 1.0817,
                        ExchangeConvention = "Foreign currency amount for 1 Euro",
                        ExchangeConventionCode = "C",
                        ReferenceDate = referenceDate.ToString("yyyy-MM-dd")
                    });
                }
            }

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new
                    {
                        TotalRecords = rates.Count,
                        TimezoneReference = lang == Language.It
                            ? "Le date sono riferite al fuso orario dell'Europa Centrale"
                            : "Dates refer to the Central European Time Zone"
                    },
                    Rates = rates
                }))
            };
            return response;
        }

        private HttpResponseMessage GetMonthlyAverageRates(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetString("month", true, out var month, out var response))
                return response;

            if (!queryParameters.TryGetString("year", true, out var year, out response))
                return response;

            if (!queryParameters.TryGetStrings("baseCurrencyIsoCode", false, out var baseCurrencyIsoCodes, out response))
                return response;

            if (!queryParameters.TryGetString("currencyIsoCode", true, out var currencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out response))
                return response;

            var rates = new List<object>();

            if (currencyIsoCode == "EUR")
            {
                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("USD"))
                {
                    rates.Add(new
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
                    });
                }

                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("GBP"))
                {
                    rates.Add(new
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
                    });
                }

                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("CHF"))
                {
                    rates.Add(new
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
                    });
                }
            }

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new 
                    {
                        TotalRecords = rates.Count
                    },
                    Rates = rates
                }))
            };
            return response;
        }

        private HttpResponseMessage GetAnnualAverageRates(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetString("year", true, out var year, out var response))
                return response;

            if (!queryParameters.TryGetStrings("baseCurrencyIsoCode", false, out var baseCurrencyIsoCodes, out response))
                return response;

            if (!queryParameters.TryGetString("currencyIsoCode", true, out var currencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out response))
                return response;

            var rates = new List<object>();

            if (currencyIsoCode == "EUR")
            {
                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("USD"))
                {
                    rates.Add(new
                    {
                        Currency = "U.S. Dollar",
                        Country = "UNITED STATES",
                        IsoCode = "USD",
                        UicCode = "001",
                        AvgRate = 1.1652,
                        ExchangeConvention = "Foreign currency amount for 1 Euro",
                        ExchangeConventionCode = "C",
                        Year = year
                    });
                }

                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("GBP"))
                {
                    rates.Add(new
                    {
                        Currency = "Pound Sterling",
                        Country = "UNITED KINGDOM",
                        IsoCode = "GBP",
                        UicCode = "002",
                        AvgRate = 0.89183,
                        ExchangeConvention = "Foreign currency amount for 1 Euro",
                        ExchangeConventionCode = "C",
                        Year = year
                    });
                }

                if (baseCurrencyIsoCodes == default || baseCurrencyIsoCodes.Contains("CHF"))
                {
                    rates.Add(new
                    {
                        Currency = "Swiss Franc",
                        Country = "SWITZERLAND",
                        IsoCode = "CHF",
                        UicCode = "003",
                        AvgRate = 1.0817,
                        ExchangeConvention = "Foreign currency amount for 1 Euro",
                        ExchangeConventionCode = "C",
                        Year = year
                    });
                }
            }

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new
                    {
                        TotalRecords = rates.Count
                    },
                    Rates = rates
                }))
            };
            return response;
        }

        private HttpResponseMessage GetDailyTimeSeries(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetDateTime("startDate", true, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var startDate, out var response))
                return response;

            if (!queryParameters.TryGetDateTime("endDate", true, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var endDate, out response))
                return response;

            if (!queryParameters.TryGetString("baseCurrencyIsoCode", true, out var baseCurrencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetString("currencyIsoCode", true, out var currencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out response))
                return response;

            var rates = new List<object>();

            if (baseCurrencyIsoCode == "USD" && currencyIsoCode == "EUR")
            {
                rates.Add(new
                {
                    ReferenceDate = startDate.ToString("yyyy-MM-dd"),
                    AvgRate = 1.1652,
                    ExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Euro"
                        : "Foreign currency amount for 1 Euro"
                });
                
                rates.Add(new
                {
                    ReferenceDate = endDate.ToString("yyyy-MM-dd"),
                    AvgRate = 1.1625,
                    ExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Euro"
                        : "Foreign currency amount for 1 Euro"
                });
            }

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new
                    {
                        TotalRecords = rates.Count,
                        TimezoneReference = lang == Language.It
                            ? "Le date sono riferite al fuso orario dell'Europa Centrale"
                            : "Dates refer to the Central European Time Zone",
                        Currency = "U.S. Dollar",
                        IsoCode = "USD",
                        UicCode = "001",
                        ExchangeConventionCode = "C"
                    },
                    Rates = rates
                }))
            };
            return response;
        }

        private HttpResponseMessage GetMonthlyTimeSeries(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetString("startMonth", true, out var startMonth, out var response))
                return response;

            if (!queryParameters.TryGetString("startYear", true, out var startYear, out response))
                return response;

            if (!queryParameters.TryGetString("endMonth", true, out var endMonth, out response))
                return response;

            if (!queryParameters.TryGetString("endYear", true, out var endYear, out response))
                return response;

            if (!queryParameters.TryGetString("baseCurrencyIsoCode", true, out var baseCurrencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetString("currencyIsoCode", true, out var currencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out response))
                return response;

            var rates = new List<object>();

            if (baseCurrencyIsoCode == "USD" && currencyIsoCode == "EUR")
            {
                rates.Add(new
                {
                    ReferenceDate = $"{startYear}-{startMonth}",
                    AvgRate = 1.1652,
                    ExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Euro"
                        : "Foreign currency amount for 1 Euro"
                });

                rates.Add(new
                {
                    ReferenceDate = $"{endYear}-{endMonth}",
                    AvgRate = 1.1625,
                    ExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Euro"
                        : "Foreign currency amount for 1 Euro"
                });
            }

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new
                    {
                        TotalRecords = rates.Count,
                        Currency = "U.S. Dollar",
                        IsoCode = "USD",
                        UicCode = "001",
                        ExchangeConventionCode = "C"
                    },
                    Rates = rates
                }))
            };
            return response;
        }

        private HttpResponseMessage GetAnnualTimeSeries(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetString("startYear", true, out var startYear, out var response))
                return response;

            if (!queryParameters.TryGetString("endYear", true, out var endYear, out response))
                return response;

            if (!queryParameters.TryGetString("baseCurrencyIsoCode", true, out var baseCurrencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetString("currencyIsoCode", true, out var currencyIsoCode, out response))
                return response;

            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out response))
                return response;

            var rates = new List<object>();

            if (baseCurrencyIsoCode == "USD" && currencyIsoCode == "EUR")
            {
                rates.Add(new
                {
                    ReferenceDate = startYear,
                    AvgRate = 1.1652,
                    ExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Euro"
                        : "Foreign currency amount for 1 Euro"
                });

                rates.Add(new
                {
                    ReferenceDate = endYear,
                    AvgRate = 1.1625,
                    ExchangeConvention = lang == Language.It
                        ? "Quantita' di valuta estera per 1 Euro"
                        : "Foreign currency amount for 1 Euro"
                });
            }

            response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ResultsInfo = new
                    {
                        TotalRecords = rates.Count,
                        Currency = "U.S. Dollar",
                        IsoCode = "USD",
                        UicCode = "001",
                        ExchangeConventionCode = "C"
                    },
                    Rates = rates
                }))
            };
            return response;
        }
    }
}

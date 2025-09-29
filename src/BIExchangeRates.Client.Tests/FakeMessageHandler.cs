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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIExchangeRates.Client.Tests;

internal class FakeMessageHandler : HttpMessageHandler
{
	private static readonly Uri BaseAddress = new("https://tassidicambio.bancaditalia.it/terzevalute-wf-web/rest/v1.0/");

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
			return await Task.Run(() => GetLatestRates(queryParameters));

		if (isEndpoint(HttpMethod.Get, "dailyRates"))
			return await Task.Run(() => GetDailyRates(queryParameters));

		if (isEndpoint(HttpMethod.Get, "monthlyAverageRates"))
			return await Task.Run(() => GetMonthlyAverageRates(queryParameters));

		if (isEndpoint(HttpMethod.Get, "annualAverageRates"))
			return await Task.Run(() => GetAnnualAverageRates(queryParameters));

		if (isEndpoint(HttpMethod.Get, "dailyTimeSeries"))
			return await Task.Run(() => GetDailyTimeSeries(queryParameters));

		if (isEndpoint(HttpMethod.Get, "monthlyTimeSeries"))
			return await Task.Run(() => GetMonthlyTimeSeries(queryParameters));

		if (isEndpoint(HttpMethod.Get, "annualTimeSeries"))
			return await Task.Run(() => GetAnnualTimeSeries(queryParameters));

		if (isEndpoint(HttpMethod.Get, "currencies"))
			return await Task.Run(() => GetCurrencies(queryParameters));

		throw new Exception($"Requested resource non found ({request.Method} {request.RequestUri}).");
	}

	private static HttpResponseMessage GetLatestRates(NameValueCollection queryParameters)
	{
		var lang = queryParameters.GetEnum<Language>("lang", false);

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

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetDailyRates(NameValueCollection queryParameters)
	{
		var referenceDate = queryParameters.GetDateTime("referenceDate", true,
			"yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

		var baseCurrencyIsoCodes = queryParameters.GetStrings("baseCurrencyIsoCode", false);

		var currencyIsoCode = queryParameters.GetString("currencyIsoCode", true);

		var lang = queryParameters.GetEnum<Language>("lang", false);

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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
					ExchangeConventionCode = "C",
					ReferenceDate = referenceDate.ToString("yyyy-MM-dd")
				});
			}
		}

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetMonthlyAverageRates(NameValueCollection queryParameters)
	{
		var month = queryParameters.GetInt("month", true);

		var year = queryParameters.GetInt("year", true);

		var baseCurrencyIsoCodes = queryParameters.GetStrings("baseCurrencyIsoCode", false);

		var currencyIsoCode = queryParameters.GetString("currencyIsoCode", true);

		var lang = queryParameters.GetEnum<Language>("lang", false);

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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
					ExchangeConventionCode = "C",
					Year = year,
					Month = month
				});
			}
		}

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetAnnualAverageRates(NameValueCollection queryParameters)
	{
		var year = queryParameters.GetInt("year", true);

		var baseCurrencyIsoCodes = queryParameters.GetStrings("baseCurrencyIsoCode", false);

		var currencyIsoCode = queryParameters.GetString("currencyIsoCode", true);

		var lang = queryParameters.GetEnum<Language>("lang", false);

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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
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
					ExchangeConvention = lang == Language.It
						? "Quantita' di valuta estera per 1 Euro"
						: "Foreign currency amount for 1 Euro",
					ExchangeConventionCode = "C",
					Year = year
				});
			}
		}

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetDailyTimeSeries(NameValueCollection queryParameters)
	{
		var startDate = queryParameters.GetDateTime("startDate", true,
			"yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

		var endDate = queryParameters.GetDateTime("endDate", true,
			"yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

		var baseCurrencyIsoCode = queryParameters.GetString("baseCurrencyIsoCode", true);

		var currencyIsoCode = queryParameters.GetString("currencyIsoCode", true);

		var lang = queryParameters.GetEnum<Language>("lang", false);

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

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetMonthlyTimeSeries(NameValueCollection queryParameters)
	{
		var startMonth = queryParameters.GetInt("startMonth", true);

		var startYear = queryParameters.GetInt("startYear", true);

		var endMonth = queryParameters.GetInt("endMonth", true);

		var endYear = queryParameters.GetInt("endYear", true);

		var baseCurrencyIsoCode = queryParameters.GetString("baseCurrencyIsoCode", true);

		var currencyIsoCode = queryParameters.GetString("currencyIsoCode", true);

		var lang = queryParameters.GetEnum<Language>("lang", false);

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

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetAnnualTimeSeries(NameValueCollection queryParameters)
	{
		var startYear = queryParameters.GetInt("startYear", true);

		var endYear = queryParameters.GetInt("endYear", true);

		var baseCurrencyIsoCode = queryParameters.GetString("baseCurrencyIsoCode", true);

		var currencyIsoCode = queryParameters.GetString("currencyIsoCode", true);

		var lang = queryParameters.GetEnum<Language>("lang", false);

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

		return new HttpResponseMessage()
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
	}

	private static HttpResponseMessage GetCurrencies(NameValueCollection queryParameters)
	{
		var lang = queryParameters.GetEnum<Language>("lang", false);

		var currencies = new object[]
		{
			new
			{
				Countries = new object[]
				{
					new
					{
						CurrencyIso = "USD",
						Country = "UNITED STATES",
						CountryIso = "US",
						ValidityStartDate = "1918-01-02",
						ValidityEndDate = (string)null
					}
				},
				IsoCode = "USD",
				Name = "U.S. Dollar",
				Graph = false
			},
			new
			{
				Countries = new object[]
				{
					new
					{
						CurrencyIso = "EUR",
						Country = "EUROPEAN MONETARY UNION",
						CountryIso = "XX",
						ValidityStartDate = "1999-01-01",
						ValidityEndDate = (string)null
					}
				},
				IsoCode = "EUR",
				Name = "Euro",
				Graph = false
			},
			new
			{
				Countries = new object[]
				{
					new
					{
						CurrencyIso = "ITL",
						Country = "ITALY",
						CountryIso = "IT",
						ValidityStartDate = "1918-02-01",
						ValidityEndDate = "2001-12-28",
					}
				},
				IsoCode = "ITL",
				Name = "Italian Lira",
				Graph = false
			}
		};

		return new HttpResponseMessage()
		{
			Content = new StringContent(JsonConvert.SerializeObject(new
			{
				ResultsInfo = new
				{
					TotalRecords = currencies.Length,
					TimezoneReference = lang == Language.It
						? "Le date sono riferite al fuso orario dell'Europa Centrale"
						: "Dates refer to the Central European Time Zone"
				},
				Currencies = currencies
			}))
		};
	}
}

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
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BIExchangeRates.Client.Tests;

public class TestResults
{
	[Theory]
	[InlineData("0.12345", "1.23456", 0.12345, 1.23456)]
	[InlineData("N.A.", "1.23456", 0, 1.23456)]
	[InlineData("0.12345", "N.A.", 0.12345, 0)]
	public async Task GetLatestRates(string eurRate, string usdRate, double expectedEurRate, double expectedUsdRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
							timezoneReference = "timezoneReference",
							notice = "notice",
						},
						latestRates = new object[]
						{
							new
							{
								currency = "currency",
								country = "country",
								isoCode = "isoCode",
								uicCode = "uicCode",
								eurRate,
								usdRate,
								usdExchangeConvention = "usdExchangeConvention",
								usdExchangeConventionCode = "usdExchangeConventionCode",
								referenceDate = "2020-01-02",
							}
						}
					}))
				}
			)
		);

		var results = await client.GetLatestRates();
		Assert.Equal(1, results.ResultsInfo.TotalRecords);
		Assert.Equal("timezoneReference", results.ResultsInfo.TimezoneReference);
		Assert.Equal("notice", results.ResultsInfo.Notice);

		var rates = Assert.Single(results.LatestRates);
		Assert.Equal("currency", rates.Currency);
		Assert.Equal("country", rates.Country);
		Assert.Equal("isoCode", rates.IsoCode);
		Assert.Equal("uicCode", rates.UicCode);
		Assert.Equal(expectedEurRate, rates.EurRate);
		Assert.Equal(expectedUsdRate, rates.UsdRate);
		Assert.Equal("usdExchangeConvention", rates.UsdExchangeConvention);
		Assert.Equal("usdExchangeConventionCode", rates.UsdExchangeConventionCode);
		Assert.Equal(new DateTime(2020, 1, 2), rates.ReferenceDate);
	}

	[Theory]
	[InlineData("0.12345", 0.12345)]
	[InlineData("N.A.", 0)]
	public async Task GetDailyRates(string avgRate, double expectedAvgRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
							timezoneReference = "timezoneReference",
						},
						rates = new object[]
						{
							new
							{
								currency = "currency",
								country = "country",
								isoCode = "isoCode",
								uicCode = "uicCode",
								avgRate,
								exchangeConvention = "exchangeConvention",
								exchangeConventionCode = "exchangeConventionCode",
								referenceDate = "2020-01-02",
							}
						}
					}))
				}
			)
		);

		var results = await client.GetDailyRates(DateTime.MinValue, "EUR");
		Assert.Equal(1, results.ResultsInfo.TotalRecords);
		Assert.Equal("timezoneReference", results.ResultsInfo.TimezoneReference);

		var rates = Assert.Single(results.Rates);
		Assert.Equal("currency", rates.Currency);
		Assert.Equal("country", rates.Country);
		Assert.Equal("isoCode", rates.IsoCode);
		Assert.Equal("uicCode", rates.UicCode);
		Assert.Equal(expectedAvgRate, rates.AvgRate);
		Assert.Equal("exchangeConvention", rates.ExchangeConvention);
		Assert.Equal("exchangeConventionCode", rates.ExchangeConventionCode);
		Assert.Equal(new DateTime(2020, 1, 2), rates.ReferenceDate);
	}

	[Theory]
	[InlineData("0.12345", 0.12345)]
	[InlineData("N.A.", 0)]
	public async Task GetMonthlyAverageRates(string avgRate, double expectedAvgRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
						},
						rates = new object[]
						{
							new
							{
								currency = "currency",
								country = "country",
								isoCode = "isoCode",
								uicCode = "uicCode",
								avgRate,
								exchangeConvention = "exchangeConvention",
								exchangeConventionCode = "exchangeConventionCode",
								year = 2020,
								month = 1,
							}
						}
					}))
				}
			)
		);

		var results = await client.GetMonthlyAverageRates(1, 2020, "EUR");
		Assert.Equal(1, results.ResultsInfo.TotalRecords);

		var rates = Assert.Single(results.Rates);
		Assert.Equal("currency", rates.Currency);
		Assert.Equal("country", rates.Country);
		Assert.Equal("isoCode", rates.IsoCode);
		Assert.Equal("uicCode", rates.UicCode);
		Assert.Equal(expectedAvgRate, rates.AvgRate);
		Assert.Equal("exchangeConvention", rates.ExchangeConvention);
		Assert.Equal("exchangeConventionCode", rates.ExchangeConventionCode);
		Assert.Equal(2020, rates.Year);
		Assert.Equal(1, rates.Month);
	}

	[Theory]
	[InlineData("0.12345", 0.12345)]
	[InlineData("N.A.", 0)]
	public async Task GetAnnualAverageRates(string avgRate, double expectedAvgRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
						},
						rates = new object[]
						{
							new
							{
								currency = "currency",
								country = "country",
								isoCode = "isoCode",
								uicCode = "uicCode",
								avgRate,
								exchangeConvention = "exchangeConvention",
								exchangeConventionCode = "exchangeConventionCode",
								year = 2020,
							}
						}
					}))
				}
			)
		);

		var results = await client.GetAnnualAverageRates(2020, "EUR");
		Assert.Equal(1, results.ResultsInfo.TotalRecords);

		var rates = Assert.Single(results.Rates);
		Assert.Equal("currency", rates.Currency);
		Assert.Equal("country", rates.Country);
		Assert.Equal("isoCode", rates.IsoCode);
		Assert.Equal("uicCode", rates.UicCode);
		Assert.Equal(expectedAvgRate, rates.AvgRate);
		Assert.Equal("exchangeConvention", rates.ExchangeConvention);
		Assert.Equal("exchangeConventionCode", rates.ExchangeConventionCode);
		Assert.Equal(2020, rates.Year);
	}

	[Theory]
	[InlineData("0.12345", 0.12345)]
	[InlineData("N.A.", 0)]
	public async Task GetDailyTimeSeries(string avgRate, double expectedAvgRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
							timezoneReference = "timezoneReference",
							currency = "currency",
							isoCode = "isoCode",
							uicCode = "uicCode",
							exchangeConventionCode = "exchangeConventionCode",
						},
						rates = new object[]
						{
							new
							{
								referenceDate = "2020-01-02",
								avgRate,
								exchangeConvention = "exchangeConvention",
							}
						}
					}))
				}
			)
		);

		var results = await client.GetDailyTimeSeries(DateTime.MinValue, DateTime.MaxValue, "USD", "EUR");
		Assert.Equal(1, results.ResultsInfo.TotalRecords);
		Assert.Equal("timezoneReference", results.ResultsInfo.TimezoneReference);
		Assert.Equal("currency", results.ResultsInfo.Currency);
		Assert.Equal("isoCode", results.ResultsInfo.IsoCode);
		Assert.Equal("uicCode", results.ResultsInfo.UicCode);
		Assert.Equal("exchangeConventionCode", results.ResultsInfo.ExchangeConventionCode);

		var rates = Assert.Single(results.Rates);
		Assert.Equal(new DateTime(2020, 1, 2), rates.ReferenceDate);
		Assert.Equal(expectedAvgRate, rates.AvgRate);
		Assert.Equal("exchangeConvention", rates.ExchangeConvention);
	}

	[Theory]
	[InlineData("0.12345", 0.12345)]
	[InlineData("N.A.", 0)]
	public async Task GetMonthlyTimeSeries(string avgRate, double expectedAvgRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
							currency = "currency",
							isoCode = "isoCode",
							uicCode = "uicCode",
							exchangeConventionCode = "exchangeConventionCode",
						},
						rates = new object[]
						{
							new
							{
								referenceDate = "2020-01",
								avgRate,
								exchangeConvention = "exchangeConvention",
							}
						}
					}))
				}
			)
		);

		var results = await client.GetMonthlyTimeSeries(1, 2020, 2, 2021, "USD", "EUR");
		Assert.Equal(1, results.ResultsInfo.TotalRecords);
		Assert.Equal("currency", results.ResultsInfo.Currency);
		Assert.Equal("isoCode", results.ResultsInfo.IsoCode);
		Assert.Equal("uicCode", results.ResultsInfo.UicCode);
		Assert.Equal("exchangeConventionCode", results.ResultsInfo.ExchangeConventionCode);

		var rates = Assert.Single(results.Rates);
		Assert.Equal(new DateTime(2020, 1, 1), rates.ReferenceDate);
		Assert.Equal(expectedAvgRate, rates.AvgRate);
		Assert.Equal("exchangeConvention", rates.ExchangeConvention);
	}

	[Theory]
	[InlineData("0.12345", 0.12345)]
	[InlineData("N.A.", 0)]
	public async Task GetAnnualTimeSeries(string avgRate, double expectedAvgRate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
							currency = "currency",
							isoCode = "isoCode",
							uicCode = "uicCode",
							exchangeConventionCode = "exchangeConventionCode",
						},
						rates = new object[]
						{
							new
							{
								referenceDate = "2020",
								avgRate,
								exchangeConvention = "exchangeConvention",
							}
						}
					}))
				}
			)
		);

		var results = await client.GetAnnualTimeSeries(2020, 2021, "USD", "EUR");
		Assert.Equal(1, results.ResultsInfo.TotalRecords);
		Assert.Equal("currency", results.ResultsInfo.Currency);
		Assert.Equal("isoCode", results.ResultsInfo.IsoCode);
		Assert.Equal("uicCode", results.ResultsInfo.UicCode);
		Assert.Equal("exchangeConventionCode", results.ResultsInfo.ExchangeConventionCode);

		var rates = Assert.Single(results.Rates);
		Assert.Equal(2020, rates.ReferenceDate);
		Assert.Equal(expectedAvgRate, rates.AvgRate);
		Assert.Equal("exchangeConvention", rates.ExchangeConvention);
	}

	[Theory]
	[InlineData("2020-03-04", "2020-3-4")]
	[InlineData(null, null)]
	public async Task GetCurrencies(string validityEndDate, string expectedValidityEndDate)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				_ => new HttpResponseMessage
				{
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						resultsInfo = new
						{
							totalRecords = 1,
							timezoneReference = "timezoneReference",
						},
						currencies = new object[]
						{
							new
							{
								countries = new object[]
								{
									new
									{
										currencyIso = "currencyIso",
										country = "country",
										countryIso = "countryIso",
										validityStartDate = "2020-01-02",
										validityEndDate
									}
								},
								isoCode = "isoCode",
								name = "name",
								graph = true
							}
						}
					}))
				}
			)
		);

		var results = await client.GetCurrencies();
		Assert.Equal(1, results.ResultsInfo.TotalRecords);
		Assert.Equal("timezoneReference", results.ResultsInfo.TimezoneReference);

		var currencies = Assert.Single(results.Currencies);
		Assert.Equal("isoCode", currencies.IsoCode);
		Assert.Equal("name", currencies.Name);
		Assert.True(currencies.Graph);

		var countries = Assert.Single(currencies.Countries);
		Assert.Equal("currencyIso", countries.CurrencyIso);
		Assert.Equal("country", countries.Country);
		Assert.Equal("countryIso", countries.CountryIso);
		Assert.Equal(new DateTime(2020, 1, 2), countries.ValidityStartDate);
		Assert.Equal(expectedValidityEndDate == null ? null : DateTime.Parse(expectedValidityEndDate), countries.ValidityEndDate);
	}
}

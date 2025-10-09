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

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BIExchangeRates.Client.Tests;

public class TestRequests
{
	private const string BaseAddress = "https://tassidicambio.bancaditalia.it/terzevalute-wf-web/rest/v1.0/";

	private static HttpResponseMessage CreateDefaultResponseMessage()
	{
		return new() { Content = new StringContent("{}") };
	}

	[Theory]
	[InlineData(Language.En, BaseAddress + "latestRates?lang=En")]
	[InlineData(Language.It, BaseAddress + "latestRates?lang=It")]
	public async Task GetLatestRates(Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetLatestRates(language);
	}

	[Theory]
	[InlineData("EUR", Language.En, BaseAddress + "dailyRates?referenceDate=2020-01-02&currencyIsoCode=EUR&lang=En")]
	[InlineData("USD", Language.En, BaseAddress + "dailyRates?referenceDate=2020-01-02&currencyIsoCode=USD&lang=En")]
	[InlineData("EUR", Language.It, BaseAddress + "dailyRates?referenceDate=2020-01-02&currencyIsoCode=EUR&lang=It")]
	public async Task GetDailyRates1(string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetDailyRates(new DateTime(2020, 1, 2), currency, language);
	}

	[Theory]
	[InlineData(new string[0], "EUR", Language.En, BaseAddress + "dailyRates?referenceDate=2020-01-02&currencyIsoCode=EUR&lang=En")]
	[InlineData(new[] { "USD" }, "EUR", Language.En, BaseAddress + "dailyRates?referenceDate=2020-01-02&baseCurrencyIsoCode=USD&currencyIsoCode=EUR&lang=En")]
	[InlineData(new[] { "USD", "GBP" }, "EUR", Language.En, BaseAddress + "dailyRates?referenceDate=2020-01-02&baseCurrencyIsoCode=USD&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=En")]
	[InlineData(new string[0], "USD", Language.En, BaseAddress + "dailyRates?referenceDate=2020-01-02&currencyIsoCode=USD&lang=En")]
	[InlineData(new string[0], "EUR", Language.It, BaseAddress + "dailyRates?referenceDate=2020-01-02&currencyIsoCode=EUR&lang=It")]
	public async Task GetDailyRates2(string[] baseCurrencies, string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetDailyRates(new DateTime(2020, 1, 2), baseCurrencies, currency, language);
	}

	[Theory]
	[InlineData("EUR", Language.En, BaseAddress + "monthlyAverageRates?month=1&year=2020&currencyIsoCode=EUR&lang=En")]
	[InlineData("USD", Language.En, BaseAddress + "monthlyAverageRates?month=1&year=2020&currencyIsoCode=USD&lang=En")]
	[InlineData("EUR", Language.It, BaseAddress + "monthlyAverageRates?month=1&year=2020&currencyIsoCode=EUR&lang=It")]
	public async Task GetMonthlyAverageRates1(string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetMonthlyAverageRates(1, 2020, currency, language);
	}

	[Theory]
	[InlineData(new string[0], "EUR", Language.En, BaseAddress + "monthlyAverageRates?month=1&year=2020&currencyIsoCode=EUR&lang=En")]
	[InlineData(new[] { "USD" }, "EUR", Language.En, BaseAddress + "monthlyAverageRates?month=1&year=2020&baseCurrencyIsoCode=USD&currencyIsoCode=EUR&lang=En")]
	[InlineData(new[] { "USD", "GBP" }, "EUR", Language.En, BaseAddress + "monthlyAverageRates?month=1&year=2020&baseCurrencyIsoCode=USD&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=En")]
	[InlineData(new string[0], "USD", Language.En, BaseAddress + "monthlyAverageRates?month=1&year=2020&currencyIsoCode=USD&lang=En")]
	[InlineData(new string[0], "EUR", Language.It, BaseAddress + "monthlyAverageRates?month=1&year=2020&currencyIsoCode=EUR&lang=It")]
	public async Task GetMonthlyAverageRates2(string[] baseCurrencies, string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetMonthlyAverageRates(1, 2020, baseCurrencies, currency, language);
	}

	[Theory]
	[InlineData("EUR", Language.En, BaseAddress + "annualAverageRates?year=2020&currencyIsoCode=EUR&lang=En")]
	[InlineData("USD", Language.En, BaseAddress + "annualAverageRates?year=2020&currencyIsoCode=USD&lang=En")]
	[InlineData("EUR", Language.It, BaseAddress + "annualAverageRates?year=2020&currencyIsoCode=EUR&lang=It")]
	public async Task GetAnnualAverageRates1(string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetAnnualAverageRates(2020, currency, language);
	}

	[Theory]
	[InlineData(new string[0], "EUR", Language.En, BaseAddress + "annualAverageRates?year=2020&currencyIsoCode=EUR&lang=En")]
	[InlineData(new[] { "USD" }, "EUR", Language.En, BaseAddress + "annualAverageRates?year=2020&baseCurrencyIsoCode=USD&currencyIsoCode=EUR&lang=En")]
	[InlineData(new[] { "USD", "GBP" }, "EUR", Language.En, BaseAddress + "annualAverageRates?year=2020&baseCurrencyIsoCode=USD&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=En")]
	[InlineData(new string[0], "USD", Language.En, BaseAddress + "annualAverageRates?year=2020&currencyIsoCode=USD&lang=En")]
	[InlineData(new string[0], "EUR", Language.It, BaseAddress + "annualAverageRates?year=2020&currencyIsoCode=EUR&lang=It")]
	public async Task GetAnnualAverageRates2(string[] baseCurrencies, string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetAnnualAverageRates(2020, baseCurrencies, currency, language);
	}

	[Theory]
	[InlineData("GBP", "EUR", Language.En, BaseAddress + "dailyTimeSeries?startDate=2020-01-02&endDate=2021-03-04&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=En")]
	[InlineData("USD", "EUR", Language.En, BaseAddress + "dailyTimeSeries?startDate=2020-01-02&endDate=2021-03-04&baseCurrencyIsoCode=USD&currencyIsoCode=EUR&lang=En")]
	[InlineData("GBP", "USD", Language.En, BaseAddress + "dailyTimeSeries?startDate=2020-01-02&endDate=2021-03-04&baseCurrencyIsoCode=GBP&currencyIsoCode=USD&lang=En")]
	[InlineData("GBP", "EUR", Language.It, BaseAddress + "dailyTimeSeries?startDate=2020-01-02&endDate=2021-03-04&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=It")]
	public async Task GetDailyTimeSeries(string baseCurrency, string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetDailyTimeSeries(new DateTime(2020, 1, 2), new DateTime(2021, 3, 4), baseCurrency, currency, language);
	}

	[Theory]
	[InlineData("GBP", "EUR", Language.En, BaseAddress + "monthlyTimeSeries?startMonth=1&startYear=2020&endMonth=12&endYear=2021&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=En")]
	[InlineData("USD", "EUR", Language.En, BaseAddress + "monthlyTimeSeries?startMonth=1&startYear=2020&endMonth=12&endYear=2021&baseCurrencyIsoCode=USD&currencyIsoCode=EUR&lang=En")]
	[InlineData("GBP", "USD", Language.En, BaseAddress + "monthlyTimeSeries?startMonth=1&startYear=2020&endMonth=12&endYear=2021&baseCurrencyIsoCode=GBP&currencyIsoCode=USD&lang=En")]
	[InlineData("GBP", "EUR", Language.It, BaseAddress + "monthlyTimeSeries?startMonth=1&startYear=2020&endMonth=12&endYear=2021&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=It")]
	public async Task GetMonthlyTimeSeries(string baseCurrency, string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetMonthlyTimeSeries(1, 2020, 12, 2021, baseCurrency, currency, language);
	}

	[Theory]
	[InlineData("GBP", "EUR", Language.En, BaseAddress + "annualTimeSeries?startYear=2020&endYear=2021&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=En")]
	[InlineData("USD", "EUR", Language.En, BaseAddress + "annualTimeSeries?startYear=2020&endYear=2021&baseCurrencyIsoCode=USD&currencyIsoCode=EUR&lang=En")]
	[InlineData("GBP", "USD", Language.En, BaseAddress + "annualTimeSeries?startYear=2020&endYear=2021&baseCurrencyIsoCode=GBP&currencyIsoCode=USD&lang=En")]
	[InlineData("GBP", "EUR", Language.It, BaseAddress + "annualTimeSeries?startYear=2020&endYear=2021&baseCurrencyIsoCode=GBP&currencyIsoCode=EUR&lang=It")]
	public async Task GetAnnualTimeSeries(string baseCurrency, string currency, Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetAnnualTimeSeries(2020, 2021, baseCurrency, currency, language);
	}

	[Theory]
	[InlineData(Language.En, BaseAddress + "currencies?lang=En")]
	[InlineData(Language.It, BaseAddress + "currencies?lang=It")]
	public async Task GetCurrencies(Language language, string expectedRequestUri)
	{
		var client = new ExchangeRatesClient(
			new CustomHttpMessageHandler(
				request =>
				{
					Assert.Equal(HttpMethod.Get, request.Method);

					Assert.Equal(expectedRequestUri, request.RequestUri?.ToString());

					return CreateDefaultResponseMessage();
				}
			)
		);

		await client.GetCurrencies(language);
	}
}

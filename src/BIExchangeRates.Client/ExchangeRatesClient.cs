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
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BIExchangeRates.Client
{
    /// <summary>
    /// Provides a wrapper for the REST API of the currency exchange rates of Banca d'Italia (https://tassidicambio.bancaditalia.it).
    /// </summary>
    public sealed class ExchangeRatesClient : HttpClient, IExchangeRatesClient
	{
		/// <summary>
		/// Initializes a new instance of the ExchangeRatesClient class using a HttpClientHandler that is disposed when this instance is disposed.
		/// </summary>
		public ExchangeRatesClient() : this(new HttpClientHandler(), true) { }

		/// <summary>
		/// Initializes a new instance of the ExchangeRatesClient class with the specified handler. The handler is disposed when this instance is disposed.
		/// </summary>
		/// <param name="handler">The HttpMessageHandler responsible for processing the HTTP response messages.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public ExchangeRatesClient(HttpMessageHandler handler) : this(handler, true) { }

		/// <summary>
		/// Initializes a new instance of the ExchangeRatesClient class with the provided handler, and specifies whether that handler should be disposed when this instance is disposed.
		/// </summary>
		/// <param name="handler">The HttpMessageHandler responsible for processing the HTTP response messages.</param>
		/// <param name="disposeHandler">true if the inner handler should be disposed of by ExchangeRatesClient.Dispose; false if you intend to reuse the inner handler.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public ExchangeRatesClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {
			BaseAddress = new Uri("https://tassidicambio.bancaditalia.it/terzevalute-wf-web/rest/v1.0/");
			DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
		}

		/// <summary>
		/// Returns the latest available exchange rates for all the valid currencies.
		/// </summary>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the latest available exchange rates for all the valid currencies.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<LatestRatesModel> GetLatestRates(Language language = Language.En) =>
			await GetModel<LatestRatesModel>($"latestRates?lang={language}");

		/// <summary>
		/// Returns the daily exchange rates for a specific date for all the available currencies.
		/// </summary>
		/// <param name="referenceDate">The reference date for the exchange rates.</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for all the available currencies.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, string currencyIsoCode,
			Language language = Language.En) =>
			await GetDailyRates(referenceDate, new List<string>(), currencyIsoCode, language);

		/// <summary>
		/// Returns the daily exchange rates for a specific date for a list of currencies.
		/// </summary>
		/// <param name="referenceDate">The reference date for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCodes">The list of ISO codes of the required currencies (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for a list of currencies.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, IEnumerable<string> baseCurrencyIsoCodes,
			string currencyIsoCode, Language language = Language.En)
		{
			if (baseCurrencyIsoCodes is null) throw new ArgumentNullException(nameof(baseCurrencyIsoCodes));
			var baseCurrencyIsoCodeList = baseCurrencyIsoCodes.ToList();
			return await GetModel<DailyRatesModel>(new StringBuilder("dailyRates")
				.Append("?referenceDate=").Append(referenceDate.ToString("yyyy-MM-dd"))
				.Append(baseCurrencyIsoCodeList.Count > 0 ? "&" : string.Empty)
				.AppendJoin("&", baseCurrencyIsoCodeList.Select(isoCode => $"baseCurrencyIsoCode={isoCode}"))
				.Append("&currencyIsoCode=").Append(currencyIsoCode)
				.Append("&lang=").Append(language)
				.ToString());
		}

		/// <summary>
		/// Returns the monthly average exchange rates for specific month and year for all the available currencies.
		/// </summary>
		/// <param name="month">The reference month for the exchange rates (1-12).</param>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the monthly average exchange rates for specific month and year for all the available currencies.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year, string currencyIsoCode,
			Language language = Language.En) =>
			await GetMonthlyAverageRates(month, year, new List<string>(), currencyIsoCode, language);

		/// <summary>
		/// Returns the monthly average exchange rates for specific month and year for a list of currencies.
		/// </summary>
		/// <param name="month">The reference month for the exchange rates (1-12).</param>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCodes">The list of ISO codes of the required currencies (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the monthly average exchange rates for specific month and year for a list of currencies.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year,
			IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En)
		{
			if (baseCurrencyIsoCodes is null) throw new ArgumentNullException(nameof(baseCurrencyIsoCodes));
			var baseCurrencyIsoCodeList = baseCurrencyIsoCodes.ToList();
			return await GetModel<MonthlyAverageRatesModel>(new StringBuilder("monthlyAverageRates")
				.Append("?month=").Append(month)
				.Append("&year=").Append(year)
				.Append(baseCurrencyIsoCodeList.Count > 0 ? "&" : string.Empty)
				.AppendJoin("&", baseCurrencyIsoCodeList.Select(isoCode => $"baseCurrencyIsoCode={isoCode}"))
				.Append("&currencyIsoCode=").Append(currencyIsoCode)
				.Append("&lang=").Append(language)
				.ToString());
		}

		/// <summary>
		/// Returns the annual average exchange rates for a specific year for all the available currencies.
		/// </summary>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the annual average exchange rates for a specific year for all the available currencies.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year, string currencyIsoCode,
			Language language = Language.En) =>
			await GetAnnualAverageRates(year, new List<string>(), currencyIsoCode, language);

		/// <summary>
		/// Returns the annual average exchange rates for a specific year for a list of currencies.
		/// </summary>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCodes">The list of ISO codes of the required currencies (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the annual average exchange rates for a specific year for a list of currencies.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year,
			IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En)
		{
			if (baseCurrencyIsoCodes is null) throw new ArgumentNullException(nameof(baseCurrencyIsoCodes));
			var baseCurrencyIsoCodeList = baseCurrencyIsoCodes.ToList();
			return await GetModel<AnnualAverageRatesModel>(new StringBuilder("annualAverageRates")
				.Append("?year=").Append(year)
				.Append(baseCurrencyIsoCodeList.Count > 0 ? "&" : string.Empty)
				.AppendJoin("&", baseCurrencyIsoCodeList.Select(isoCode => $"baseCurrencyIsoCode={isoCode}"))
				.Append("&currencyIsoCode=").Append(currencyIsoCode)
				.Append("&lang=").Append(language)
				.ToString());
		}

		/// <summary>
		/// Returns the daily exchange rates of a currency for a specific date range.
		/// </summary>
		/// <param name="startDate">The start date of the range for the exchange rates.</param>
		/// <param name="endDate">The end date of the range for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCode">The ISO code of the required currency (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the daily exchange rates of a currency for a specific date range.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<DailyTimeSeriesModel> GetDailyTimeSeries(DateTime startDate, DateTime endDate,
			string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En) =>
			await GetModel<DailyTimeSeriesModel>(new StringBuilder("dailyTimeSeries")
				.Append("?startDate=").Append(startDate.ToString("yyyy-MM-dd"))
				.Append("&endDate=").Append(endDate.ToString("yyyy-MM-dd"))
				.Append("&baseCurrencyIsoCode=").Append(baseCurrencyIsoCode)
				.Append("&currencyIsoCode=").Append(currencyIsoCode)
				.Append("&lang=").Append(language)
				.ToString());

		/// <summary>
		/// Returns the monthly average exchange rates of a currency for a specific month range.
		/// </summary>
		/// <param name="startMonth">The start month of the range for the exchange rates (1-12).</param>
		/// <param name="startYear">The start year of the range for the exchange rates.</param>
		/// <param name="endMonth">The end month of the range for the exchange rates (1-12).</param>
		/// <param name="endYear">The end year of the range for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCode">The ISO code of the required currency (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the monthly average exchange rates of a currency for a specific month range.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<MonthlyTimeSeriesModel> GetMonthlyTimeSeries(int startMonth, int startYear, int endMonth, int endYear,
			string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En) =>
			await GetModel<MonthlyTimeSeriesModel>(new StringBuilder("monthlyTimeSeries")
				.Append("?startMonth=").Append(startMonth)
				.Append("&startYear=").Append(startYear)
				.Append("&endMonth=").Append(endMonth)
				.Append("&endYear=").Append(endYear)
				.Append("&baseCurrencyIsoCode=").Append(baseCurrencyIsoCode)
				.Append("&currencyIsoCode=").Append(currencyIsoCode)
				.Append("&lang=").Append(language)
				.ToString());

		/// <summary>
		/// Returns the annual average exchange rates of a currency for a specific year range.
		/// </summary>
		/// <param name="startYear">The start year of the range for the exchange rates.</param>
		/// <param name="endYear">The end year of the range for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCode">The ISO code of the required currency (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the annual average exchange rates of a currency for a specific year range.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<AnnualTimeSeriesModel> GetAnnualTimeSeries(int startYear, int endYear, string baseCurrencyIsoCode,
			string currencyIsoCode, Language language = Language.En) =>
			await GetModel<AnnualTimeSeriesModel>(new StringBuilder("annualTimeSeries")
				.Append("?startYear=").Append(startYear)
				.Append("&endYear=").Append(endYear)
				.Append("&baseCurrencyIsoCode=").Append(baseCurrencyIsoCode)
				.Append("&currencyIsoCode=").Append(currencyIsoCode)
				.Append("&lang=").Append(language)
				.ToString());

		/// <summary>
		/// Returns the list of all the available currencies.
		/// </summary>
		/// <param name="language">The language of the returned data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the list of all the available currencies.</returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<CurrenciesModel> GetCurrencies(Language language = Language.En) =>
			await GetModel<CurrenciesModel>($"currencies?lang={language}");

		private async Task<T> GetModel<T>(string requestUri)
		{
			var response = await GetAsync(requestUri);
			var content = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException(
					$"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}). Response content: {content}");
			return JsonConvert.DeserializeObject<T>(content);
		}
	}
}
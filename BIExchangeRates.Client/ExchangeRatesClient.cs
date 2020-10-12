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
	/// A wrapper for the REST API of the currency exchange rates of Banca d'Italia (https://tassidicambio.bancaditalia.it).
	/// </summary>
	public sealed class ExchangeRatesClient : IExchangeRatesClient
	{
		private const string BaseAddress = "https://tassidicambio.bancaditalia.it/terzevalute-wf-web/rest/v1.0/";

		private const string LatestRatesPath = "latestRates";

		private const string DailyRatesPath = "dailyRates";

		private const string MonthlyAverageRatesPath = "monthlyAverageRates";

		private const string AnnualAverageRatesPath = "annualAverageRates";

		private const string DailyTimeSeriesPath = "dailyTimeSeries";

		private const string MonthlyTimeSeriesPath = "monthlyTimeSeries";

		private const string AnnualTimeSeriesPath = "annualTimeSeries";

		private const string CurrenciesPath = "currencies";

		private readonly HttpClient _httpClient;

		/// <summary>
		/// Initializes a new instance of the ExchangeRatesClient class.
		/// </summary>
		public ExchangeRatesClient()
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };
			_httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
		}

		/// <summary>
		/// Returns the latest available exchange rates for all the valid currencies.
		/// </summary>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<LatestRatesModel> GetLatestRates(Language language = Language.En) =>
			JsonConvert.DeserializeObject<LatestRatesModel>(
				await GetAsync($"{LatestRatesPath}?lang={language}"));

		/// <summary>
		/// Returns the daily exchange rates for a specific date.
		/// </summary>
		/// <param name="referenceDate">The reference date for the exchange rates.</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, string currencyIsoCode,
			Language language = Language.En) =>
			await GetDailyRates(referenceDate, new List<string>(), currencyIsoCode, language);

		/// <summary>
		/// Returns the daily exchange rates for a specific date.
		/// </summary>
		/// <param name="referenceDate">The reference date for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCodes">The list of ISO codes of the required currencies (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<DailyRatesModel> GetDailyRates(DateTime referenceDate,
			IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En)
		{
			if (baseCurrencyIsoCodes is null) throw new ArgumentNullException(nameof(baseCurrencyIsoCodes));
			var baseCurrencyIsoCodeList = baseCurrencyIsoCodes.ToList();
			return JsonConvert.DeserializeObject<DailyRatesModel>(
				await GetAsync(new StringBuilder(DailyRatesPath)
					.Append("?referenceDate=").Append(referenceDate.ToString("yyyy-MM-dd"))
					.Append(baseCurrencyIsoCodeList.Count > 0 ? "&" : string.Empty)
					.AppendJoin("&", baseCurrencyIsoCodeList.Select(isoCode => $"baseCurrencyIsoCode={isoCode}"))
					.Append("&currencyIsoCode=").Append(currencyIsoCode)
					.Append("&lang=").Append(language)
					.ToString()));
		}

		/// <summary>
		/// Returns the monthly average exchange rates for specific month and year.
		/// </summary>
		/// <param name="month">The reference month for the exchange rates (1-12).</param>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year, string currencyIsoCode,
			Language language = Language.En) =>
			await GetMonthlyAverageRates(month, year, new List<string>(), currencyIsoCode, language);

		/// <summary>
		/// Returns the monthly average exchange rates for specific month and year.
		/// </summary>
		/// <param name="month">The reference month for the exchange rates (1-12).</param>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCodes">The list of ISO codes of the required currencies (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year,
			IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En)
		{
			if (baseCurrencyIsoCodes is null) throw new ArgumentNullException(nameof(baseCurrencyIsoCodes));
			var baseCurrencyIsoCodeList = baseCurrencyIsoCodes.ToList();
			return JsonConvert.DeserializeObject<MonthlyAverageRatesModel>(
				await GetAsync(new StringBuilder(MonthlyAverageRatesPath)
					.Append("?month=").Append(month)
					.Append("&year=").Append(year)
					.Append(baseCurrencyIsoCodeList.Count > 0 ? "&" : string.Empty)
					.AppendJoin("&", baseCurrencyIsoCodeList.Select(isoCode => $"baseCurrencyIsoCode={isoCode}"))
					.Append("&currencyIsoCode=").Append(currencyIsoCode)
					.Append("&lang=").Append(language)
					.ToString()));
		}

		/// <summary>
		/// Returns the annual average exchange rates for a specific year.
		/// </summary>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year, string currencyIsoCode,
			Language language = Language.En) =>
			await GetAnnualAverageRates(year, new List<string>(), currencyIsoCode, language);

		/// <summary>
		/// Returns the annual average exchange rates for a specific year.
		/// </summary>
		/// <param name="year">The reference year for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCodes">The list of ISO codes of the required currencies (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year,
			IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En)

		{
			if (baseCurrencyIsoCodes is null) throw new ArgumentNullException(nameof(baseCurrencyIsoCodes));
			var baseCurrencyIsoCodeList = baseCurrencyIsoCodes.ToList();
			return JsonConvert.DeserializeObject<AnnualAverageRatesModel>(
				await GetAsync(new StringBuilder(AnnualAverageRatesPath)
					.Append("?year=").Append(year)
					.Append(baseCurrencyIsoCodeList.Count > 0 ? "&" : string.Empty)
					.AppendJoin("&", baseCurrencyIsoCodeList.Select(isoCode => $"baseCurrencyIsoCode={isoCode}"))
					.Append("&currencyIsoCode=").Append(currencyIsoCode)
					.Append("&lang=").Append(language)
					.ToString()));
		}

		/// <summary>
		/// Returns the daily exchange rates of a currency for a specific date range.
		/// </summary>
		/// <param name="startDate">The start date of the range for the exchange rates.</param>
		/// <param name="endDate">The end date of the range for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCode">The ISO code of the required currency (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<DailyTimeSeriesModel> GetDailyTimeSeries(DateTime startDate, DateTime endDate,
			string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En) =>
			JsonConvert.DeserializeObject<DailyTimeSeriesModel>(
				await GetAsync(new StringBuilder(DailyTimeSeriesPath)
					.Append("?startDate=").Append(startDate.ToString("yyyy-MM-dd"))
					.Append("&endDate=").Append(endDate.ToString("yyyy-MM-dd"))
					.Append("&baseCurrencyIsoCode=").Append(baseCurrencyIsoCode)
					.Append("&currencyIsoCode=").Append(currencyIsoCode)
					.Append("&lang=").Append(language)
					.ToString()));

		/// <summary>
		/// Returns the monthly average exchange rates of a currency for a specific month range.
		/// </summary>
		/// <param name="startMonth">The start month of the range for the exchange rates (1-12).</param>
		/// <param name="startYear">The start year of the range for the exchange rates.</param>
		/// <param name="endMonth">The end month of the range for the exchange rates (1-12).</param>
		/// <param name="endYear">The end year of the range for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCode">The ISO code of the required currency (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<MonthlyTimeSeriesModel> GetMonthlyTimeSeries(int startMonth, int startYear, int endMonth, int endYear,
			string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En) =>
			JsonConvert.DeserializeObject<MonthlyTimeSeriesModel>(
				await GetAsync(new StringBuilder(MonthlyTimeSeriesPath)
					.Append("?startMonth=").Append(startMonth)
					.Append("&startYear=").Append(startYear)
					.Append("&endMonth=").Append(endMonth)
					.Append("&endYear=").Append(endYear)
					.Append("&baseCurrencyIsoCode=").Append(baseCurrencyIsoCode)
					.Append("&currencyIsoCode=").Append(currencyIsoCode)
					.Append("&lang=").Append(language)
					.ToString()));

		/// <summary>
		/// Returns the annual average exchange rates of a currency for a specific year range.
		/// </summary>
		/// <param name="startYear">The start year of the range for the exchange rates.</param>
		/// <param name="endYear">The end year of the range for the exchange rates.</param>
		/// <param name="baseCurrencyIsoCode">The ISO code of the required currency (case insensitive).</param>
		/// <param name="currencyIsoCode">The ISO code of the reference currency ("EUR" or "USD", case insensitive).</param>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<AnnualTimeSeriesModel> GetAnnualTimeSeries(int startYear, int endYear,
			string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En) =>
			JsonConvert.DeserializeObject<AnnualTimeSeriesModel>(
				await GetAsync(new StringBuilder(AnnualTimeSeriesPath)
					.Append("?startYear=").Append(startYear)
					.Append("&endYear=").Append(endYear)
					.Append("&baseCurrencyIsoCode=").Append(baseCurrencyIsoCode)
					.Append("&currencyIsoCode=").Append(currencyIsoCode)
					.Append("&lang=").Append(language)
					.ToString()));

		/// <summary>
		/// Returns the list of all the available currencies.
		/// </summary>
		/// <param name="language">The language of the returned data.</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<CurrenciesModel> GetCurrencies(Language language = Language.En) =>
			JsonConvert.DeserializeObject<CurrenciesModel>(
				await GetAsync($"{CurrenciesPath}?lang={language}"));

		private async Task<string> GetAsync(string requestUri)
		{
			var response = await _httpClient.GetAsync(requestUri);
			var content = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException(
					$"The server responded with an error. Status code: {(int)response.StatusCode}; Message: {content}");
			return content;
		}
	}
}
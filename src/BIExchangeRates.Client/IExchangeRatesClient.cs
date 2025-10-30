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

using BIExchangeRates.Client.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BIExchangeRates.Client;

/// <summary>
/// Defines a wrapper for the REST API of the currency exchange rates of Banca d'Italia (https://tassidicambio.bancaditalia.it).
/// </summary>
public interface IExchangeRatesClient
{
	/// <summary>
	/// Returns the latest available exchange rates for all the valid currencies.
	/// </summary>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the latest available exchange rates for all the valid currencies.</returns>
	Task<LatestRatesModel> GetLatestRates(Language language = Language.En);

	/// <summary>
	/// Returns the daily exchange rates for a specific date for all the available currencies.
	/// </summary>
	/// <param name="referenceDate">The reference date for the exchange rates.</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for all the available currencies.</returns>
	Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the daily exchange rates for a specific date for a list of currencies.
	/// </summary>
	/// <param name="referenceDate">The reference date for the exchange rates.</param>
	/// <param name="baseCurrencyIsoCodes">The list of ISO 4217 codes of the required currencies (case insensitive).</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for a list of currencies.</returns>
	Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, IEnumerable<string> baseCurrencyIsoCodes,
		string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the monthly average exchange rates for specific month and year for all the available currencies.
	/// </summary>
	/// <param name="month">The reference month for the exchange rates (1-12).</param>
	/// <param name="year">The reference year for the exchange rates.</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the monthly average exchange rates for specific month and year for all the available currencies.</returns>
	Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year, string currencyIsoCode,
		Language language = Language.En);

	/// <summary>
	/// Returns the monthly average exchange rates for specific month and year for a list of currencies.
	/// </summary>
	/// <param name="month">The reference month for the exchange rates (1-12).</param>
	/// <param name="year">The reference year for the exchange rates.</param>
	/// <param name="baseCurrencyIsoCodes">The list of ISO 4217 codes of the required currencies (case insensitive).</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the monthly average exchange rates for specific month and year for a list of currencies.</returns>
	Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year, IEnumerable<string> baseCurrencyIsoCodes,
		string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the annual average exchange rates for a specific year for all the available currencies.
	/// </summary>
	/// <param name="year">The reference year for the exchange rates.</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the annual average exchange rates for a specific year for all the available currencies.</returns>
	Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year, string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the annual average exchange rates for a specific year for a list of currencies.
	/// </summary>
	/// <param name="year">The reference year for the exchange rates.</param>
	/// <param name="baseCurrencyIsoCodes">The list of ISO 4217 codes of the required currencies (case insensitive).</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the annual average exchange rates for a specific year for a list of currencies.</returns>
	Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year, IEnumerable<string> baseCurrencyIsoCodes,
		string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the daily exchange rates of a currency for a specific date range.
	/// </summary>
	/// <param name="startDate">The start date of the range for the exchange rates.</param>
	/// <param name="endDate">The end date of the range for the exchange rates.</param>
	/// <param name="baseCurrencyIsoCode">The ISO 4217 code of the required currency (case insensitive).</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the daily exchange rates of a currency for a specific date range.</returns>
	Task<DailyTimeSeriesModel> GetDailyTimeSeries(DateTime startDate, DateTime endDate, string baseCurrencyIsoCode,
		string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the monthly average exchange rates of a currency for a specific month range.
	/// </summary>
	/// <param name="startMonth">The start month of the range for the exchange rates (1-12).</param>
	/// <param name="startYear">The start year of the range for the exchange rates.</param>
	/// <param name="endMonth">The end month of the range for the exchange rates (1-12).</param>
	/// <param name="endYear">The end year of the range for the exchange rates.</param>
	/// <param name="baseCurrencyIsoCode">The ISO 4217 code of the required currency (case insensitive).</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the monthly average exchange rates of a currency for a specific month range.</returns>
	Task<MonthlyTimeSeriesModel> GetMonthlyTimeSeries(int startMonth, int startYear, int endMonth, int endYear,
		string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the annual average exchange rates of a currency for a specific year range.
	/// </summary>
	/// <param name="startYear">The start year of the range for the exchange rates.</param>
	/// <param name="endYear">The end year of the range for the exchange rates.</param>
	/// <param name="baseCurrencyIsoCode">The ISO 4217 code of the required currency (case insensitive).</param>
	/// <param name="currencyIsoCode">The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive).</param>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the annual average exchange rates of a currency for a specific year range.</returns>
	Task<AnnualTimeSeriesModel> GetAnnualTimeSeries(int startYear, int endYear, string baseCurrencyIsoCode,
		string currencyIsoCode, Language language = Language.En);

	/// <summary>
	/// Returns the list of all the available currencies.
	/// </summary>
	/// <param name="language">The language of the returned data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all the available currencies.</returns>
	Task<CurrenciesModel> GetCurrencies(Language language = Language.En);
}

/// <summary>
/// Specifies the language of the results.
/// </summary>
public enum Language
{
	/// <summary>
	/// English language.
	/// </summary>
	En,

	/// <summary>
	/// Italian language.
	/// </summary>
	It,
}

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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BIExchangeRates.Client
{
	public enum Language { En, It }

	public interface IExchangeRatesClient
	{
		Task<LatestRatesModel> GetLatestRates(Language language);

		Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, string currencyIsoCode,
			Language language);

		Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, IEnumerable<string> baseCurrencyIsoCodes,
			string currencyIsoCode, Language language);

		Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year, string currencyIsoCode,
			Language language);

		Task<MonthlyAverageRatesModel> GetMonthlyAverageRates(int month, int year,
			IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language);

		Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year, string currencyIsoCode, Language language);

		Task<AnnualAverageRatesModel> GetAnnualAverageRates(int year, IEnumerable<string> baseCurrencyIsoCodes,
			string currencyIsoCode, Language language);

		Task<DailyTimeSeriesModel> GetDailyTimeSeries(DateTime startDate, DateTime endDate, string baseCurrencyIsoCode,
			string currencyIsoCode, Language language);

		Task<MonthlyTimeSeriesModel> GetMonthlyTimeSeries(int startMonth, int startYear, int endMonth, int endYear,
			string baseCurrencyIsoCode, string currencyIsoCode, Language language);

		Task<AnnualTimeSeriesModel> GetAnnualTimeSeries(int startYear, int endYear, string baseCurrencyIsoCode,
			string currencyIsoCode, Language language);

		Task<CurrenciesModel> GetCurrencies(Language language);
	}
}
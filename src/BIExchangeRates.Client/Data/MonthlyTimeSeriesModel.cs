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
using System.Collections.Generic;

namespace BIExchangeRates.Client.Data;

/// <summary>
/// Contains the monthly average exchange rates of a currency.
/// </summary>
public sealed class MonthlyTimeSeriesModel
{
	/// <summary>
	/// Contains aggregated information about the results.
	/// </summary>
	public sealed class ResultsInfoModel
	{
		/// <summary>
		/// Gets or sets the total amount of results.
		/// </summary>
		/// <returns>The total amount of results.</returns>
		public int TotalRecords { get; set; }

		/// <summary>
		/// Gets or sets the currency name.
		/// </summary>
		/// <returns>The currency name.</returns>
		public string Currency { get; set; }

		/// <summary>
		/// Gets or sets the ISO 4217 code of the currency.
		/// </summary>
		/// <returns>The ISO 4217 code of the currency.</returns>
		public string IsoCode { get; set; }

		/// <summary>
		/// Gets or sets the unique identification code of the currency.
		/// </summary>
		/// <returns>The unique identification code of the currency.</returns>
		public string UicCode { get; set; }

		/// <summary>
		/// Gets or sets the code of the exchange convention against the reference currency.
		/// </summary>
		/// <returns>The code of the exchange convention against the reference currency.</returns>
		public string ExchangeConventionCode { get; set; }
	}

	/// <summary>
	/// Gets or sets aggregated information about the results.
	/// </summary>
	/// <returns>The aggregated information about the results.</returns>
	public ResultsInfoModel ResultsInfo { get; set; }

	/// <summary>
	/// Contains information about the monthly average exchange rate of a currency.
	/// </summary>
	public sealed class ExchangeRateModel
	{
		/// <summary>
		/// Gets or sets the reference date.
		/// </summary>
		/// <returns>The reference date.</returns>
		public DateTime ReferenceDate { get; set; }

		/// <summary>
		/// Gets or sets the monthly average exchange rate.
		/// </summary>
		/// <returns>The monthly average exchange rate.</returns>
		public double AvgRate { get; set; }

		/// <summary>
		/// Gets or sets the exchange convention against the reference currency.
		/// </summary>
		/// <returns>The exchange convention against the reference currency.</returns>
		public string ExchangeConvention { get; set; }
	}

	/// <summary>
	/// Gets or sets the exchange rates.
	/// </summary>
	/// <returns>The exchange rates.</returns>
	public IEnumerable<ExchangeRateModel> Rates { get; set; }
}

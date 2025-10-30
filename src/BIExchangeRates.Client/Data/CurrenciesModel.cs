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
/// Contains the list of all the available currencies.
/// </summary>
public sealed class CurrenciesModel
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
		/// Gets or sets the time zone of the results.
		/// </summary>
		/// <returns>The time zone of the results.</returns>
		public string TimezoneReference { get; set; }
	}

	/// <summary>
	/// Gets or sets aggregated information about the results.
	/// </summary>
	/// <returns>The aggregated information about the results.</returns>
	public ResultsInfoModel ResultsInfo { get; set; }

	/// <summary>
	/// Contains information about a currency.
	/// </summary>
	public sealed class CurrencyModel
	{
		/// <summary>
		/// Contains information about the adoption of a currency in a country.
		/// </summary>
		public sealed class CountryModel
		{
			/// <summary>
			/// Gets or sets the ISO 4217 code of the currency.
			/// </summary>
			/// <returns>The ISO 4217 code of the currency.</returns>
			public string CurrencyIso { get; set; }

			/// <summary>
			/// Gets or sets the country name.
			/// </summary>
			/// <returns>The country name.</returns>
			public string Country { get; set; }

			/// <summary>
			/// Gets or sets the ISO 3166-1 alpha-3 code of the country.
			/// </summary>
			/// <returns>The ISO 3166-1 alpha-3 code of the country.</returns>
			public string CountryIso { get; set; }

			/// <summary>
			/// Gets or sets the validity start date.
			/// </summary>
			/// <returns>The validity start date.</returns>
			public DateTime ValidityStartDate { get; set; }

			/// <summary>
			/// Gets or sets the validity end date.
			/// </summary>
			/// <returns>The validity end date.</returns>
			public DateTime? ValidityEndDate { get; set; }
		}

		/// <summary>
		/// Gets or sets the list of countries that adopt the currency.
		/// </summary>
		/// <returns>The list of countries that adopt the currency.</returns>
		public IEnumerable<CountryModel> Countries { get; set; }

		/// <summary>
		/// Gets or sets the ISO 4217 code of the currency.
		/// </summary>
		/// <returns>The ISO 4217 code of the currency.</returns>
		public string IsoCode { get; set; }

		/// <summary>
		/// Gets or sets the currency name.
		/// </summary>
		/// <returns>The currency name.</returns>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Graph { get; set; }
	}

	/// <summary>
	/// Gets or sets the list of currencies.
	/// </summary>
	/// <returns>The list of currencies.</returns>
	public IEnumerable<CurrencyModel> Currencies { get; set; }
}

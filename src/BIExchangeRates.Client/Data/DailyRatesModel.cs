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

using System;
using System.Collections.Generic;

namespace BIExchangeRates.Client.Data
{
	public sealed class DailyRatesModel
	{
		public sealed class ResultsInfoModel
		{
			public int TotalRecords { get; set; }

			public string TimezoneReference { get; set; }
		}

		public ResultsInfoModel ResultsInfo { get; set; }

		public sealed class ExchangeRateModel
		{
			public string Country { get; set; }

			public string Currency { get; set; }

			public string IsoCode { get; set; }

			public string UicCode { get; set; }

			public double AvgRate { get; set; }

			public string ExchangeConvention { get; set; }

			public string ExchangeConventionCode { get; set; }

			public DateTime ReferenceDate { get; set; }
		}

		public IEnumerable<ExchangeRateModel> Rates { get; set; }
	}
}
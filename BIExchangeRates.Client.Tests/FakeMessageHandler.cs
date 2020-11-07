﻿// Copyright (c) 2020 Maurizio Basaglia
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
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIExchangeRates.Client.Tests
{
    class FakeMessageHandler : HttpMessageHandler
    {
        private static readonly Uri BaseAddress = new Uri("https://tassidicambio.bancaditalia.it/terzevalute-wf-web/rest/v1.0/");

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
                return await Task.FromResult(GetLatestRates(queryParameters));

            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent($"Requested resource non found ({request.Method} {request.RequestUri}).")
            });
        }

        private HttpResponseMessage GetLatestRates(NameValueCollection queryParameters)
        {
            if (!queryParameters.TryGetEnum<Language>("lang", false, out var lang, out var response)) return response;

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

            response = new HttpResponseMessage()
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
            return response;
        }
    }
}

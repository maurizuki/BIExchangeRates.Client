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

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace BIExchangeRates.Client.Tests
{
    static class NameValueCollectionExtensions
    {
        public static bool TryGetInt(this NameValueCollection queryParameters, string parameter, bool isRequired,
            out int value, out HttpResponseMessage response)
        {
            value = default;

            if (!queryParameters.TryGetString(parameter, isRequired, out var stringValue, out response)) return false;

            if (!int.TryParse(stringValue, out value))
            {
                response = InvalidParameter(parameter, stringValue);
                return false;
            }

            return true;
        }

        public static bool TryGetEnum<T>(this NameValueCollection queryParameters, string parameter, bool isRequired,
            out T value, out HttpResponseMessage response) where T : struct
        {
            value = default;

            if (!queryParameters.TryGetString(parameter, isRequired, out var stringValue, out response)) return false;

            if (!Enum.TryParse(stringValue, out value))
            {
                response = InvalidParameter(parameter, stringValue);
                return false;
            }

            return true;
        }

        public static bool TryGetDateTime(this NameValueCollection queryParameters, string parameter, bool isRequired,
            string format, IFormatProvider provider, DateTimeStyles style,
            out DateTime value, out HttpResponseMessage response)
        {
            value = default;

            if (!queryParameters.TryGetString(parameter, isRequired, out var stringValue, out response)) return false;

            if (!DateTime.TryParseExact(stringValue, format, provider, style, out value))
            {
                response = InvalidParameter(parameter, stringValue);
                return false;
            }

            return true;
        }

        public static bool TryGetString(this NameValueCollection queryParameters, string parameter, bool isRequired,
            out string value, out HttpResponseMessage response)
        {
            value = default;
            response = null;

            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired)
                {
                    response = MissingParameter(parameter);
                    return false;
                }

                return true;
            }

            value = queryParameters[parameter];

            return true;
        }

        public static bool TryGetStrings(this NameValueCollection queryParameters, string parameter, bool isRequired,
            out string[] value, out HttpResponseMessage response)
        {
            value = default;
            response = null;

            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired)
                {
                    response = MissingParameter(parameter);
                    return false;
                }

                return true;
            }

            value = queryParameters.GetValues(parameter) ?? default;

            return true;
        }

        private static HttpResponseMessage MissingParameter(string parameter) =>
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent($"Required parameter {parameter} not found.")
            };

        private static HttpResponseMessage InvalidParameter(string parameter, string value) =>
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent($"Parameter {parameter} has an invalid value ({value}).")
            };
    }
}

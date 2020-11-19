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
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace BIExchangeRates.Client.Tests
{
    static class NameValueCollectionExtensions
    {
        public static int GetInt(this NameValueCollection queryParameters, string parameter, bool isRequired)
        {
            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired) MissingParameter(parameter);
                return default;
            }

            if (!int.TryParse(queryParameters[parameter], out var value))
                InvalidParameter(parameter, queryParameters[parameter]);

            return value;
        }

        public static T GetEnum<T>(this NameValueCollection queryParameters, string parameter, bool isRequired)
            where T : struct
        {
            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired) MissingParameter(parameter);
                return default;
            }

            if (!Enum.TryParse(queryParameters[parameter], out T value))
                InvalidParameter(parameter, queryParameters[parameter]);

            return value;
        }

        public static DateTime GetDateTime(this NameValueCollection queryParameters, string parameter, bool isRequired,
            string format, IFormatProvider provider, DateTimeStyles style)
        {
            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired) MissingParameter(parameter);
                return default;
            }

            if (!DateTime.TryParseExact(queryParameters[parameter], format, provider, style, out var value))
                InvalidParameter(parameter, queryParameters[parameter]);

            return value;
        }

        public static string GetString(this NameValueCollection queryParameters, string parameter, bool isRequired)
        {
            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired) MissingParameter(parameter);
                return default;
            }

            return queryParameters[parameter];
        }

        public static string[] GetStrings(this NameValueCollection queryParameters, string parameter, bool isRequired)
        {
            if (!queryParameters.AllKeys.Contains(parameter))
            {
                if (isRequired) MissingParameter(parameter);
                return default;
            }

            return queryParameters.GetValues(parameter) ?? default;
        }

        private static void MissingParameter(string parameter) =>
            throw new Exception($"Required parameter {parameter} not found.");

        private static void InvalidParameter(string parameter, string value) =>
            throw new Exception($"Parameter {parameter} has an invalid value ({value}).");
    }
}

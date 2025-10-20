# BIExchangeRates.Client

[![language](https://img.shields.io/github/languages/top/maurizuki/BIExchangeRates.Client)](https://github.com/maurizuki/BIExchangeRates.Client)
[![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/maurizuki/BIExchangeRates.Client/build.yml)](https://github.com/maurizuki/BIExchangeRates.Client/actions/workflows/build.yml)
[![issues](https://img.shields.io/github/issues/maurizuki/BIExchangeRates.Client)](https://github.com/maurizuki/BIExchangeRates.Client/issues)
[![Nuget](https://img.shields.io/nuget/v/BIExchangeRates.Client)](https://www.nuget.org/packages/BIExchangeRates.Client)
[![Nuget](https://img.shields.io/nuget/dt/BIExchangeRates.Client)](https://www.nuget.org/packages/BIExchangeRates.Client)

A .NET wrapper for the REST API of the currency exchange rates of [Banca d'Italia](https://tassidicambio.bancaditalia.it) (the central bank of Italy).

## Getting started

To add BIExchangeRates.Client to your project, you can use the following NuGet Package Manager command:

```PowerShell
Install-Package BIExchangeRates.Client
```

More options are available on the [BIExchangeRates.Client page](https://www.nuget.org/packages/BIExchangeRates.Client) of the NuGet Gallery website.

The console application [BIExchangeRates.Console](./src/BIExchangeRates.Console) is intended as an example on how to use the REST API wrapper in a real scenario.

## Remarks

The class ExchangeRatesClient is derived from [HttpClient](https://docs.microsoft.com/dotnet/api/system.net.http.httpclient) that is intended to be instantiated once and re-used throughout the life of an application. Instantiating an ExchangeRatesClient class for every request will exhaust the number of sockets available under heavy loads. This will result in SocketException errors. Below is an example using ExchangeRatesClient correctly.

```C#
public class GoodController : ApiController
{
    private static readonly ExchangeRatesClient ExchangeRatesClient;

    static GoodController()
    {
        ExchangeRatesClient = new ExchangeRatesClient();
    }
}
```

## Documentation

* [BIExchangeRates.Client 1.0 API Reference](https://github.com/maurizuki/BIExchangeRates.Client/wiki/BIExchangeRates.Client-1.0)
* [Official REST API documentation](https://tassidicambio.bancaditalia.it/terzevalute-wf-ui-web/assets/files/Operating_Instructions.pdf)

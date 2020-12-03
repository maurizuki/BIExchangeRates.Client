# BIExchangeRates.Client

[![Nuget](https://img.shields.io/nuget/v/BIExchangeRates.Client)](https://www.nuget.org/packages/BIExchangeRates.Client)
[![Nuget](https://img.shields.io/nuget/dt/BIExchangeRates.Client)](https://www.nuget.org/packages/BIExchangeRates.Client)

A .NET wrapper for the REST API of the currency exchange rates of [Banca d'Italia](https://tassidicambio.bancaditalia.it) (the central bank of Italy).

## Getting started

To start using BIExchangeRates.Client in your project, install the package with the NuGet Package Manager:

```PowerShell
PM> Install-Package BIExchangeRates.Client
```

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

The console application BIExchangeRates.Console is intended as an example on how to use the REST API wrapper in a real scenario.

## Documentation

* [BIExchangeRates.Client 1.0 Reference](https://github.com/maurizuki/BIExchangeRates.Client/wiki/BIExchangeRates.Client-1.0)
* [REST API documentation](https://tassidicambio.bancaditalia.it/assets/files/Operating_Instructions.pdf)

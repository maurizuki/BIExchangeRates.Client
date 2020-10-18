# BIExchangeRates.Client

[![Nuget](https://img.shields.io/nuget/v/BIExchangeRates.Client)](https://www.nuget.org/packages/BIExchangeRates.Client)

A .NET wrapper for the REST API of the currency exchange rates of [Banca d'Italia](https://tassidicambio.bancaditalia.it) (the central bank of Italy).

## ExchangeRatesClient Class

Namespace: BIExchangeRates.Client

```c#
public sealed class ExchangeRatesClient : IExchangeRatesClient
```

### Constructors

Initializes a new instance of the ExchangeRatesClient class.

```c#
public ExchangeRatesClient ();
```
### Methods

#### GetLatestRates Method

Returns the latest available exchange rates for all the valid currencies.

```c#
public async Task<LatestRatesModel> GetLatestRates (Language language = Language.En);
```

##### Parameters

**language** Language  
The language of the returned data.

#### GetDailyRates Method

Returns the daily exchange rates for a specific date.

```c#
public async Task<DailyRatesModel> GetDailyRates (DateTime referenceDate, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**referenceDate** DateTime  
The reference date for the exchange rates.  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

```c#
public async Task<DailyRatesModel> GetDailyRates (DateTime referenceDate, IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**referenceDate** DateTime  
The reference date for the exchange rates.  
**baseCurrencyIsoCodes** IEnumerable&lt;string&gt;  
The list of ISO codes of the required currencies (case insensitive).  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

#### GetMonthlyAverageRates Method

Returns the monthly average exchange rates for specific month and year.

```c#
public async Task<MonthlyAverageRatesModel> GetMonthlyAverageRates (int month, int year, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**month** int  
The reference month for the exchange rates (1-12).  
**year** int  
The reference year for the exchange rates.  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

```c#
public async Task<MonthlyAverageRatesModel> GetMonthlyAverageRates (int month, int year, IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**month** int  
The reference month for the exchange rates (1-12).  
**year** int  
The reference year for the exchange rates.  
**baseCurrencyIsoCodes** IEnumerable&lt;string&gt;  
The list of ISO codes of the required currencies (case insensitive).  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

#### GetAnnualAverageRates Method

Returns the annual average exchange rates for a specific year.

```c#
public async Task<AnnualAverageRatesModel> GetAnnualAverageRates (int year, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**year** int  
The reference year for the exchange rates.  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

```c#
public async Task<AnnualAverageRatesModel> GetAnnualAverageRates (int year, IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**year** int  
The reference year for the exchange rates.  
**baseCurrencyIsoCodes** IEnumerable&lt;string&gt;  
The list of ISO codes of the required currencies (case insensitive).  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

#### GetDailyTimeSeries Method

Returns the daily exchange rates of a currency for a specific date range.

```c#
public async Task<DailyTimeSeriesModel> GetDailyTimeSeries (DateTime startDate, DateTime endDate, string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**startDate** DateTime  
The start date of the range for the exchange rates.  
**endDate** DateTime  
The end date of the range for the exchange rates.  
**baseCurrencyIsoCode** string  
The ISO code of the required currency (case insensitive).  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

#### GetMonthlyTimeSeries Method

Returns the monthly average exchange rates of a currency for a specific month range.

```c#
public async Task<MonthlyTimeSeriesModel> GetMonthlyTimeSeries (int startMonth, int startYear, int endMonth, int endYear, string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**startMonth** int  
The start month of the range for the exchange rates (1-12).  
**startYear** int  
The start year of the range for the exchange rates.  
**endMonth** int  
The end month of the range for the exchange rates (1-12).  
**endYear** int  
The end year of the range for the exchange rates.  
**baseCurrencyIsoCode** string  
The ISO code of the required currency (case insensitive).  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

#### GetAnnualTimeSeries Method

Returns the annual average exchange rates of a currency for a specific year range.

```c#
public async Task<AnnualTimeSeriesModel> GetAnnualTimeSeries (int startYear, int endYear, string baseCurrencyIsoCode, string currencyIsoCode, Language language = Language.En);
```

##### Parameters

**startYear** int  
The start year of the range for the exchange rates.  
**endYear** int  
The end year of the range for the exchange rates.  
**baseCurrencyIsoCode** string  
The ISO code of the required currency (case insensitive).  
**currencyIsoCode** string  
The ISO code of the reference currency ("EUR", "USD" or "ITL", case insensitive).  
**language** Language  
The language of the returned data.  

#### GetCurrencies Method

Returns the list of all the available currencies.

```c#
public async Task<CurrenciesModel> GetCurrencies (Language language = Language.En);
```

##### Parameters

**language** Language  
The language of the returned data.

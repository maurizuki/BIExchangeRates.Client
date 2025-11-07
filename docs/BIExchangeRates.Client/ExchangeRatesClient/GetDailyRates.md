# ExchangeRatesClient.GetDailyRates method (1 of 4)

Returns the daily exchange rates for a specific date for all the available currencies.

```csharp
public Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, string currencyIsoCode, 
    Language language = Language.En)
```

| parameter | description |
| --- | --- |
| referenceDate | The reference date for the exchange rates. |
| currencyIsoCode | The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive). |
| language | The language of the returned data. |

## Return Value

A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for all the available currencies.

## Exceptions

| exception | condition |
| --- | --- |
| HttpRequestException | The response status code does not indicate success. |

## See Also

* class [DailyRatesModel](../../BIExchangeRates.Client.Data/DailyRatesModel.md)
* enum [Language](../Language.md)
* class [ExchangeRatesClient](../ExchangeRatesClient.md)
* namespace [BIExchangeRates.Client](../../BIExchangeRates.Client.md)

---

# ExchangeRatesClient.GetDailyRates method (2 of 4)

Returns the daily exchange rates for a specific date for all the available currencies.

```csharp
public Task<DailyRatesModel> GetDailyRates(CancellationToken cancellationToken, 
    DateTime referenceDate, string currencyIsoCode, Language language = Language.En)
```

| parameter | description |
| --- | --- |
| cancellationToken | A cancellation token that can be used to receive notice of cancellation. |
| referenceDate | The reference date for the exchange rates. |
| currencyIsoCode | The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive). |
| language | The language of the returned data. |

## Return Value

A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for all the available currencies.

## Exceptions

| exception | condition |
| --- | --- |
| HttpRequestException | The response status code does not indicate success. |
| OperationCanceledException | The cancellation token was canceled. |

## See Also

* class [DailyRatesModel](../../BIExchangeRates.Client.Data/DailyRatesModel.md)
* enum [Language](../Language.md)
* class [ExchangeRatesClient](../ExchangeRatesClient.md)
* namespace [BIExchangeRates.Client](../../BIExchangeRates.Client.md)

---

# ExchangeRatesClient.GetDailyRates method (3 of 4)

Returns the daily exchange rates for a specific date for a list of currencies.

```csharp
public Task<DailyRatesModel> GetDailyRates(DateTime referenceDate, 
    IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, 
    Language language = Language.En)
```

| parameter | description |
| --- | --- |
| referenceDate | The reference date for the exchange rates. |
| baseCurrencyIsoCodes | The list of ISO 4217 codes of the required currencies (case insensitive). |
| currencyIsoCode | The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive). |
| language | The language of the returned data. |

## Return Value

A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for a list of currencies.

## Exceptions

| exception | condition |
| --- | --- |
| ArgumentNullException | The parameter *baseCurrencyIsoCodes* is `null`. |
| HttpRequestException | The response status code does not indicate success. |

## See Also

* class [DailyRatesModel](../../BIExchangeRates.Client.Data/DailyRatesModel.md)
* enum [Language](../Language.md)
* class [ExchangeRatesClient](../ExchangeRatesClient.md)
* namespace [BIExchangeRates.Client](../../BIExchangeRates.Client.md)

---

# ExchangeRatesClient.GetDailyRates method (4 of 4)

Returns the daily exchange rates for a specific date for a list of currencies.

```csharp
public Task<DailyRatesModel> GetDailyRates(CancellationToken cancellationToken, 
    DateTime referenceDate, IEnumerable<string> baseCurrencyIsoCodes, string currencyIsoCode, 
    Language language = Language.En)
```

| parameter | description |
| --- | --- |
| cancellationToken | A cancellation token that can be used to receive notice of cancellation. |
| referenceDate | The reference date for the exchange rates. |
| baseCurrencyIsoCodes | The list of ISO 4217 codes of the required currencies (case insensitive). |
| currencyIsoCode | The ISO 4217 code of the reference currency ("EUR", "USD" or "ITL", case insensitive). |
| language | The language of the returned data. |

## Return Value

A task that represents the asynchronous operation. The task result contains the daily exchange rates for a specific date for a list of currencies.

## Exceptions

| exception | condition |
| --- | --- |
| ArgumentNullException | The parameter *baseCurrencyIsoCodes* is `null`. |
| HttpRequestException | The response status code does not indicate success. |
| OperationCanceledException | The cancellation token was canceled. |

## See Also

* class [DailyRatesModel](../../BIExchangeRates.Client.Data/DailyRatesModel.md)
* enum [Language](../Language.md)
* class [ExchangeRatesClient](../ExchangeRatesClient.md)
* namespace [BIExchangeRates.Client](../../BIExchangeRates.Client.md)

<!-- DO NOT EDIT: generated by xmldocmd for BIExchangeRates.Client.dll -->

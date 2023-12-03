### Create trade command

```json
{
  "tickerSymbol": "aapl",
  "type": "buy | sell",
  "price": 12,
  "numberOfShares": 1,
  "brokerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"

}
```

## Notes
- Stock prices for a given ticker symbol are calculated simplistically by dividing the total stock value by the total number of stocks.

## Assumptions
- Trades posted to the API are assumed to be pre-validated for overall integrity, including confirmed validity of price and quantity.


## Nice to have
- Auth
- Logging
- Paginate response

## Improvements
- When getting stocks and no snapshot found, rehydrate from trade events. 


The rehydration of stock values from trade events is not required in the current implementation, as both the writing of trades and the creation of stock snapshots occur within the same request. Therefore, there is no need for additional rehydration when retrieving stock information.
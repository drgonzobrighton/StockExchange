# Stock Exchange API

Description:
This Stock Exchange API serves as a Minimum Viable Product (MVP) allowing users to manage trades and retrieve stock prices. The API is designed to facilitate the addition of trades and the retrieval of stock prices for specified ticker symbols.

## Endpoints

### Add Trades:

- Method: `POST`
- Endpoint: `/api/trades`
- Description: Allows users to add trades to the stock exchange.

#### Create trade command

```json
{
  "tickerSymbol": "AAPL",
  "type": "buy | sell",
  "price": 12,
  "numberOfShares": 1,
  "brokerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "createdAt": "2023-12-03T00:00:00Z"
}
```

### Get Stock Price by Ticker Symbol:

- Method: `GET`
- Endpoint: `/api/stocks/{tickerSymbol}`
- Description: Retrieves the current stock price for the specified ticker symbol.

#### Response

```json
{
  "tickerSymbol": "AAPL",
  "value": 10,
  "timestamp": "2023-12-03T19:52:31.49Z"
}
```

### Get All Stock Prices:

- Method: `GET`
- Endpoint: `/api/stocks`
- Description: Retrieves the current stock prices for all available stocks.

#### Response

```json
[
  {
    "tickerSymbol": "AAPL",
    "value": 10,
    "timestamp": "2023-12-03T19:52:31.49Z"
  },
  {
    "tickerSymbol": "MSFT",
    "value": 10,
    "timestamp": "2023-12-03T19:52:31.49Z"
  }
]
```

### Get Stock Prices in a Range:

- Method: `GET`
- Endpoint: `/api/stocks/range?tickerSymbols=aapl,msft`
- Description: Retrieves the stock prices for the specified ticker symbols.

#### Response

```json
[
  {
    "tickerSymbol": "AAPL",
    "value": 10,
    "timestamp": "2023-12-03T19:52:31.49Z"
  },
  {
    "tickerSymbol": "MSFT",
    "value": 10,
    "timestamp": "2023-12-03T19:52:31.49Z"
  }
]
```

## Notes

- Stock prices for a given ticker symbol are calculated simplistically by dividing the total stock value by the total number of stocks.
- Stock values rely solely on stock snapshots and there is no interaction with the trade event stream.
- Trade submissions and snapshot creation occur concurrently in a single request.

## Assumptions

- Trades posted to the API are assumed to be pre-validated for overall integrity, including confirmed validity of price and quantity.

## Nice to have

- Auth
- Logging
- Paginate response

## Improvements

- When getting stocks and no snapshot found, rehydrate from trade events.

The rehydration of stock values from trade events is not required in the current implementation, as both the writing of trades and the creation of stock snapshots occur within the same request. Therefore, there is no need for additional rehydration when retrieving stock information.

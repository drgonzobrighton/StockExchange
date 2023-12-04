# Stock Exchange API

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

### Trade Event and Stock Snapshot:

- Adding a new trade triggers an event that initiates the generation of a stock snapshot.
- The snapshot is created by applying all trades that have occurred since the previous snapshot. If no previous snapshot is found, all trades are applied to generate the initial snapshot.

### Stock Price Calculation:

- Stock prices for a given ticker symbol are calculated simplistically. The calculation involves dividing the total stock value by the total number of stocks for that particular symbol.

### Concurrency and Event Stream:

- Trade submissions and snapshot creation occur concurrently in a single request.
- Stock values rely solely on stock snapshots, and there is no direct interaction with the trade event stream for individual stock price calculations.

## Assumptions

- Trades posted to the API are assumed to be pre-validated for overall integrity, including confirmed validity of price and quantity.

## Future Improvements

- Add logging
- Add authorisation infrastructure using a well-known methodology like OAuth 2.0
- Implement a caching layer using the built-in .NET Cache object or evaluate the Redis Cache services of Azure to `GET` endpoints. Depending on business requirements, caching can be configured for a fixed period or invalidated dynamically when new `trade`s or `stock snapshot`s are created.
- Add error handling middleware
- Consider moving snapshot creation to it's own service
- Implement a CQRS pipelines to separate the write operations and their scalability from the read operations. This separation enhances scalability by allowing dedicated resources and optimization strategies for each type of operation.



## Nice to have
- Auth
- Logging
- Paginate response

## Improvements
- When getting stocks and no snapshot found, rehydrate from trade events. 
This is not needed in the current implementation as writing `trade` and `stock snapshots` happen in the same request
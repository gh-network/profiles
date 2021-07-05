# GhostNetwork - Profiles

## Installation

copy provided docker-compose.yml and customize for your needs

compile images from the sources - `docker-compose build && docker-compose up -d`

### Parameters

| Environment                    | Description                                               |
|--------------------------------|-----------------------------------------------------------|
| MONGO_CONNECTION               | Connection string to MongoDb instance                     |
| ~~MONGO_ADDRESS~~              | Address of MongoDb instance (OBSOLETE)                    |

## Development

To run dependent environment use

```bash
docker-compose -f dev-compose.yml pull
docker-compose -f dev-compose.yml up --force-recreate
```

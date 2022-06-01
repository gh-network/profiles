# GhostNetwork - Profiles

Content management service (publications, comments and reactions) for [GhostNetwork](https://github.com/ghosts-network) education project

### Parameters

| Environment                    | Description                                                                                         |
|--------------------------------|-----------------------------------------------------------------------------------------------------|
| MONGO_CONNECTION               | Connection string to MongoDb instance                                                               |
| EVENTHUB_TYPE                  | Represent type of service for event bus. Options: rabbit, servicebus. By default all events ignored |
| RABBIT_CONNECTION              | Connection string to rabbitmq. Required for EVENTHUB_TYPE=rabbit                                    |
| SERVICEBUS_CONNECTION          | Connection string to azure service bus. Required for EVENTHUB_TYPE=servicebus                       |

## Development

To run dependent environment use

```bash
docker-compose -f dev-compose.yml pull
docker-compose -f dev-compose.yml up --force-recreate
```

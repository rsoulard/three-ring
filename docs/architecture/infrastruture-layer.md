# Infrastructure Layer
The infrastructure provides concrete implementations of abstractions needed by the application and domain layers. By separating the infrastructure from the application and business logic, the ThreeRing can mix and match different infrastructure components to meet the needs of the client.

# Storage
In order to store uploaded source documents and final PDFs, there needs to be a place to store the actual bits that make up the documents.

## File Storage
- Default storage implementation, providing an "out of the box" experience when no further storage configuration is provided
- Uses the host's file system

## Blob Storage
- Allows for storage of documents across an HTTP surface

## S3 Storage
- Allows for storage of documents across an S3 like surface

# Database
In order to store the current state of Binders, their Documents, and metadata, there needs to be a place to store the records used to orchestrate ThreeRing

## EF Core
- As we only support SQL style RDBMS databases, we only need to support EF Core
- By default, if no connection string to a database is provided, ThreeRing will create and use a SQLite database, providing an "out of the box" experience

# MessageQueue
In order to communicate down to the Worker instance, many Worker instances or any outside context, there needs to be a message queue we can put messages on.

## Unix Domain Sockets
- Default message queue implementation, providing an "out of the box" experience when no further message queue configuration is provided
- Uses Unix's Domain Sockets on a Linux based host

## RabbitMQ
- Allows for messages to be managed by a RabbitMQ exchange

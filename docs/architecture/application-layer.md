# Application Layer
The Application layer coordinates use cases. It does not contain business rules and it does not contain infrastructure details. It orchestrates domain behavior by:
- Loading aggregates
- Invoking domain methods
- Enforcing application level policies
- Interacting with storage, persistence, and messaging through abstractions
- Returning results to the API

# Commands
## CreateBinderCommand
- Orchestrates the creation and persistence of a new Binder

## AddDocumentCommand
- Orchestrates the source document uploading, creation and persistence of a new Document

## RequestCompositionCommand
- Orchestrates the change of state in a Binder and the enqueueing of a message for the job

# Queries
## GetBinderQuery
- Orchestrates the retrieval of a single Binder's data

# Abstractions
## IStorageProvider
- Declares possible interactions with a storage provider

## IMessageQueue
- Declares possible interactions with a message queue

# DTOs
## BinderDto
- Defines what properties are publicly accessible on a Binder

## DocumentDto
- Defines what properties are publicly accessible on a Document

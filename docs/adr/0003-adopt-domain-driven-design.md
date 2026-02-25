# ADR 0003: Adopt Domain-Driven Design

## Status
Accepted

## Context
ThreeRing is a system with non-trivial business rules including aspects like page ordering, expiration policies, abstractions over storage methods, and the lifecycle of a Binder as it moves from creation to completion. These rules must remain consistent across multiple execution environments (API, Worker) and multiple infrastructure choices (databases, storage providers).

We want an architectural approach that:
- Keeps business rules explicit and testable
- Prevents infrastructure concerns from leaking into core logic
- Allows runtimes to share a consistent domain model
- Encourages clear boundaries and intentional language
- Scales as new features and policies are introduced

A layered or CRUD-centric architecture risks scattering business logic across controllers, services and infrastructure, making the system harder to reason about and evolve.

## Decision
We will structure the ThreeRing project using **Domain-Driven Design** principles.

Specifically:
- A Domain layer will contain aggregates, value objects, domain events, and policies
- An Application layer will orchestrate use cases via commands, queries and interfaces
- An Infrastructure layer will implement persistence, storage and messaging
- Runtimes will depend on the Application layer and share the same domain model
- Domain logic will remain pure and free of infrastructure concerns
- Ubiquitous language will be used consistently across code, documentation, and APIs

These traits ensure that ThreeRing's core behavior is explicit, testable, and independent of deployment or storage choices.

## Consequences
### Positive
- Clear boundaries between domain logic, application orchestration, and infrastructure concerns
- Compartmentalization that helps test business logic independently
- Easier to extend to use different infrastructure without modifying domain logic
- Runtimes can share the same models and invariants
- Business rules live in one consistent place
- Reading the domain model can help onboard new developers

### Negatives
- Requires discipline to maintain boundaries
- More overhead than CRUD-oriented design
- Some contributors may be unfamiliar with Domain-Driven Design patterns

### Neutral
- Domain-Driven Design does not dictate infrastructure choices; it only shapes how the core logic is organized
- The architecture can change without rewriting the core domain model

## Alternatives Considered
- ### CRUD-centric model
  Rejected: no consistent home for business logic and harder to maintain consistency across runtimes

## References
- Eric Evans, *"Domain-Driven Design: Tackling Complexity in the Heart of Software"*
- Vaughn Vernon, *"Implementing Domain-Driven Design"*

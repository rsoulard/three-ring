# ADR 0004: Separate API and Worker Runtimes

## Status
Accepted

## Context
The intent of ThreeRing is to automate a process of combining many flat documents into one. While on paper, this seems like a simple task, the act of composing these documents into one is highly technical in nature.

We want an approach to automation that:
- Keeps the technical details away from the orchestration
- Prevents the process of composing the document from hogging resources needed to run the API
- Separates out the infrastructure concerns of orchestration from composing
- Allows the composing to scale to independently from the orchestration

Combining the API and Worker runtimes would make them hard to scale independently and have them constantly fighting for resources.

## Decision
We will keep the Worker runtime Separate from the API runtime.

This helps to:
- Allow the Worker to scale as needed
- Prevents the Worker from affecting the availability of the API
- Keeps infrastructure concerns that only pertain to the Worker out of the API

## Consequences
### Positive
- If more worker instances are needed, they can be scaled out without having to scale out the API
- In single host configurations, the host can determine who to give more resources to

### Negatives
- Requires discipline to maintain boundaries
- Can be confusing for onboarding new developers as it seems to be too disjointed

### Neutral
- Infrastructure is separated on a "need-to-know" basis but the interfaces in the application layer can still be shared 

## Alternatives Considered
- ### Worker as an in-process background service
  Rejected: Possibility of the Worker disrupting the availability of the API and poor scaling options

## References

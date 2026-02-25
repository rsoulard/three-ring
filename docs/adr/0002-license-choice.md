# ADR 0002: License Choice

## Status
Accepted

## Context
The ThreeRing project is intended to be an open, community-driven document composition engine. We want developers and organizations to adopt it freely, integrate it into their systems, and build on top of it. However, we also want to ensure that improvements to the core engine remain open and that no party can create a proprietary fork that diverges from the community version.

A permissive license such as MIT or Apache 2.0 would maximize adoption but would also allow closed-source forks, proprietary enhancements, and commercial derivatives that do not contribute back. This conflicts with the project's long-term goals of openness, shared improvement, and architectural transparency.

A strong copyleft license such as AGPL-3.0 would prevent proprietary forks and SaaS-only derivatives, but it would also significantly reduce adoption, especially in enterprise environments that avoid AGPL due to its broad obligations.

We require a license that:
- Prevents closed-source forks
- Ensures modifications to ThreeRing's source files remain open
- Allows commercial use and integration
- Does not impose full copyleft on entire applications
- Encourages ecosystem growth and contributions

## Decision
We will license the ThreeRing project under the **Mozilla Public License 2.0 (MPL-2.0)**.

MPL-2.0 is a *file-level copyleft* license. It requires that any modifications to MPL-licensed files be published under the same license, while allowing those files to be combined with proprietary code in larger applications.

This strikes the desired balance between openness and adoption.

## Consequences
### Positive
- Prevents close-source forks making any modification to ThreeRing's source files open
- Encourages contributions from the community
- Friendly for commercial applications without forcing them to open source their codebase
- Supports ecosystem growth

### Negatives
- Slightly more complex than MIT in terms of obligations
- Does not prevent commercial hosting or selling of unmodified versions.

### Neutral
- MPL does not require open-sourcing unrelated proprietary code, only modifications to MPL-licensed files.

## Alternatives Considered
- ### MIT License
  Rejected: Allows closed-source forks and proprietary derivatives.

- ### Apache 2.0
  Rejected: Similar to MIT; does not prevent proprietary forks.

- ### LGPL-3.0
  Rejected: Stronger than MPL but introduces linking obligations that may reduce adoption.

- ### AGPL-3.0
  Rejected: Prevents SaaS forks, but significantly limits enterprise adoption and ecosystem growth.

## References
- Mozilla Public License 2.0: https://www.mozilla.org/en-US/MPL/2.0/

# Development Log - Appointment Duration Automation

Basic validation for the duration.

Validating uniqueness by passing all the names and codes (there will be not a lot of them)
Trilemma: https://enterprisecraftsmanship.com/posts/domain-model-purity-completeness/
Completeness and purity, without performance (not many so no problem).
There is a chance for concurency issue, race condition but in the relational dbs it can be also secured on db level. But it is good to secure it in the domain for completeness and better showing error to the user.

## Key Decisions

### 1. Domain Model Trilemma - Completeness & Purity Over Performance
**Decision**: Pass existing names/codes to `Create` method for duplicate validation.
- **Alternatives**: Repository dependency injection into domain or validation in the application layer
- **Why chosen**: Domain stays pure, validates all business rules
- **Trade-off**: Performance acceptable given limited healthcare service types

### 2. Healthcare Service Type Scope
**Decision**: Only implement `AddHealthcareServiceTypeCommand` for now.
- **Why**: Focus on immediate need, expand as system grows
- **Future**: Will add update/delete/archive operations when requirements emerge

### 3. Code as Primary Key Instead of Guid
**Decision**: Use healthcare service code as primary identifier instead of synthetic Guid.
- **Alternatives**: Keep Guid as primary key, use code as secondary unique key
- **Why chosen**:
  - **Business-meaningful**: Medical staff reference services by codes naturally (RCU, BT, XR)
  - **Human-readable**: No need to look up Guids for service identification
  - **Stable identifiers**: Healthcare service codes rarely change in practice
  - **Simpler queries**: Reference services directly by code without Guid lookup
- **Constraint**: Service codes should not change once established (acceptable in healthcare domain)
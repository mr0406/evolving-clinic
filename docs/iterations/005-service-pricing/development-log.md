# Service Pricing - Development Log

## Key Decisions

### 1. Money Value Object
**Decision**: Create a simple Money value object.
- **Implementation**: Only what is needed - decimal value rounded to two decimal places and string formatting
- **Why no currency**: USD-only requirement for small clinic simplifies implementation
- **Benefits**: Type safety, consistent formatting, matches other value object patterns

### 2. Negative Money Values and Price Validation
**Decision**: Allow negative values in Money but validate non-negative prices at usage points.
- **Rationale**: Money in general can be negative (refunds, discounts, debts), but prices must be ≥ 0
- **Implementation**:
  - `Money` value object accepts any decimal value including negative
  - `HealthcareServiceType.Create()` validates price ≥ 0
  - `ScheduledAppointment` constructor validates price ≥ 0
- **Alternative considered**: Creating separate `Price` or `PositiveMoney` value objects
- **Why not now**: Current validation approach is sufficient, let's keep it simple for now
# Service Pricing - Development Log

## Key Decisions

### 1. Money Value Object
**Decision**: Create a simple Money value object.
- **Implementation**: Only what is needed - decimal value rounded to two decimal places and string formatting
- **Why no currency**: USD-only requirement for small clinic simplifies implementation
- **Benefits**: Type safety, consistent formatting, matches other value object patterns
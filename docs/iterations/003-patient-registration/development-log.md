# Development Log - Patient Registration

## Key Decisions

- **Patient identification**: Use name + date of birth + address for phone verification
- **Rich domain**: Implement value objects to avoid primitive obsession
  - Person names with proper validation
  - International phone numbers - keep country code, needed for calling  
  - Structured Polish addresses - no country field needed
- **Address design**: Single constructor, natural parameter order, apartment can be letters (15D, A)
- **Patient creation**: Use `Patient.Register()` factory method instead of constructor for clearer intent
- **DTO design**: Use primitive types in DTOs with nested records for clean serialization and consistency with existing codebase
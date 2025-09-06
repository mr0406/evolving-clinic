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
- **Appointment scheduling**: Changed from patient name strings to proper PatientId references - solves "multiple John Smith" problem
- **Step definitions separation**: Use `GetAllPatientsQuery` with `GetAllDtos()` repository method - transformation logic in repository layer, query handler delegates, step definitions work with DTOs
- **PatientId validation**: `ScheduleAppointmentCommandHandler` doesn't validate PatientId existence - assumes GUIDs won't be randomly provided, can be enforced at relational DB level
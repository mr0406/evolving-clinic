# Development Log - Business Hours Validation

## Key Decisions

### 1. Weekend Schedule Creation
**Decision**: Allow `DailyAppointmentSchedule` creation for weekends, but reject appointments during scheduling.
- **Alternative**: Pre-generate schedules for future days
- **Why rejected**: Pre-generation adds complexity without clear benefit

### 2. Business Test Strategy

| Test Type | Purpose | Coverage |
|-----------|---------|----------|
| **Unit Tests** | Verify implementation details | Edge cases and boundary conditions |
| **Business Tests** | Validate business rules | Representative scenarios only |

**Key insights**:
- Realized which scenarios belong where - prevents test duplication between layers
- This confirmed the test separation was the right approach
- In a real application, business tests would likely be integration tests

### 3. GetOrCreate Logic Moved to Application Layer
**Decision**: Use `GetOptional` and create schedules on-demand:
```csharp
var schedule = await repository.GetOptional(command.Date) 
               ?? new DailyAppointmentSchedule(command.Date);
```
- **Why**: Creation logic doesn't belong in repository - repositories should be simple
- **Benefit**: Clean separation - application layer handles orchestration

## Refactors
**Refactored**: `ScheduleAppointment(string patientName, TimeOnly startTime, TimeOnly endTime)`
- Eliminates impossible date validation scenarios
- Cleaner API - no external `AppointmentTimeSlot` construction needed
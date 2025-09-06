# Development Log - Patient Registration

## Domain Design

### 🎯 Patient - New Aggregate Root
- **Factory method**: `Patient.Register()` instead of constructor - clearer intent
- **Rich value objects**: PersonName, PhoneNumber, Address - avoids primitive obsession
- **Unique identification**: Solves "multiple John Smith" problem with proper patient IDs

### 🎯 Value Objects - Shared Domain Concepts
- **PersonName**: FirstName + LastName (simple Polish naming)
- **PhoneNumber**: CountryCode + Number with basic validation
- **Address**: Polish addressing (Street, HouseNumber, Apartment?, PostalCode, City)

## Architecture Evolution

### 🔄 Appointment Scheduling Refactor
**Before**: `ScheduleAppointmentCommand(DateOnly date, string patientName, ...)`
**After**: `ScheduleAppointmentCommand(DateOnly date, Guid patientId, ...)`

- **Impact**: All scheduling now uses proper patient references
- **Benefit**: Eliminates patient name ambiguity completely

### 📡 Query Layer Extension
**New**: `GetAllPatientsQuery` + `GetAllDtos()` repository method
- **Responsibility split**: Repository handles entity→DTO transformation
- **Query handler**: Simple delegation, no business logic

## Key Decisions

### 1. Value Object Complexity
**Decision**: Keep value objects simple, avoid over-engineering
- **PhoneNumber**: Basic validation only, no normalization logic
- **Address**: Single constructor with natural parameter order
- **Why**: Implement what we need now, not what we might need

### 2. DTO Design Pattern
**Decision**: Primitive types in DTOs with nested records
```csharp
new PatientDto.PersonNameData(firstName, lastName)
```
- **Consistency**: Matches existing codebase patterns
- **Serialization**: Clean primitive types for external boundaries

### 3. Step Definition Decoupling
**Decision**: Query-based patient lookup instead of shared context
- **Alternative rejected**: SharedTestContext coupling between step definitions
- **Benefit**: Each step definition works independently
- **Real-world alignment**: Mimics how actual API would work

### 4. PatientId Validation
**Decision**: `ScheduleAppointmentCommandHandler` doesn't validate PatientId existence
- **Assumption**: GUIDs won't be randomly provided in real usage
- **Future**: Can be enforced at relational database level
- **Pragmatic**: Keeps command handler focused on core scheduling logic

## Technical Debt

### 🏗️ Domain Infrastructure
- **No aggregate base classes**: Still keeping it simple
- **No domain events**: Patient registration doesn't trigger events yet
- **No complex validation**: Basic value object validation only

## Summary

**Major milestone**: First rich domain aggregate with proper value objects. The "multiple John Smith" problem is now solved with proper patient identification.

**Architecture maturity**: Query layer expanded, repository patterns consistent, step definition decoupling achieved.

**Domain complexity**: Value objects emerging naturally from business needs - this is exactly how domain modeling should evolve.

---

> _"Rich domains emerge from real business problems - patient identification was the catalyst"_
# Development Log - Doctor Work Schedules

## Key Decisions

### 1. TimeRange Value Object Creation 🕒
**Decision**: Create `TimeRange` value object instead of separate start/end times.

**Implementation**: `public record TimeRange(TimeOnly Start, TimeOnly End)` in `Domain.Shared`

**Benefits**:
- **Type safety**: Can't swap start/end parameters
- **Validation**: Start < End validation encapsulated
- **Reusable**: Shared across aggregates

### 2. Copy TimeRange to DailyAppointmentSchedule 📋
**Decision**: Copy doctor's working hours to `DailyAppointmentSchedule` during creation instead of maintaining reference.

**Trade-off**: Working hours might change later, but system is too naive to handle this complexity right now.

**Benefits**:
- **Simple implementation**: No need to track changes to doctor schedules
- **Performance**: No lookups needed during appointment scheduling
- **Pragmatic**: Solves current needs without over-engineering

### 3. Pass Data Instead of Whole Aggregate 📦
**Decision**: Extract working hours data from `DoctorWorkSchedule` instead of passing whole aggregate to appointment scheduling.

**Implementation**: Use `doctorWorkSchedule.CreateSnapshot().WeeklySchedule` to get only needed data.

**Benefits**:
- **Aggregate boundaries**: Respects DDD principle of not passing whole aggregates between contexts
- **Minimal coupling**: Appointment scheduling only gets what it needs
- **Clear dependencies**: Explicit about which data is required

### 4. Application Layer Validation in ScheduleAppointmentCommandHandler ⚠️
**Decision**: Add doctor work schedule validation and existence checks in application layer.

**Implementation**:
- Validate doctor work schedule exists
- Check if doctor works on requested day
- Throw `ArgumentException` for validation failures

**Trade-off**: Application layer now contains business validation logic that feels heavy.

**Rationale**:
- **Defensive programming**: Prevents invalid appointments from being created
- **Clear error messages**: Users get specific feedback about validation failures
- **Pragmatic**: Keeps domain aggregate focused on core scheduling logic

**Technical debt**: Consider moving validation to domain service in future iterations.

### 6. Consolidate BDD Guidelines into CLAUDE.md 📋
**Action**: Moved business test guidelines from separate `docs/guidelines/business-tests-guidelines.md` into main `CLAUDE.md` file.

**Benefits**:
- **Single source**: All project guidelines in one centralized location
- **Easier maintenance**: No need to sync multiple guideline files
- **Better discoverability**: Developers only need to reference CLAUDE.md

---

## Refactors

### 1. Use TimeRange in DailyAppointmentSchedule 🔄
**Before**: `DailyAppointmentSchedule.Create(key, workingStartTime, workingEndTime)`
**After**: `DailyAppointmentSchedule.Create(key, workingHours)`

**Benefits**:
- **Consistency**: Same time range concept across both aggregates
- **Simplified API**: Single parameter instead of two separate times

### 3. IDispatcher Interface and BusinessTestsDispatcher 🔧
**Decision**: Create `IDispatcher` interface and `BusinessTestsDispatcher` decorator for BDD error handling.

**Implementation**:
- **IDispatcher**: Interface with 3 methods (Execute command, Execute command with result, ExecuteQuery)
- **BusinessTestsDispatcher**: Decorator that captures exceptions globally in `BddErrorContext`
- **Global error state**: Simple static field, no thread safety (single-threaded tests)

**Benefits**:
- **Clean separation**: Application layer unchanged, BDD concerns isolated
- **DRY**: No more `_scenarioException` fields in every step definition
- **Global error handling**: One place to capture and check exceptions

---
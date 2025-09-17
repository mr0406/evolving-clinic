# Multiple Doctors - Development Log

## Key decisions

### 1. Doctor-Centric Appointment Scheduling
**Decision**: Use `doctorCode` in appointment scheduling, acknowledging this may evolve in the future.
- **Current scope**: Only doctors perform healthcare services and handle appointments
- **Business assumption**: Doctor is the primary service provider for all appointments
- **Future considerations**:
  - Other staff (nurses, specialists, assistants) might perform services
  - Multiple staff members might be involved in single appointments
  - Services might be performed by non-doctor professionals
- **Rationale**:
  - **Current business reality**: All services are doctor-provided today
  - **Simple implementation**: Direct doctor-to-appointment relationship is sufficient
  - **YAGNI principle**: Don't over-engineer for uncertain future service models
- **Alternative considered**: Generic "service provider" or "staff member" concept
- **Why not**: Adds complexity without current business value - doctors handle everything today

### 2. Doctor Validation Strategy
**Decision**: Do not validate doctor existence in application layer.
- **Rationale**: Foreign key constraints can handle this validation at database level
- **Benefits**: Keeps application layer focused on business logic
- **Trade-off**: Relies on infrastructure layer for referential integrity

### 3. DailyAppointmentSchedule Key Structure
**Decision**: Created composite key `Key(string DoctorCode, DateOnly Date)` to uniquely identify schedules.
- **Doctor code comes first**: Logical grouping for doctor-centric view
- **Benefits**:
  - Enables efficient lookups by doctor and date combination
  - No Guid needed since the combination is naturally unique for business domain
  - Key is immutable record type following DDD patterns
- **Replaces**: Previous approach where date and doctor were separate properties

### 4. Missing Schedule Handling Strategy
**Decision**: Return empty DTO when no schedule exists for a doctor/date combination.
- **Business reality**: Schedules don't exist until first appointment is scheduled
- **Alternatives considered**:
  - **Pre-create all schedules**: Create empty schedules for all doctors/dates upfront
    - **Why not**: Massive data overhead, doesn't reflect business reality
  - **Throw exception**: Return error when schedule doesn't exist
    - **Why not**: Breaks real business scenarios where checking empty schedules is normal
- **Benefits**:
  - Aligns with business workflow (schedules emerge naturally)
  - Supports queries for availability checking
  - Avoids forcing artificial schedule creation
- **Implementation**: Return `DailyAppointmentScheduleDto` with empty appointments list
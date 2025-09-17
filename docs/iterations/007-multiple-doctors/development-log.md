# Multiple Doctors - Development Log

## Iteration 007

### Implementation Progress

- Use unique code for Doctors, not Guid - easier to identify
- Name entity doctors, but maybe in the future it will not only be doctors, more like employees, and also maybe multiple employees, because main guy can have assistants, right now this is doctor so focus on this only
- do not validate if doctor exists in application layer (it can be checked my foreign key)
- DailyAppointmentSchedule create a key which is date, and doctor code
- Not sure if this is fine that we do not have Guid id
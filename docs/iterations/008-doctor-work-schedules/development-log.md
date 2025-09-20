# Development Log - Doctor Work Schedules

1. Copy time to the DailyAppointmentSchedule during creation. Minus is that it can change or something, but our system is too naive to care about this right now :)
2. Pass not whole object to the appointment scheduling but only some data.
3. Throw error o application layer then no work schedule for doctor.
4. Create TimeRange value object
5. ScheduleAppointmentCommandHandler - feel like to much logic in the application layer
6. Remove BDD tests guidelines and move them to claude 

Refactor:
1. Use TimeRange in the DailyAppointmentSchedule
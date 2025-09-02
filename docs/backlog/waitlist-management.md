# Waitlist Management

## Background Story

Dr. Smith's practice is becoming popular, and Sarah increasingly has to tell patients "we're fully booked" when they call for appointments. Yesterday, Mr. Garcia called needing an appointment but there were no openings for two weeks. Sarah had to tell him "I'll keep you in mind if anything opens up." An hour later, Mr. Thompson canceled his Tuesday appointment, but Sarah had forgotten about Mr. Garcia's request and the slot went unused. Sarah needs a waitlist system where she can track patients who want appointments, so she can call them when slots become available and no appointment time gets wasted.

## User Stories

1. **As a receptionist**<br>
   I want to add patients to a waitlist<br>
   So that I can offer them slots when cancellations occur

2. **As a receptionist**<br>
   I want to remove patients from the waitlist<br>
   So that I can keep the list up to date when patients no longer need appointments

3. **As a receptionist**<br>
   I want to schedule appointments from waitlist entries<br>
   So that patients are automatically removed from the waitlist when they get scheduled

## Requirements

1. Add patients to waitlist when no appointments are available
2. Store patient details and optional notes about preferences for waitlist
3. When scheduling appointment from waitlist entry, automatically remove patient from waitlist
4. Remove patients from waitlist when they no longer need appointment
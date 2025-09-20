# Doctor Work Schedules

## Background Story

Dr. Smith's practice now has multiple doctors, but they don't all work the same hours. Dr. Smith works traditional Monday-Friday 9 AM to 5 PM, but Dr. Jones prefers to work longer days with a day off - she works Monday, Tuesday, Thursday, and Friday from 8 AM to 6 PM, and takes Wednesdays off completely. Dr. Martinez, who just joined the practice, works part-time on weekends - Saturday and Sunday from 10 AM to 4 PM.

Currently, the system enforces the same business hours (Monday-Friday, 9 AM-5 PM) for all doctors, which creates problems. Sarah can't schedule appointments with Dr. Jones on Wednesday even though Dr. Jones doesn't work that day, and she can't schedule weekend appointments with Dr. Martinez even though he's available. Patients are frustrated when they're told "we're closed" on days when their preferred doctor is actually working.

Sarah needs a way to define individual work schedules for each doctor so that appointments can only be scheduled during their actual working hours. This will prevent scheduling conflicts and ensure patients can book appointments when their chosen doctor is truly available.

## User Stories

1. **As a clinic administrator**<br>
   I want to define custom work schedules for each doctor<br>
   So that each doctor can work their preferred hours and days

2. **As a receptionist**<br>
   I want appointments to only be schedulable during a doctor's working hours<br>
   So that I don't accidentally book patients when the doctor isn't available

3. **As a receptionist**<br>
   I want to see which doctors are working on any given day<br>
   So that I can offer appropriate appointment options to patients

4. **As a clinic administrator**<br>
   I want to set some days as completely off for specific doctors<br>
   So that doctors can have proper rest days without any appointments

## Requirements

1. Define individual work schedules for each doctor
2. Set different working hours for each day of the week (Monday through Sunday)
3. Allow doctors to have completely off days (no working hours)
4. Prevent appointment scheduling outside a doctor's working hours
5. Replace the current system-wide business hours (Monday-Friday, 9 AM-5 PM) with doctor-specific schedules
6. Support flexible scheduling - different doctors can work different days and times
7. Each doctor's schedule should specify start and end times for each day they work

## Current System Behavior

Currently, all appointments are restricted to Monday-Friday, 9:00 AM - 5:00 PM regardless of which doctor the appointment is with. This system-wide restriction doesn't account for individual doctor availability and prevents flexible scheduling that would better serve both doctors and patients.
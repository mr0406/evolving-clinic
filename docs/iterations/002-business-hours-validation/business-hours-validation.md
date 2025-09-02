# Business Hours Validation

## Background Story

After using the digital appointment system for a few weeks, Sarah accidentally scheduled Mrs. Johnson for 6 PM on a Saturday while reviewing the week's appointments. Dr. Smith only works weekdays from 9 AM to 5 PM, but the current system allows appointments to be scheduled at any time on any day. Sarah realizes she needs the system to prevent scheduling outside of business hours to avoid these embarrassing mistakes with patients.

## User Stories

1. **As a receptionist**<br>
   I want the system to reject appointments scheduled outside business hours<br>
   So that I don't accidentally book patients when the clinic is closed

## Requirements

1. Appointments can only be scheduled Monday through Friday
2. Appointments can only be scheduled between 9:00 AM and 5:00 PM

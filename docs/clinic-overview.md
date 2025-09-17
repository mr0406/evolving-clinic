# Clinic Overview

This document describes the current operational state of Dr. Smith's medical practice.

## How It Works Today

- Multiple doctor practice
- Sarah (the receptionist) manages both patient registration and appointment scheduling for all doctors

## Doctor Registration
- Doctors must be registered in the system before appointments can be scheduled with them
- Each doctor has a unique code for easy identification (e.g., "SMITH", "JONES")

## Patient Registration
- Patients must be registered before scheduling appointments
- System assigns unique patient IDs to solve identity conflicts

## Healthcare Service Types
- Service types define standard appointment durations and pricing for different medical services
- Each service has a name, code, predefined duration, and fixed price (e.g., "Routine Check-up", 30 minutes, $100.00)
- Service types must be registered before appointments can be scheduled
- Consistent pricing eliminates manual price entry errors
- Complete price history tracking maintains records of all pricing changes with effective dates

## Appointment Scheduling
- No more double bookings
- Appointments have guaranteed time slots
- Business hours enforced (Monday-Friday, 9:00 AM - 5:00 PM only)
- System prevents scheduling outside clinic operating hours
- Automated duration calculation based on service types
- Automatic price capture from service type at time of scheduling
- Appointments can be scheduled for specific doctors
- Each doctor maintains their own separate appointment schedule

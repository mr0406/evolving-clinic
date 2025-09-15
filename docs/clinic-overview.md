# Clinic Overview

This document describes the current operational state of Dr. Smith's medical practice.

## How It Works Today

- Single doctor, private practice
- Sarah (the receptionist) manages both patient registration and appointment scheduling

## Patient Registration
- Patients must be registered before scheduling appointments
- System assigns unique patient IDs to solve identity conflicts

## Healthcare Service Types
- Service types define standard appointment durations for different medical services
- Each service has a name, code, and predefined duration (e.g., "Routine Check-up", 30 minutes)
- Service types must be registered before appointments can be scheduled

## Appointment Scheduling
- No more double bookings
- Appointments have guaranteed time slots
- Business hours enforced (Monday-Friday, 9:00 AM - 5:00 PM only)
- System prevents scheduling outside clinic operating hours
- Automated duration calculation based on service types

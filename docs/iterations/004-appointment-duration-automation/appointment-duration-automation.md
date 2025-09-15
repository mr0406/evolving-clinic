# Appointment Duration Automation

## Background Story

Sarah has been scheduling appointments by manually calculating appointment durations for different types of healthcare services. When patients call to schedule appointments, she needs to remember that routine check-ups take 30 minutes, blood pressure monitoring sessions take 15 minutes, and diabetes consultations take 45 minutes. Currently, she has to keep this information in her notes and manually calculate end times. To streamline her daily work, Sarah would like the system to store different healthcare service types with their standard durations, so she can simply select the service type and start time, and have the system automatically handle the scheduling details.

## User Stories

1. **As a receptionist**<br>
   I want to define different healthcare service types with their standard durations<br>
   So that I can streamline appointment scheduling without manual duration calculations

2. **As a receptionist**<br>
   I want to schedule appointments by selecting service type and start time<br>
   So that the system automatically calculates the correct end time

## Requirements

1. Create healthcare service types with name, code, and duration
2. When scheduling appointments, associate appointment with specific service type
3. System automatically calculates appointment end time based on service duration
4. Store service reference in scheduled appointments
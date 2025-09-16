Feature: Schedule Appointment

As a clinic staff member
I want to schedule patient appointments
So that patients can receive medical care

Scenario: Schedule a new appointment
	Given patient "John" "Smith" is registered
	And healthcare service type "Routine Check-up" with code "RCU", duration "60 minutes" and price "$120.00" exists
	When I schedule an appointment for "John" "Smith" with healthcare service type "RCU" on "2024-01-15" at "10:00"
	Then the scheduled appointment should be:
		| Patient Name | Healthcare Service Code | Healthcare Service Name | Date       | Start Time | End Time | Price   |
		| John Smith   | RCU                     | Routine Check-up        | 2024-01-15 | 10:00      | 11:00    | $120.00 |
	And there should be 1 appointment

Scenario: Schedule two consecutive appointments
	Given patient "Alice" "Johnson" is registered
	And patient "Bob" "Wilson" is registered
	And healthcare service type "Routine Check-up" with code "RCU", duration "60 minutes" and price "$120.00" exists
	And I have scheduled an appointment for "Alice" "Johnson" with healthcare service type "RCU" on "2024-01-15" at "09:00"
	When I schedule an appointment for "Bob" "Wilson" with healthcare service type "RCU" on "2024-01-15" at "10:00"
	Then the scheduled appointment should be:
		| Patient Name | Healthcare Service Code | Healthcare Service Name | Date       | Start Time | End Time | Price   |
		| Bob Wilson   | RCU                     | Routine Check-up        | 2024-01-15 | 10:00      | 11:00    | $120.00 |
	And there should be 2 appointments

Scenario: Cannot schedule overlapping appointment
	Given patient "Alice" "Johnson" is registered
	And patient "Bob" "Wilson" is registered
	And healthcare service type "Routine Check-up" with code "RCU", duration "60 minutes" and price "$120.00" exists
	And healthcare service type "Consultation" with code "CONS", duration "30 minutes" and price "$80.00" exists
	And I have scheduled an appointment for "Alice" "Johnson" with healthcare service type "RCU" on "2024-01-15" at "10:00"
	When I schedule an appointment for "Bob" "Wilson" with healthcare service type "CONS" on "2024-01-15" at "10:30"
	Then there should be 1 appointment

Scenario: Cannot schedule appointment on weekend
	Given patient "John" "Smith" is registered
	And healthcare service type "Routine Check-up" with code "RCU", duration "60 minutes" and price "$120.00" exists
	When I schedule an appointment for "John" "Smith" with healthcare service type "RCU" on "2024-01-13" at "10:00"
	Then there should be 0 appointments

Scenario: Cannot schedule appointment outside business hours
	Given patient "John" "Smith" is registered
	And healthcare service type "Routine Check-up" with code "RCU", duration "60 minutes" and price "$120.00" exists
	When I schedule an appointment for "John" "Smith" with healthcare service type "RCU" on "2024-01-15" at "08:30"
	Then there should be 0 appointments
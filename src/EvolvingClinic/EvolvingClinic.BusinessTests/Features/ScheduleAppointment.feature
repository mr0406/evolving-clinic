Feature: Schedule Appointment

As a clinic staff member
I want to schedule patient appointments
So that patients can receive medical care

Scenario: Schedule a new appointment
	Given I registered patients:
		| First Name | Last Name |
		| John       | Smith     |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Healthcare Service Code | Start Time |
		| John Smith   | RCU                     | 10:00      |
	Then the scheduled appointment should be:
		| Patient Name | Healthcare Service Code | Healthcare Service Name | Date       | Start Time | End Time | Price   |
		| John Smith   | RCU                     | Routine Check-up        | 2024-01-15 | 10:00      | 11:00    | $120.00 |
	And there should be 1 appointment

Scenario: Schedule two consecutive appointments
	Given I registered patients:
		| First Name | Last Name |
		| Alice      | Johnson   |
		| Bob        | Wilson    |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	And I scheduled appointment on "2024-01-15":
		| Patient Name  | Healthcare Service Code | Start Time |
		| Alice Johnson | RCU                     | 09:00      |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Healthcare Service Code | Start Time |
		| Bob Wilson   | RCU                     | 10:00      |
	Then the scheduled appointment should be:
		| Patient Name | Healthcare Service Code | Healthcare Service Name | Date       | Start Time | End Time | Price   |
		| Bob Wilson   | RCU                     | Routine Check-up        | 2024-01-15 | 10:00      | 11:00    | $120.00 |
	And there should be 2 appointments

Scenario: Cannot schedule overlapping appointment
	Given I registered patients:
		| First Name | Last Name |
		| Alice      | Johnson   |
		| Bob        | Wilson    |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
		| Consultation            | CONS | 30 minutes | $80.00  |
	And I scheduled appointment on "2024-01-15":
		| Patient Name  | Healthcare Service Code | Start Time |
		| Alice Johnson | RCU                     | 10:00      |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Healthcare Service Code | Start Time |
		| Bob Wilson   | CONS                    | 10:30      |
	Then there should be 1 appointment

Scenario: Cannot schedule appointment on weekend
	Given I registered patients:
		| First Name | Last Name |
		| John       | Smith     |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	When I schedule appointment on "2024-01-13":
		| Patient Name | Healthcare Service Code | Start Time |
		| John Smith   | RCU                     | 10:00      |
	Then there should be 0 appointments

Scenario: Cannot schedule appointment outside business hours
	Given I registered patients:
		| First Name | Last Name |
		| John       | Smith     |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Healthcare Service Code | Start Time |
		| John Smith   | RCU                     | 08:30      |
	Then there should be 0 appointments
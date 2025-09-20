Feature: Schedule Appointment

As a clinic staff member
I want to schedule patient appointments
So that patients can receive medical care

Scenario: Schedule a new appointment
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	And I registered patients:
		| First Name | Last Name |
		| Jane       | Doe       |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	And I created doctor work schedule for "SMITH":
		| Day    | Start Time | End Time |
		| Monday | 09:00      | 17:00    |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Doctor Code | Healthcare Service Code | Start Time |
		| Jane Doe     | SMITH       | RCU                     | 10:00      |
	Then the scheduled appointment should be:
		| Patient Name | Doctor Code | Healthcare Service Code | Healthcare Service Name | Date       | Start Time | End Time | Price   |
		| Jane Doe     | SMITH       | RCU                     | Routine Check-up        | 2024-01-15 | 10:00      | 11:00    | $120.00 |
	And there should be 1 appointment for doctor "SMITH" on "2024-01-15"

Scenario: Schedule two consecutive appointments
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	And I registered patients:
		| First Name | Last Name |
		| Alice      | Johnson   |
		| Bob        | Wilson    |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	And I created doctor work schedule for "SMITH":
		| Day    | Start Time | End Time |
		| Monday | 08:00      | 16:00    |
	And I scheduled appointment on "2024-01-15":
		| Patient Name  | Doctor Code | Healthcare Service Code | Start Time |
		| Alice Johnson | SMITH       | RCU                     | 09:00      |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Doctor Code | Healthcare Service Code | Start Time |
		| Bob Wilson   | SMITH       | RCU                     | 10:00      |
	Then the scheduled appointment should be:
		| Patient Name | Doctor Code | Healthcare Service Code | Healthcare Service Name | Date       | Start Time | End Time | Price   |
		| Bob Wilson   | SMITH       | RCU                     | Routine Check-up        | 2024-01-15 | 10:00      | 11:00    | $120.00 |
	And there should be 2 appointments for doctor "SMITH" on "2024-01-15"

Scenario: Cannot schedule overlapping appointment
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	And I registered patients:
		| First Name | Last Name |
		| Alice      | Johnson   |
		| Bob        | Wilson    |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
		| Consultation            | CONS | 30 minutes | $80.00  |
	And I created doctor work schedule for "SMITH":
		| Day    | Start Time | End Time |
		| Monday | 10:00      | 18:00    |
	And I scheduled appointment on "2024-01-15":
		| Patient Name  | Doctor Code | Healthcare Service Code | Start Time |
		| Alice Johnson | SMITH       | RCU                     | 10:00      |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Doctor Code | Healthcare Service Code | Start Time |
		| Bob Wilson   | SMITH       | CONS                    | 10:30      |
	Then there should be 1 appointment for doctor "SMITH" on "2024-01-15"

Scenario: Cannot schedule appointment on weekend
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	And I registered patients:
		| First Name | Last Name |
		| Jane       | Doe       |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	And I created doctor work schedule for "SMITH":
		| Day       | Start Time | End Time |
		| Monday    | 09:00      | 17:00    |
		| Tuesday   | 09:00      | 17:00    |
		| Wednesday | 09:00      | 17:00    |
		| Thursday  | 09:00      | 17:00    |
		| Friday    | 09:00      | 17:00    |
	When I schedule appointment on "2024-01-13":
		| Patient Name | Doctor Code | Healthcare Service Code | Start Time |
		| Jane Doe     | SMITH       | RCU                     | 10:00      |
	Then there should be 0 appointments for doctor "SMITH" on "2024-01-13"

Scenario: Cannot schedule appointment outside business hours
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	And I registered patients:
		| First Name | Last Name |
		| Jane       | Doe       |
	And I added healthcare service types:
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 60 minutes | $120.00 |
	And I created doctor work schedule for "SMITH":
		| Day    | Start Time | End Time |
		| Monday | 09:00      | 15:00    |
	When I schedule appointment on "2024-01-15":
		| Patient Name | Doctor Code | Healthcare Service Code | Start Time |
		| Jane Doe     | SMITH       | RCU                     | 08:30      |
	Then there should be 0 appointments for doctor "SMITH" on "2024-01-15"
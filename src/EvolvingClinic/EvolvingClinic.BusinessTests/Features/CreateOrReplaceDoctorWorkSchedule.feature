Feature: Create Or Replace Doctor Work Schedule

As a clinic administrator
I want to create or replace doctor work schedules
So that appointments can be scheduled during doctor's working hours

Scenario: Create a new doctor work schedule successfully
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	When I create or replace doctor work schedule for "SMITH":
		| Day       | Start Time | End Time |
		| Monday    | 09:00      | 17:00    |
		| Tuesday   | 09:00      | 17:00    |
		| Wednesday | 09:00      | 17:00    |
		| Thursday  | 09:00      | 17:00    |
		| Friday    | 09:00      | 17:00    |
	Then the doctor work schedule for "SMITH" should be:
		| Day       | Start Time | End Time |
		| Monday    | 09:00      | 17:00    |
		| Tuesday   | 09:00      | 17:00    |
		| Wednesday | 09:00      | 17:00    |
		| Thursday  | 09:00      | 17:00    |
		| Friday    | 09:00      | 17:00    |

Scenario: Replace existing doctor work schedule
	Given I registered doctors:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	And I created doctor work schedule for "SMITH":
		| Day     | Start Time | End Time |
		| Monday  | 09:00      | 17:00    |
		| Tuesday | 09:00      | 17:00    |
	When I create or replace doctor work schedule for "SMITH":
		| Day       | Start Time | End Time |
		| Monday    | 08:00      | 16:00    |
		| Wednesday | 10:00      | 18:00    |
		| Friday    | 09:00      | 15:00    |
	Then the doctor work schedule for "SMITH" should be:
		| Day       | Start Time | End Time |
		| Monday    | 08:00      | 16:00    |
		| Wednesday | 10:00      | 18:00    |
		| Friday    | 09:00      | 15:00    |

Scenario: Create doctor work schedule with part-time hours
	Given I registered doctors:
		| Code | First Name | Last Name |
		| DOE  | Jane       | Doe       |
	When I create or replace doctor work schedule for "DOE":
		| Day       | Start Time | End Time |
		| Tuesday   | 13:00      | 17:00    |
		| Thursday  | 13:00      | 17:00    |
	Then the doctor work schedule for "DOE" should be:
		| Day       | Start Time | End Time |
		| Tuesday   | 13:00      | 17:00    |
		| Thursday  | 13:00      | 17:00    |

Scenario: Create doctor work schedule with different hours each day
	Given I registered doctors:
		| Code   | First Name | Last Name |
		| WILSON | Bob        | Wilson    |
	When I create or replace doctor work schedule for "WILSON":
		| Day       | Start Time | End Time |
		| Monday    | 08:00      | 12:00    |
		| Wednesday | 14:00      | 18:00    |
		| Friday    | 10:00      | 16:00    |
	Then the doctor work schedule for "WILSON" should be:
		| Day       | Start Time | End Time |
		| Monday    | 08:00      | 12:00    |
		| Wednesday | 14:00      | 18:00    |
		| Friday    | 10:00      | 16:00    |

Scenario: Cannot create work schedule for non-existent doctor
	When I create or replace doctor work schedule for "NONEXISTENT":
		| Day    | Start Time | End Time |
		| Monday | 09:00      | 17:00    |
	Then I should get an error "Doctor with code 'NONEXISTENT' not found"
Feature: Schedule Appointment

As a clinic staff member
I want to schedule patient appointments
So that patients can receive medical care

Scenario: Schedule a new appointment
	Given patient "John" "Smith" is registered
	When I schedule an appointment for "John" "Smith" on "2024-01-15" from "10:00" to "11:00"
	Then the appointment should be scheduled successfully
	And there should be 1 appointments in the schedule

Scenario: Schedule two consecutive appointments
	Given patient "Alice" "Johnson" is registered
	And patient "Bob" "Wilson" is registered
	And I have scheduled an appointment for "Alice" "Johnson" on "2024-01-15" from "09:00" to "10:00"
	When I schedule an appointment for "Bob" "Wilson" on "2024-01-15" from "10:00" to "11:00"
	Then the appointment should be scheduled successfully
	And there should be 2 appointments in the schedule

Scenario: Cannot schedule overlapping appointment
	Given patient "Alice" "Johnson" is registered
	And patient "Bob" "Wilson" is registered
	And I have scheduled an appointment for "Alice" "Johnson" on "2024-01-15" from "10:00" to "11:00"
	When I schedule an appointment for "Bob" "Wilson" on "2024-01-15" from "10:30" to "11:30"
	Then the appointment should fail to be scheduled
	And there should be 1 appointments in the schedule

Scenario: Cannot schedule appointment with less than 15 minutes duration
	Given patient "John" "Smith" is registered
	When I schedule an appointment for "John" "Smith" on "2024-01-15" from "10:00" to "10:10"
	Then the appointment should fail to be scheduled
	And there should be no appointments in the schedule

Scenario: Cannot schedule appointment with invalid time range
	Given patient "John" "Smith" is registered
	When I schedule an appointment for "John" "Smith" on "2024-01-15" from "11:00" to "10:00"
	Then the appointment should fail to be scheduled
	And there should be no appointments in the schedule

Scenario: Cannot schedule appointment on weekend
	Given patient "John" "Smith" is registered
	When I schedule an appointment for "John" "Smith" on "2024-01-13" from "10:00" to "11:00"
	Then the appointment should fail to be scheduled
	And there should be no appointments in the schedule

Scenario: Cannot schedule appointment outside business hours
	Given patient "John" "Smith" is registered
	When I schedule an appointment for "John" "Smith" on "2024-01-15" from "08:30" to "09:30"
	Then the appointment should fail to be scheduled
	And there should be no appointments in the schedule
Feature: Register Patient

As a clinic staff member
I want to register new patients
So that I can schedule appointments and maintain patient records

Scenario: Register a new patient successfully
	When I register a patient "Patrick Jones" born on "1985-03-20" with phone "+1 5551234567" and address "Main Street 123 A, 10001 New York"
	Then the patient should be registered successfully
	And the patient should be registered with the correct data
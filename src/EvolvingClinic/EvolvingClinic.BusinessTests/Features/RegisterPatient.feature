Feature: Register Patient

As a clinic staff member
I want to register new patients
So that I can schedule appointments and maintain patient records

Scenario: Register a new patient successfully
	When I register patient on "2024-09-16":
		| First Name | Last Name | Date of Birth | Phone Number  | Street Address    | Postal Code | City     |
		| Patrick    | Jones     | 1985-03-20    | +1 5551234567 | Main Street 123 A | 10001       | New York |
	Then the registered patient should be:
		| First Name | Last Name | Date of Birth | Phone Number  | Street Address    | Postal Code | City     |
		| Patrick    | Jones     | 1985-03-20    | +1 5551234567 | Main Street 123 A | 10001       | New York |
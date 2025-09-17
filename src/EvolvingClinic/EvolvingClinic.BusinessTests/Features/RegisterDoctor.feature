Feature: Register Doctor

As a clinic administrator
I want to register new doctors
So that patients can schedule appointments with them

Scenario: Register a new doctor successfully
	When I register a doctor:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
	Then the registered doctor should be:
		| Code  | First Name | Last Name |
		| SMITH | John       | Smith     |
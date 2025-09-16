Feature: Add Healthcare Service Type

As a clinic administrator
I want to add healthcare service types
So that I can define the services offered by the clinic

Scenario: Add a new healthcare service type
	Given '2024-09-16' date
	When I add healthcare service type "Routine Check-up" with code "RCU", duration "30 minutes" and price "$100.00"
	Then the healthcare service type should be added successfully
	And the healthcare service type should be added with the correct data
	And there should be 1 healthcare service types in the system
	And the price history should contain exactly:
		| Price   | EffectiveFrom | EffectiveTo |
		| $100.00 | 2024-09-16    |             |

Scenario: Add multiple healthcare service types
	Given I have added healthcare service type "Blood Test" with code "BT", duration "15 minutes" and price "$50.00"
	When I add healthcare service type "X-Ray" with code "XR", duration "45 minutes" and price "$150.00"
	Then the healthcare service type should be added successfully
	And the healthcare service type should be added with the correct data
	And there should be 2 healthcare service types in the system
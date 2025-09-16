Feature: Add Healthcare Service Type

As a clinic administrator
I want to add healthcare service types
So that I can define the services offered by the clinic

Scenario: Add a new healthcare service type
	When I add healthcare service type on "2024-09-16":
		| Healthcare Service Name | Code | Duration   | Price   |
		| Routine Check-up        | RCU  | 30 minutes | $100.00 |
	Then the added healthcare service type should be:
		| Healthcare Service Name | Code | Duration   | Current Price | Price History From | Price History To |
		| Routine Check-up        | RCU  | 30 minutes | $100.00       | 2024-09-16         |                  |
	And there should be 1 healthcare service type

Scenario: Add multiple healthcare service types
	Given I added healthcare service type on "2024-09-16":
		| Healthcare Service Name | Code | Duration   | Price  |
		| Blood Test              | BT   | 15 minutes | $50.00 |
	When I add healthcare service type on "2024-09-16":
		| Healthcare Service Name | Code | Duration   | Price    |
		| X-Ray                   | XR   | 45 minutes | $150.00 |
	Then the added healthcare service type should be:
		| Healthcare Service Name | Code | Duration   | Current Price | Price History From | Price History To |
		| X-Ray                   | XR   | 45 minutes | $150.00       | 2024-09-16         |                  |
	And there should be 2 healthcare service types
Feature: Add Healthcare Service Type

As a clinic administrator
I want to add healthcare service types
So that I can define the services offered by the clinic

Scenario: Add a new healthcare service type
	When I add healthcare service type "Routine Check-up" with code "RCU" and duration "30 minutes"
	Then the healthcare service type should be added successfully
	And the healthcare service type should be added with the correct data
	And there should be 1 healthcare service types in the system

Scenario: Add multiple healthcare service types
	Given I have added healthcare service type "Blood Test" with code "BT" and duration "15 minutes"
	When I add healthcare service type "X-Ray" with code "XR" and duration "45 minutes"
	Then the healthcare service type should be added successfully
	And the healthcare service type should be added with the correct data
	And there should be 2 healthcare service types in the system
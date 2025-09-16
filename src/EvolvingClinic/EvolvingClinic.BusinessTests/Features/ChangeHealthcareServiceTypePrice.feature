Feature: Change Healthcare Service Type Price

As a clinic administrator
I want to change the price of healthcare service types
So that I can maintain accurate pricing and track historical price changes

Scenario: Change price on different days creates price history
    Given "2024-09-14" date
    And I have a healthcare service type "Routine Check-up" with code "RCU", duration "30 minutes", and price "$80.00"
    When "2024-09-15" date
    And I change healthcare service type price of "RCU" to "$100.00"
    And "2024-09-16" date
    And I change healthcare service type price of "RCU" to "$110.00"
    Then the price history should contain exactly:
        | Price   | Effective From | Effective To |
        | $80.00  | 2024-09-14     | 2024-09-14   |
        | $100.00 | 2024-09-15     | 2024-09-15   |
        | $110.00 | 2024-09-16     |              |

Scenario: Change price on same day overrides previous change
    Given "2024-09-14" date
    And I have a healthcare service type "Routine Check-up" with code "RCU", duration "30 minutes", and price "$80.00"
    When I change healthcare service type price of "RCU" to "$90.00"
    And I change healthcare service type price of "RCU" to "$95.00"
    Then the price history should contain exactly:
        | Price  | Effective From | Effective To |
        | $95.00 | 2024-09-14     |              |
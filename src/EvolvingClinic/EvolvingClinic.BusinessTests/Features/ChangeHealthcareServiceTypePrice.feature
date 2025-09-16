Feature: Change Healthcare Service Type Price

As a clinic administrator
I want to change the price of healthcare service types
So that I can maintain accurate pricing and track historical price changes

Scenario: Change price on different days creates price history
    Given I added healthcare service type on "2024-09-14":
        | Healthcare Service Name | Code | Duration   | Price  |
        | Routine Check-up        | RCU  | 30 minutes | $80.00 |
    When I change healthcare service type price of "RCU" to "$100.00" on "2024-09-15"
    And I change healthcare service type price of "RCU" to "$110.00" on "2024-09-16"
    Then the price history should contain exactly:
        | Price   | Effective From | Effective To |
        | $80.00  | 2024-09-14     | 2024-09-14   |
        | $100.00 | 2024-09-15     | 2024-09-15   |
        | $110.00 | 2024-09-16     |              |

Scenario: Change price on same day overrides previous change
    Given I added healthcare service type on "2024-09-14":
        | Healthcare Service Name | Code | Duration   | Price  |
        | Routine Check-up        | RCU  | 30 minutes | $80.00 |
    When I change healthcare service type price of "RCU" to "$90.00" on "2024-09-14"
    And I change healthcare service type price of "RCU" to "$95.00" on "2024-09-14"
    Then the price history should contain exactly:
        | Price  | Effective From | Effective To |
        | $95.00 | 2024-09-14     |              |
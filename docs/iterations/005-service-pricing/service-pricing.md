# Service Pricing

## Background Story

Dr. Smith and Sarah have been manually tracking service prices on paper, leading to confusion and billing errors. Last month, Sarah quoted a routine check-up at \$75 to one patient and \$85 to another because she couldn't remember the correct price. When Dr. Smith reviewed the billing at month-end, he found inconsistent pricing across similar services. Additionally, they're spending too much time looking up prices during appointment scheduling - Sarah has to interrupt patient calls to check a printed price list that's often outdated.

Dr. Smith wants to store service prices directly in the system so that when appointments are scheduled, the price is automatically captured. This will ensure consistent pricing, speed up the scheduling process, and provide clear billing information for both staff and patients.

## User Stories

1. **As a clinic administrator**<br>
   I want to define prices for healthcare services<br>
   So that pricing is consistent and standardized across all appointments

2. **As a receptionist**<br>
   I want appointment prices to be automatically captured during scheduling<br>
   So that billing information is ready without manual price entry

## Requirements

1. Add price field to healthcare service types
2. Copy service price to appointment when scheduled
3. Basic price validation (positive amounts, reasonable ranges)
4. Support dollars only - no multiple currencies (small clinic, local patients)
5. Price should be captured at scheduling time to handle future price changes

## Technical Notes

- Store prices as decimal/money type
- Price is copied from service to appointment (not referenced) to handle price changes over time
- USD currency assumption - no internationalization needed
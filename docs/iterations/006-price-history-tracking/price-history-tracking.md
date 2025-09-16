# Price History Tracking

## Background Story

Dr. Smith has been running his clinic for several months now, and inflation is starting to affect his operating costs. Medical supply costs are rising, and he needs to adjust his service prices accordingly. Last week, he had to increase the price of routine check-ups from \$120 to \$135 due to increased equipment costs.

However, Dr. Smith realizes he has a problem - he has no historical record of when prices changed or what they used to be. When his accountant asked for pricing information for tax purposes, he couldn't provide accurate data. Additionally, some insurance companies require historical pricing information for reimbursement claims.

Sarah also mentioned that patients sometimes call asking about price changes, and she has no way to tell them when a particular service price was updated or what it cost previously.

While Dr. Smith could theoretically look through individual appointments to piece together pricing history, this approach is inefficient and unreliable. He needs the pricing information to be directly available within each service type definition.

Dr. Smith wants to maintain transparency with patients and needs proper documentation for accounting and regulatory purposes. He plans to update prices only at the beginning of each day before the clinic opens to keep things simple and avoid confusion during operating hours.

## User Stories

1. **As a clinic administrator**<br>
   I want to update service prices while automatically preserving historical pricing<br>
   So that I can maintain complete pricing records for accounting and regulatory needs

2. **As a clinic administrator**<br>
   I want to know what price was effective for any service on any given date<br>
   So that I can answer patient questions and provide accurate information to insurance companies

3. **As a clinic administrator**<br>
   I want to understand when each service price was last changed<br>
   So that I can make informed decisions about future pricing adjustments

## Requirements

1. Track complete price history within each healthcare service type (not derived from appointments)
2. Record effective dates (not times) for each price change
3. Maintain current pricing for new appointments
4. Store historical pricing with effective date periods
5. Support price updates only at the beginning of each day before clinic opens
6. Ability to determine what price was effective for any service on any given date
7. Preserve pricing history when service prices are updated
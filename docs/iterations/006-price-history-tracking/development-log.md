# Price History Tracking - Development Log

## Key Decisions

### 1. Current Price Field + History Collection (Intentional Duplication)
**Decision**: Maintain both `_price` field and `_priceHistory` collection.
- **Rationale**:
  - **Database persistence**: Separate price field maps cleanly to database columns
  - **Performance**: Direct access to current price without collection queries
  - **Encapsulation**: Price history is internal implementation detail
  - **Testing**: Can verify price field and history stay in sync
- **Trade-off**: Intentional duplication that must be kept consistent
- **Alternative considered**: Price as getter from history
- **Why not**: Database mapping complexity and performance considerations

### 2. Price History with Explicit End Dates
**Decision**: Store price history with explicit EffectiveFrom and EffectiveTo dates.
- **Implementation**: `PriceHistoryEntry(Money Price, DateOnly EffectiveFrom, DateOnly? EffectiveTo)`
- **Current entry**: Always has `EffectiveTo = null`
- **Benefits**:
  - Explicit date ranges make queries clearer
  - No ambiguity about when prices were effective
  - Easy to validate no gaps or overlaps in pricing periods

### 3. Same-Day Price Change Override
**Decision**: Allow multiple price changes on the same day by overriding the previous change.
- **Business assumption**: Price changes are made before the work day starts, so same-day corrections are administrative fixes
- **Scenario**: Admin makes pricing mistake and needs to correct it immediately before appointments begin
- **Implementation**: If changing price on same effective date, update existing entry instead of creating new one
- **Benefits**:
  - Prevents cluttered history from same-day corrections
  - Maintains clean audit trail for intentional price changes
  - Handles human error gracefully without breaking history consistency
  - Aligns with business practice of setting prices before daily operations
- **Alternative considered**: Always create new entries
- **Why not**: Same-day corrections aren't meaningful price changes, just error corrections

### 4. Private ApplyPriceChange Method
**Decision**: Extract all price change logic into single unified private method.
- **Contains**: Validation, entity searching, same-day override logic, history management
- **Usage**: Both constructor initialization and ChangePrice method
- **Benefits**:
  - DRY principle - single place for all price change logic
  - Consistent behavior between creation and updates
  - Complete encapsulation - all price logic in one place
  - No parameter passing needed - always operates on "today"
- **Method responsibility**: Validate price, find existing entries, handle override/history, update current price

### 5. ApplicationClock Introduction
**Decision**: Introduce ApplicationClock utility to replace direct DateTime calls for testable time-dependent logic.
- **Problem**: Price history logic depends on current date, making tests non-deterministic and hard to control
- **Solution**: Static ApplicationClock class that wraps DateTime functionality with test override capability
- **Implementation**: `ApplicationClock.Today` returns current date in production, settable date in tests
- **Benefits**:
  - **Deterministic testing**: Predictable dates enable reliable test scenarios
  - **Time simulation**: Easy testing of multi-day price change scenarios
  - **Production transparency**: Zero performance overhead, behaves exactly like DateTime.Today
  - **Test isolation**: Each test can control its own time context

### 6. Explicit Table Validation in BDD Tests
**Decision**: Replace generic "should be registered with correct data" assertions with explicit business-friendly tables.
- **Problem**:
  - Hidden validation logic in step definitions made tests opaque
  - Business stakeholders couldn't understand what was actually being verified
  - "Magic" assertions like "should have correct data" provided no insight into expectations
- **Solution**: Use explicit table validation showing all expected data in business language
- **Implementation**:
  ```gherkin
  Then the registered patient should be:
    | First Name | Last Name | Date of Birth | Phone Number  | Street Address    | Postal Code | City     |
    | Patrick    | Jones     | 1985-03-20    | +1 5551234567 | Main Street 123 A | 10001       | New York |
  ```
- **Benefits**:
  - **Business transparency**: Stakeholders can see exactly what's being validated
  - **No hidden magic**: All expectations are explicitly stated in the feature file
  - **Natural language**: Uses business terms like "First Name" instead of technical "Property/Value"
  - **Complete visibility**: Every field being validated is clearly shown
  - **Maintainability**: Changes to validation are visible in the feature file, not buried in step definitions
- **Trade-offs**:
  - Slightly more verbose feature files
  - Requires updating both feature and step definition for new fields
- **Alternative considered**: Technical Property/Value table approach
- **Why not**: Too technical, breaks business language of BDD, doesn't improve readability for stakeholders
- **Code cleanup**: Removed intermediate data structures and simplified step definitions to focus only on table validation
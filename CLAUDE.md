# Claude Guidelines for Evolving Clinic Project

## Project Overview
This is a **Domain-Driven Design experiment** showing how business requirements drive domain model evolution. It's a C#/.NET 8 clinic management system that starts simple and evolves organically under business pressure - **no big upfront design, no event storming, no predictions about patterns needed**.

The focus is on domain logic and comprehensive testing, with intentionally simple infrastructure (in-memory repositories, etc.).

## Architecture & Structure
- **Simplified Clean Architecture**: Domain → Application
- **CQRS**: Commands and Queries separated in Application layer
- **DDD**: Rich domain models with business logic encapsulation
- **Testing**: BDD with Reqnroll (SpecFlow successor), unit tests with domain focus

## Code Standards

### C# Conventions
- Use `.NET 8` target framework
- Enable `ImplicitUsings` and `Nullable` reference types
- Follow standard C# naming conventions (PascalCase for public members, camelCase for private)
- Prefer `record` types for DTOs and value objects

### Project Structure
```
src/EvolvingClinic/
├── EvolvingClinic.Domain/          # Domain entities, value objects, domain services
├── EvolvingClinic.Application/     # Commands, queries, application services
├── EvolvingClinic.BusinessTests/   # BDD tests using Reqnroll
└── EvolvingClinic.Domain.UnitTests/ # Unit tests for domain logic
```

### Testing Approach
- **BDD Tests**: Use Reqnroll for business scenario testing
- **Unit Tests**: Focus on domain logic and behavior using NUnit
- **Test Structure**: Given-When-Then pattern in both BDD and unit tests with explicit `// Given`, `// When`, `// Then` comments
  - `// Given`: Test setup, arrange data
  - `// When`: Single action only (method call, sometimes with setup date before action)
  - `// Then`: All assertions and snapshot creation. Never use `// When & Then`
- **Assertions**: Use Shouldly for readable assertions, including `Should.Throw<T>()` for exception testing
- **Exception Testing Pattern**: Follow correct Given-When-Then structure for exception tests:
  ```csharp
  // When
  Action methodCall = () => SomeMethod(parameters);

  // Then
  var exception = Should.Throw<ArgumentException>(methodCall);
  exception.Message.ShouldBe("Expected error message");
  ```
- Always run tests after changes: `dotnet test`

### BDD/Business Test Guidelines

#### Step Definition Tenses & Patterns
Follow natural language patterns where each step type has a specific grammatical role:
- **Given** = Past tense/State: "patient is registered", "service type exists"
  - Sets up preconditions and context
  - Describes the world state before the action
- **When** = Present tense/Action: "I register a patient", "I schedule an appointment"
  - Describes the action being tested
  - The behavior under test
- **Then** = Present tense/Assertion: "appointment should be scheduled"
  - Describes expected outcomes
  - Verifies the result of the action

#### Step Definition Entity Separation
- **Single Responsibility**: Each step definition class should ONLY contain steps for its own entity
- **One Entity, One Class**: Each domain entity has exactly ONE step definition class
  - ❌ BAD: Creating `GivenIRegisteredADoctor` in DoctorWorkScheduleStepDefinitions
  - ✅ GOOD: DoctorWorkScheduleStepDefinitions only contains work schedule steps
- **Cross-Feature Usage**: Features can use multiple step definition classes, but each class maintains its single responsibility
- **Separate Dispatchers**: Each step definition class has its own Dispatcher instance - this is correct and normal

#### Validation Patterns
- **Explicit Table Validation**: Use explicit business-friendly tables instead of hidden assertions
  - ✅ GOOD: `Then the registered patient should be:` with full table of expected data
  - ❌ BAD: `Then the patient should be registered with the correct data` (hidden logic)
- **Business Language**: Use business column names ("First Name" not "firstName")
- **Complete Transparency**: Business stakeholders should see exactly what's being validated
- **Direct Validation**: Validate directly from tables, avoid intermediate data structures

#### Anti-Patterns to Avoid
- ❌ Cross-domain logic (appointment steps creating patients)
- ❌ Shared utilities between step definition classes
- ❌ Technical language in feature files
- ❌ God classes with multiple domain responsibilities
- ❌ Hidden validation logic in step definitions
- ❌ Generic assertions like "should have correct data"
- ❌ Technical Property/Value table structures

### Unit Test Naming Conventions
- **Test Classes**: `{ClassUnderTest}{MethodUnderTest}Tests` (e.g., `PatientRegisterTests`)
- **Test Methods**: `Given{Condition}_When{Action}_Then{ExpectedResult}`
  - Example: `GivenValidPatientData_WhenRegisterPatient_ThenIsRegisteredSuccessfully`
- **Test Organization**: Group by domain entity/aggregate in separate folders
- **Test Base**: Inherit from `TestBase` for common test setup
- **Namespace**: Follow `{Project}.Domain.UnitTests.{DomainArea}` pattern

### Naming Patterns
- **Commands**: `{Action}{Entity}Command` (e.g., `RegisterDoctorCommand`)
- **Queries**: `{Get/Find}{Entity}Query`
- **Step Definitions**: `{Entity}StepDefinitions`
- **Test Features**: `{Feature}.feature.cs`

### Development Philosophy & Workflow
1. **Evolution over Planning**: Adapt to new requirements as they emerge, don't predict patterns upfront
2. **Domain-first approach**: Start with domain models and business rules
3. **Organic pattern emergence**: Let DDD patterns emerge naturally under business pressure
4. **Simple infrastructure**: Focus on domain complexity, keep everything else naive
5. **Test-driven**: Write comprehensive tests for domain behavior
6. **Iterative development**: Document each iteration with development logs
7. Always verify changes with `dotnet test` and `dotnet build`

## Creating New Backlog Items

When adding a new feature idea to the backlog:

### Steps:
1. **Read existing backlog and iterations** - Review `docs/backlog/` and `docs/iterations/` to maintain consistent styling and tone
2. **Create backlog file**: `docs/backlog/{feature-name}.md`
3. **Use standard template**:
   ```markdown
   # {Feature Name}

   ## Background Story
   [Narrative about Dr. Smith's practice and why this feature is needed]

   ## User Stories
   1. **As a [role]**<br>
      I want to [action]<br>
      So that [benefit]

   ## Requirements
   1. [Specific requirement]
   2. [Another requirement]

   ## Current System Behavior (if applicable)
   [How the system currently works that creates the need for this feature]
   ```

### Pattern Analysis from Existing Backlog:
- **Stories are simple and relatable** - focus on Dr. Smith's practice challenges
- **Background explains the business need** - why this matters to the clinic
- **User stories follow standard format** - As a [role], I want [goal], So that [benefit]
- **Requirements are specific and testable** - clear acceptance criteria
- **Roles**: receptionist (Sarah), clinic administrator, doctors by name

## Feature Implementation Workflow

When implementing a new feature from the backlog:

### ALWAYS start with these steps:
1. **Create feature branch**: `git checkout -b {feature-name}`
2. **Create next iteration folder**: `docs/iterations/{next-number}-{feature-name}/`
3. **Move backlog item**: Take the selected item from backlog and place it in the new iteration
4. **Create development log**: Set up EMPTY development log file in the iteration folder with only the title "# Development Log - {Feature Name}". NEVER pre-fill content - the log will be filled during implementation only when explicitly instructed by the user.
5. **Update clinic-overview.md**: Mark the feature as implemented
6. **Update README.md**: Add link to the new iteration folder (use pattern `docs/iterations/{number}-{name}/` NOT the specific file)

## Specific Preferences
- Use `Dispatcher` pattern for command/query execution
- Prefer `DateOnly` and `TimeOnly` for date/time handling
- Use table-driven tests in Reqnroll scenarios
- Follow existing patterns for new features (examine existing code first)

## Commands to Remember
- Build: `dotnet build`
- Test: `dotnet test`
- Run specific test: `dotnet test --filter "TestName"`
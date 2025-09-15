# Business Tests Guidelines

## Core Principles

### Tenses

BDD follows natural language patterns where each step type has a specific grammatical role:

- **Given** = Past tense/State: "patient is registered", "service type exists"
  - Sets up preconditions and context
  - Describes the world state before the action
- **When** = Present tense/Action: "I register a patient", "I schedule an appointment"
  - Describes the action being tested
  - The behavior under test
- **Then** = Present tense/Assertion: "appointment should be scheduled"
  - Describes expected outcomes
  - Verifies the result of the action

### Step Patterns

**Setup (Given):**
```csharp
[Given("patient {string} {string} is registered")]
[Given("healthcare service type {string} exists")]
```

**Actions (When):**
```csharp
[When("I register a patient {string}")]
[When("I schedule an appointment")]
```

**Assertions (Then):**
```csharp
[Then("the patient should be registered successfully")]
[Then("the appointment should be scheduled successfully")]
[Then("the appointment should fail to be scheduled")]
```

### Responsibility Boundaries

**Step Definition Classes:**
- One step definition class per domain entity
- Each class handles only its own domain logic
- Patient steps → Only patient operations
- Appointment steps → Only appointment operations
- No shared utilities between domains

**Features vs Step Definitions:**
- Features can use multiple step definition classes based on what they test
- Example: `ScheduleAppointment.feature` uses both `RegisterPatientStepDefinitions` (for setup) and `ScheduleAppointmentStepDefinitions` (for the main action)
- Each step definition class maintains its single responsibility regardless of which features use it

## Anti-Patterns

❌ Cross-domain logic (appointment steps creating patients) \
❌ Shared utilities between step definition classes \
❌ Technical language in feature files \
❌ God classes with multiple domain responsibilities
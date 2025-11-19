# Election Service

## What service is this?

Election Service manages elections, campaigns, candidates and timing of voting.

## What is it doing?

- Creates and configures elections
- Manages candidates
- Controls election status (Draft → Active → Ended)
- Defines voting start and end times
- Notifies other services when elections begin or end

## Why we need it?

Without this service:

- Elections cannot be opened or closed
- Vote validation cannot be performed
- No way to control candidates or deadlines

It provides the core structure for the entire voting process.

## What services he communicates with?

- **Vote Casting Service** — checks if elections are active
- **Tally Service** — informs when elections are finished
- **Audit Service** — logs election-related actions

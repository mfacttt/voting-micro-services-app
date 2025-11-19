# Vote Casting Service

## What service is this?

Vote Casting Service is responsible for accepting votes from authenticated voters.

## What is it doing?

- Accepts votes from users
    - Validates:
        - JWT (via API Gateway)
        - Eligibility (via Voter Registry Service)
        - Election status (via Election Service)
    - Stores each vote as immutable data
    - Publishes `VoteCasted` events to the message bus

## Why we need it?

This is the heart of the entire system:

- Only this service can accept votes
    - Ensures that each voter votes once
    - Prevents voting outside allowed time
    - Supports event-driven counting in Tally Service

Without it, the election cannot function at all.

## What services he communicates with?

- **Identity Service** — JWT validation (via Gateway)
    - **Voter Registry Service** — verifying voter rights
    - **Election Service** — checking election status
    - **Tally Service** — sends vote events
    - **Audit Service** — logs all vote attempts

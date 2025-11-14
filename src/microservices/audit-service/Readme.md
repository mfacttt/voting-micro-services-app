# Audit Service

## What service is this?

Audit Service is a central logging system that records all critical actions and events.

## What is it doing?

- Receives events from all services
- Stores logs for:
    - user authentication
    - election creation/updates
    - vote casting attempts
    - final tally results
- Works asynchronously through message queues
- Maintains immutable audit trail for investigations

## Why we need it?

To ensure:

- Transparency
- Accountability
- Full traceability of actions
- Compliance with election standards

Without the Audit Service, the system would not be verifiable.

## What services he communicates with?

Receives events from:

- **Identity Service**
- **Voter Registry Service**
- **Election Service**
- **Vote Casting Service**
- **Tally Service**

(Does not call any services itself)

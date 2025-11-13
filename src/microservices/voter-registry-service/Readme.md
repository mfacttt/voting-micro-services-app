# Voter Registry Service

## What service is this?

This is the service that manages the official list of registered voters.

## What is it doing?

- Stores voter data (FirstName, LastName, NationalID, etc.)
- Checks voter status (Active / Suspended / Deleted)
- Verifies eligibility for voting
- Ensures one person = one voter record

## Why we need it?

This service guarantees that:

- Only real verified voters participate
- Blocked or invalid voters cannot vote
- Fraud and duplicate identities are prevented

It is the foundation of election legitimacy.

## What services he communicates with?

- **Identity Service** — linking UserId to VoterId
- **Vote Casting Service** — validates eligibility for voting

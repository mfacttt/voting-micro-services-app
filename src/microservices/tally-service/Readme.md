# Tally Service

## What service is this?

Tally Service counts votes and produces final election results.

## What is it doing?

- Subscribes to `VoteCasted` events
- Aggregates votes in real time
- Ensures idempotency (no duplicate counting)
- Produces final results when elections end

## Why we need it?

It ensures:

- Transparent, reliable vote counting
- Separation of responsibilities (casting vs counting)
- Replayable event log for audits
- Consistency even under load

Without Tally Service:

- No results
- No safe, reproducible counting
- No trust in the voting platform

## What services he communicates with?

- **Vote Casting Service** — receives vote events
- **Election Service** — receives election-end notifications
- **Audit Service** — logs tally progress and results

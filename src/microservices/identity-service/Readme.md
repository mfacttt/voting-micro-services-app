# Identity Service

## What service is this?

Identity Service — сервис аутентификации и авторизации пользователей.

## What is it doing?

- Registers users (Admin, ElectionAdmin, Voter)
- Issues JWT & Refresh tokens
- Validates credentials and user sessions
- Handles login / logout flows
- Validates JWT on every request (via API Gateway)
- Manages roles and access control

## Why we need it?

Identity ensures platform security and separates access levels:

- Only verified voters can vote
- Only admins can create/edit elections
- Only authorized users can access protected data

Without Identity Service:

- No secure authentication
- No role separation
- No safe communication between services

## What services he communicates with?

- **API Gateway** — JWT validation
- **Voter Registry Service** — linking User ↔ Voter
- **All other services** indirectly via token validation

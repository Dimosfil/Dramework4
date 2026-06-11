# Feature Workflow Contract Template

Use this template for non-trivial features whose behavior needs to survive
future edits, chat resets, sprint splits, or task-manager handoffs.

Do not store secrets, credentials, private production data, large logs, or full
diffs here.

## Feature

Name:

Idea:

Functional description:

Out of scope:

## Workflow Contract

Entry points:

Required order:

Branches and states:

- Loading:
- Empty:
- Success:
- Error:
- Cancelled:
- Retry:

Blocking work:

Background work:

Data freshness:

Observability:

User-visible guarantees:

## Implementation Plan

Affected areas:

Planned changes:

Dependencies:

Risks:

## Sprints

### Sprint 1

Goal:

Scope:

Dependencies:

Exit criteria:

Verification:

## Tasks

- [ ] Task:
  - Trace to workflow or plan:
  - Definition of done:
  - Verification:

## Verification

Manual checks:

Automated checks:

Unity checks:

Release/package checks:

## Change Log

- Date:
  - Change:
  - Workflow impact:

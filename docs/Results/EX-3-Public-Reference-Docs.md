# EX-3 — Public Reference Documentation — Completion Report

**Project:** antiphon-examples
**Phase:** EX-3 (`docs/Prompts/ANTIPHON_EXAMPLES_3_PUBLIC_REFERENCE_DOCS.md`)
**Date:** 2026-09-17

**Status: STOPPED before doing scope work.** The prompt's own "Git and working-tree discipline"
section says: *"If the checked-out base looks wrong — STOP and say so in the completion report. Do
not try to correct it with git."* That condition is met here. No files in the "Files Allowed to
Change" set were touched.

---

## 1. Summary

This phase's own Project Context states as fact: *"three real, runnable sample console projects
... (`README.md`, `PACKAGES.md` already exist and are accurate as of `EX-2`, which swapped the
samples onto the real published `nuget.org` `1.0.0` packages)"* and cites
`docs/Results/EX-2-Real-Nuget-Swap.md` as *"confirms the packages are live on nuget.org at `1.0.0`
under evaluation mode"* as a source to ground "How It Was Tested"/"Licensing" claims in.

Neither is true of the branch this session was dispatched against:

- `docs/Results/EX-2-Real-Nuget-Swap.md` **does not exist** anywhere in `main`'s (or `origin/main`'s)
  history.
- The current `README.md`/`PACKAGES.md`/`NuGet.config` on `main` still describe the **pre-EX-2,
  local-feed, not-yet-published-to-nuget.org** state — the exact state EX-2 was supposed to have
  replaced.

The real EX-2 work exists, but only on an **unmerged side branch/worktree**
(`dispatch/antiphon-examples-2-real-nuget-swap`, commit `1a9a0f1`, and the still-present worktree
`.claude/worktrees/ex2-nuget-swap`), which does contain a genuine `docs/Results/EX-2-Real-Nuget-Swap.md`
and the corresponding `README.md`/`PACKAGES.md`/`NuGet.config`/`.csproj` swap to real `nuget.org`
`1.0.0` packages. That work was never merged into `main` (or `origin/main`).

Concretely, on `main`:
```
c4457f5  Merge pull request #1 (EX-1)
470ea0e  wolfgang-tools: add dispatch prompt ANTIPHON_EXAMPLES_2_REAL_NUGET_SWAP.md
e6942a7  wolfgang-tools: add dispatch prompt ANTIPHON_EXAMPLES_3_PUBLIC_REFERENCE_DOCS.md   <- local main tip
```
`1a9a0f1` (the actual EX-2 commit, with the real content described above) is **not** an ancestor of
`e6942a7`/`main` — it sits on the separate `dispatch/antiphon-examples-2-real-nuget-swap` branch
only. `origin/main` is at `470ea0e`, one commit further behind even the local `main` tip.

Since every one of this phase's five documents was scoped to be grounded in "this repo's own
existing content" — specifically `README.md`'s Release posture section and
`docs/Results/EX-2-Real-Nuget-Swap.md`'s nuget.org confirmation for the Licensing/How-It-Was-Tested
docs — writing them now would mean grounding public-facing claims either in content that doesn't
exist on this branch, or in stale pre-EX-2 content the prompt explicitly told this session not to
treat as current. Rather than guess which state is "real" (merge EX-2 myself, assume it'll land
before this phase merges, or write the docs against the stale state and risk shipping a doc that
says "evaluation mode via nuget.org 1.0.0" next to a README that still tells readers to build a
private sibling repo and populate a local feed), this session stopped per the prompt's own explicit
instruction for exactly this situation.

## 2. Safety checks / pre-flight inspection

- Read `README.md`, `PACKAGES.md` (both at the dispatched branch's tip).
- Attempted to read `docs/Results/EX-2-Real-Nuget-Swap.md` per the prompt's "Read Before Changing"
  list — **file does not exist** on this branch. This was the first signal something was wrong.
- Cross-checked `git log`, `git branch -a`, and the existing `.claude/worktrees/ex2-nuget-swap`
  worktree to confirm the EX-2 work is real but unmerged, not simply missing/never-done — see §1.
- Read `docs/Results/EX-1-Console-Samples.md` for background; no discrepancy found there.

No destructive or history-moving git commands were run. No files outside `docs/Results/` (this
report) were created or modified.

## 3. Files changed

- `docs/Results/EX-3-Public-Reference-Docs.md` (this file — new).

No other files were touched.

## 4. Files not changed

All five new documents and `README.md`, from the "Files Allowed to Change" set, were **not
created/modified** — see §1 for why. Writing them accurately requires first knowing which
release-posture state (pre-EX-2 local-feed, or post-EX-2 real nuget.org `1.0.0`) is the one
`main` will actually be in when this phase's own changes land, and that is exactly the fact this
session cannot currently determine from the dispatched branch.

## 5. Documentation changes

None made, for the reasons above. `README.md`'s Scope §6 link-adding work was not started, since it
depends on the five documents existing first.

## 6. Behavior and contract impact

None — no code was touched, and no documentation was written either.

## 7. Database impact

Not applicable.

## 8. Verification results

Verification Steps §1 (grep for leaked internal terms / bad links) and §2 (README link check) do
not apply — no documents were written, so there is nothing to grep or check.

## 9. Review outcome

**Review skipped: no diff exists to review.** No Codex review was run, since the only change in
this session is this report itself, and the Post-Completion Review section's scope ("Fix only
findings with a concrete failure scenario ... an internal-process leak, a fabricated claim, or a
broken link") presumes the five documents and `README.md` edit already exist.

## 10. Out-of-scope items discovered — flag but do not implement

- **The dispatched branch's base is stale relative to this prompt's own stated project context.**
  `docs/Results/EX-2-Real-Nuget-Swap.md` does not exist on `main`/`origin/main`; the real EX-2 work
  (commit `1a9a0f1`) exists only on the unmerged `dispatch/antiphon-examples-2-real-nuget-swap`
  branch / `.claude/worktrees/ex2-nuget-swap` worktree. Someone needs to either merge that branch
  into `main` first, or explicitly decide EX-3 should be grounded against the current (pre-EX-2)
  `main` state instead — this is a human call, not one this session should make silently by picking
  one and writing docs that might immediately go stale again.
- Every claim this phase's Scope section wanted grounded in `docs/Results/EX-2-Real-Nuget-Swap.md`
  specifically (nuget.org `1.0.0` publication, "evaluation mode is what's live today" framed around
  the real package feed rather than the local one) is impossible to ground truthfully on this
  branch as checked out, for the reason above. This is not a case of the underlying fact being
  unavailable anywhere — it's available on the unmerged branch — just not on the branch this session
  was told to work from.

## 11. Suggested follow-up tasks

- Merge `dispatch/antiphon-examples-2-real-nuget-swap` (or otherwise land EX-2's real changes) into
  `main` before re-dispatching EX-3, so the base branch actually matches what this prompt's Project
  Context describes.
- Once that's done, re-run this EX-3 prompt (or a corrected version of it) against the now-current
  `main`.
- Consider whether the `.claude/worktrees/ex2-nuget-swap` worktree (still present, locked, holding
  the real EX-2 commit) should be the merge source, or whether `dispatch/antiphon-examples-2-real-nuget-swap`
  should go through a normal PR — Wolfgang's call, not this session's.

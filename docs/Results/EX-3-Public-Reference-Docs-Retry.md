# EX-3 — Public Reference Documentation: Completion Report

**Project:** antiphon-examples
**Phase:** EX-3
**Date:** 2026-09-17

---

## 1. Summary

Authored five new, genuinely public-safe reference documents in `docs/` — `How It Works`, `How It
Was Tested`, `How To Run Examples`, `Licensing`, `Support Terms` — for an external developer
evaluating the Antiphon SDK, and updated `README.md` to link to all five prominently. These are
fresh, customer-facing replacements for the five links the NuGet package README currently points at
inside the private `Elindryn/Antiphon` repository (which 404s/access-denies for real customers);
they are not copies of those private docs. Every claim is grounded in this repo's own existing
content — `README.md`, `PACKAGES.md`, `docs/Results/EX-1-Console-Samples.md`,
`docs/Results/EX-2-Real-Nuget-Swap.md`, and the three sample projects' own `README.md` files — or
genuinely public information already referenced in this repo (the nuget.org listing, the
`clockworkotterfoundry.com/antiphon/` URL, the veraPDF project).

## 2. Safety checks / pre-flight inspection

Read all "Read Before Changing" files before writing anything: `README.md`, `PACKAGES.md`,
`docs/Results/EX-1-Console-Samples.md`, `docs/Results/EX-2-Real-Nuget-Swap.md`, and all three
sample projects' own `README.md` files (`01-generate-xrechnung/`, `02-validate-peppol/`,
`03-facturx-pdf/`). Confirmed the working tree was clean before starting and that all edits stayed
inside the "Files Allowed to Change" set.

## 3. Files changed

Added:
- `docs/Antiphon-How-It-Works.md`
- `docs/Antiphon-How-It-Was-Tested.md`
- `docs/Antiphon-How-To-Run-Examples.md`
- `docs/Antiphon-Licensing.md`
- `docs/Antiphon-Support-Terms.md`

Updated:
- `README.md` — moved the "Getting started"/"Running the samples" instructions into the new
  `docs/Antiphon-How-To-Run-Examples.md` (with a short pointer left in their place, so the
  walkthrough is maintained in one place, not two), and added a "More documentation" section
  linking all five new docs. The existing "Release posture" and "Licensing" sections were left
  as-is (they're accurate, short summaries; the new `docs/Antiphon-Licensing.md` is the detailed
  standalone version the package README will link to).

No deletions.

## 4. Files not changed

None from the "Files Allowed to Change" set turned out to be unnecessary — all six listed items
(five new files plus `README.md`) needed the changes described above.

## 5. Documentation changes

`README.md` was itself the primary documentation target of this phase (Scope §6) — see §3 above
for exactly what changed and why. No other documentation change was needed.

## 6. Behavior and contract impact

None. This is a documentation-only phase — no `.csproj`, `.cs`, `NuGet.config`, or `PACKAGES.md`
file was touched, and no `PackageReference` version or ID was changed.

## 7. Database impact

Not applicable — this repository has no database.

## 8. Verification results

Per the Verification Steps, grepped all five new documents for internal-process leaks and
out-of-scope links, both before and after the post-completion fix pass (§9). Both runs were clean:

```
$ grep -nE 'ANT-|SDK-|EX-|Wolfgang|Claude|Codex|dispatch' docs/Antiphon-How-It-Works.md docs/Antiphon-How-It-Was-Tested.md docs/Antiphon-How-To-Run-Examples.md docs/Antiphon-Licensing.md docs/Antiphon-Support-Terms.md
(no output — exit 1, no matches)

$ grep -nE 'docs/Prompts|docs/Results' docs/Antiphon-How-It-Works.md docs/Antiphon-How-It-Was-Tested.md docs/Antiphon-How-To-Run-Examples.md docs/Antiphon-Licensing.md docs/Antiphon-Support-Terms.md
(no output — exit 1, no matches)
```

`README.md`'s new relative links were confirmed to resolve to existing files (also independently
re-verified by the Codex review's own link-resolution check, §9): `docs/Antiphon-How-It-Works.md`,
`docs/Antiphon-How-It-Was-Tested.md`, `docs/Antiphon-How-To-Run-Examples.md`,
`docs/Antiphon-Licensing.md`, `docs/Antiphon-Support-Terms.md`.

## 9. Review outcome

Ran **one** Codex review (`codex exec`) over the full set of changes (the `README.md` diff plus
the five new files), scoped to: fabricated/unverifiable claims, broken relative links, and leaked
internal-process references. Findings:

1. **Fixed.** `docs/Antiphon-Licensing.md` originally stated "Commercial/paid licensing is
   available on inquiry," which contradicts `README.md`'s own accurate "Release posture" statement
   that the commercial EULA "still awaits a lawyer review pass before any paid license is sold" —
   a customer reading only the licensing doc could reasonably expect to be able to purchase a
   commercial license today, which isn't true. Reworded to state plainly that the commercial tier
   is "not yet available for purchase" and that inquiry is for interest/timing, not a purchase path
   — consistent with `README.md`. Also removed two unsupported claims from the same document while
   fixing this section: that evaluation mode uses "the same code paths a licensed deployment would
   use," and that there is "no time limit... baked into evaluation mode" — neither is established
   by this repo's own content, and both were adjacent to the contradiction being fixed.
2. **Recorded, not fixed** — style/negative-assertion framing, not a concrete failure scenario.
   Codex flagged that `docs/Antiphon-Support-Terms.md`'s statement that there's "no guaranteed
   response time or uptime commitment" isn't itself sourced from repo content. This is a true
   negative inference from the absence of any support-channel or SLA documentation anywhere in this
   repo (there is none) — it doesn't assert a positive fact the repo doesn't support, and it's
   exactly the "no SLA implied" framing the phase's own Scope section (§5) asked for. Left as-is.

Re-ran both verification greps (§8) after the fix — still clean.

## 10. Out-of-scope items discovered

- Every claim in the Scope section was traceable to real, existing repo content or genuinely
  public information (the `clockworkotterfoundry.com/antiphon/` URL already in `README.md`, the
  nuget.org package listing, the public veraPDF project). Nothing in Scope had to be dropped for
  lack of grounding.
- One nuance worth flagging: `PACKAGES.md`'s rule-set coverage list includes CEN/EN 16931, Peppol
  BIS UBL/CII, and XRechnung UBL/CII rule packages, but this repo's own three samples only actually
  *exercise* CEN + Peppol UBL validation (Sample 2) — none of them run XRechnung or CII validation,
  or Factur-X *validation* (only Factur-X *generation*, Sample 3). `docs/Antiphon-How-It-Works.md`
  describes the SDK's package-level coverage (accurately, per `PACKAGES.md`) but is careful to
  point to `PACKAGES.md` rather than implying every listed capability is demonstrated by a runnable
  sample in this repo — worth keeping in mind if a future phase adds samples that actually exercise
  the currently-undemonstrated paths (XRechnung/CII validation).
- No contact method other than `https://clockworkotterfoundry.com/antiphon/` exists anywhere in
  this repo's own content, so all five new docs route every "get in touch" / "learn more" pointer
  to that single URL. No separate support-specific contact channel was invented.

## 11. Suggested follow-up tasks

- The Antiphon-side follow-up phase (`ANT-SDK-11`, per the dispatching prompt) to repoint the
  package README's five doc links at these new documents, once this phase is reviewed and merged.
- Once XRechnung/CII validation or Factur-X validation get their own runnable samples in this repo,
  revisit `docs/Antiphon-How-It-Works.md` and `docs/Antiphon-How-It-Was-Tested.md` to cite those
  samples directly rather than only pointing to `PACKAGES.md`'s package-level coverage.
- Once the commercial EULA clears legal review and a paid tier actually becomes purchasable,
  `docs/Antiphon-Licensing.md`'s "Commercial licensing" section will need a follow-up edit — it
  currently states, correctly, that the paid tier isn't available yet.

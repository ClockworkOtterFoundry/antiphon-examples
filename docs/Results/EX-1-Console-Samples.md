# EX-1 — Three Runnable Console Samples — Completion Report

**Phase:** EX-1 (`docs/Prompts/ANTIPHON_EXAMPLES_1_CONSOLE_SAMPLES.md`)
**Repo:** `antiphon-examples`
**Date:** 2026-09-14

---

## 1. Summary

Built three real, runnable, standalone .NET console projects turning `docs/Antiphon-Getting-Started.md`'s
three scenarios (private `Antiphon` repo) into `dotnet run`-able samples, plus a root `README.md`
and a from-scratch `PACKAGES.md`. **All three samples build and run successfully against real
packed NuGet output**, producing exactly the documented output (see §6).

## 2. Local-feed setup

Followed the Precondition's **preferred, self-contained copy approach**, with one necessary
deviation:

- `../Antiphon`'s packed output (Sep 2) predated several `src/` commits (the ANTIPHON_STD-series
  standards-reconciliation work and the B1b strong-name key-ceremony finalization), so I **re-ran
  `tools/pack-release.sh`** against current `main` (timestamp `2026-09-14T00:00:00Z`) rather than
  reusing the stale packed output, to be sure the samples run against what the code actually does
  today, not a nine-day-old snapshot. The real production `.snk` at
  `~/.antiphon-secrets/AntiphonStrongName.snk` was used, per `docs/Antiphon-Packaging.md`'s
  documented custody.
  - **Deviation, worth flagging:** the prompt's `AntiphonStrongNameKeyFile=... tools/pack-release.sh`
    invocation as written did not actually reach `dotnet.exe` in this environment — `dotnet` here
    resolves to a WSL-interop symlink to Windows `dotnet.exe`, and WSL bash environment variables
    aren't visible across that boundary without `WSLENV`. I worked around it with
    `export WSLENV="$WSLENV:AntiphonStrongNameKeyFile/p"` before invoking the script. This is an
    environment quirk of this particular dev machine, not a defect in `pack-release.sh` itself —
    noted here in case another WSL-hosted session hits the same silent-looking `ANTI0801` failure.
  - Re-packing surfaced the real, current package count: **19**, not 18 (see §5).
- Copied all 19 packages' `.nupkg` + `.snupkg` (38 files) from
  `../Antiphon/src/*/bin/Release/` into `antiphon-examples/local-feed/`, gitignored.
- `NuGet.config` at the repo root defines a `local-feed` package source pointing at the relative
  `local-feed` folder (plus `nuget.org`, kept for the samples' own future non-Antiphon needs — none
  currently exist, but there's no reason to `<clear/>` it away entirely). No dependency on a sibling
  `../Antiphon` checkout existing at build/run time — only at local-feed-population time, which the
  README documents honestly as a private-repo-access precondition.

## 3. Files changed (from-scratch repo — all additions)

```
.gitignore
NuGet.config
README.md
PACKAGES.md
tools/README.md
tools/make-sample-pdfa.py
tools/sRGB-v2-micro.icc
tools/sample-input-verapdf-1b-report.xml
01-generate-xrechnung/01-generate-xrechnung.csproj
01-generate-xrechnung/Program.cs
01-generate-xrechnung/README.md
02-validate-peppol/02-validate-peppol.csproj
02-validate-peppol/Program.cs
02-validate-peppol/README.md
03-facturx-pdf/03-facturx-pdf.csproj
03-facturx-pdf/Program.cs
03-facturx-pdf/README.md
03-facturx-pdf/sample-input.pdf
docs/Results/EX-1-Console-Samples.md   (this file)
```

`local-feed/*.nupkg` / `*.snupkg` (38 files) exist on disk but are gitignored, not committed, per
the prompt's explicit instruction.

Scenario 2 uses option **(a)** from the prompt (self-contained "generate then validate" in memory)
rather than a static `sample-invoice.xml` — no such file exists; the folder structure differs from
the prompt's literal scaffold sketch in exactly that one respect, as the prompt anticipated.

## 4. Sample PDF/A provenance (`sample-input.pdf`)

**Verified, not assumed.** `03-facturx-pdf/sample-input.pdf` is a minimal, single-page, hand-built
PDF (a couple of filled rectangles, no text/fonts so no font-embedding question arises, a proper
sRGB `OutputIntent`, and synchronized Info/XMP metadata including the `pdfaid` identification).
Built by `tools/make-sample-pdfa.py` (no external PDF-authoring library) using the 456-byte
`sRGB-v2-micro.icc` profile from
[saucecontrol/Compact-ICC-Profiles](https://github.com/saucecontrol/Compact-ICC-Profiles) (CC0).

Validated with **veraPDF 1.30.2** (already installed locally at `~/verapdf`) against the PDF/A-1b
profile: **`isCompliant="true"`, 129/129 rules passed, 0 failed**. Full report committed at
`tools/sample-input-verapdf-1b-report.xml`.

As a further sanity check (not required by the prompt, but free once the sample ran successfully):
the *output* of Scenario 3, `facturx-output.pdf`, was independently validated against **PDF/A-3b**
and is also fully compliant — **146/146 rules passed, 0 failed** — matching the private repo's own
committed reference numbers for `FacturXEmbedder`'s output (`docs/Results/SDK-Phase-05-Pdf.md` §6
in `../Antiphon`).

## 5. `PACKAGES.md` — package count correction

The prompt's own refresh note assumed **18** packages (confirmed, it said, via `ls src/`) and "six
rule-set packages." Re-derived directly against `../Antiphon/src/` (and cross-checked against the
freshly-run `pack-release.sh`'s own manifest output, "19 packages built successfully"), the real,
current count is **19**. The six-rule-set-package framing was correct on its own terms; what the
count missed was `Antiphon.Rules.Ir` (the shared IR/JSON-serialization package those six compile
against — not a rule set itself, but still a 19th shippable package). `PACKAGES.md` documents this
correction explicitly at the top rather than silently reconciling it.

No other structural surprises: the grouping (core model / rule compilation output / validation
engine / generation / parsing / PDF-Factur-X / licensing / XSD schema layer) held up against the
real dependency graph read from each package's own `.csproj`.

## 6. Verification results (exact `dotnet run` output)

```
$ cd 01-generate-xrechnung && dotnet run
Wrote invoice.xml (4233 bytes)
Generation is deterministic: running this again produces byte-identical output.

$ cd ../02-validate-peppol && dotnet run
Generated a Peppol BIS Billing 3.0 UBL invoice in memory.

Well-formedness: WellFormed
Schema: Valid
Validated against cen-ubl validation-1.3.16 (ConnectingEurope/eInvoicing-EN16931, EUPL-1.2)
Validated against peppol-ubl v3.0.20 (OpenPEPPOL/peppol-bis-invoice-3, Apache-2.0 (per repository))

$ cd ../03-facturx-pdf && dotnet run
Wrote facturx-output.pdf (14648 bytes)
round-trip OK
```

Zero violations for Scenario 2, matching the getting-started doc's own claimed sample output
exactly. All three re-verified from a clean `rm -rf */bin */obj` state after the post-review fix
pass (§7), not just once before it.

## 7. Review outcome

Ran **one** Codex review (`codex exec review --uncommitted`) over the full uncommitted diff. Two
findings, both with concrete failure scenarios — both fixed:

1. **Fixed.** `net8.0`-targeted samples would fail to run on a machine with *only* the .NET 10 SDK
   installed (the README's own stated Prerequisite: ".NET 8 **or** .NET 10 SDK" implies either
   alone suffices) — .NET's default roll-forward policy doesn't cross major versions, so a bare
   .NET 10 install with no .NET 8 runtime present would hit `FrameworkNotFoundException` at
   startup. Added `<RollForward>LatestMajor</RollForward>` to all three `.csproj` files so the
   samples run on whichever major runtime (8 or 10) is actually present. Re-verified all three
   samples still produce identical output after the fix (§6).
2. **Fixed.** The README's local-feed setup instructions `cp`'d into `local-feed/` without first
   creating it — since the folder is gitignored, a fresh clone doesn't have it, so the documented
   `cp` commands would fail with "No such file or directory." Added `mkdir -p local-feed` before
   the `cp` lines.

No findings were style/naming/"consider refactoring" — nothing was recorded-but-not-fixed.

## 8. Bugs/inaccuracies found in `docs/Antiphon-Getting-Started.md`

**One, worth flagging back to Wolfgang (not fixed here — no write access to `../Antiphon`):**
Scenario 1's fabricated `ElectronicAddress` scheme (`"EM"`) is valid for XRechnung but **not** on
Peppol's actual EAS (Electronic Address Identifier Scheme) codelist as compiled into
`Antiphon.Rules.Peppol.Ubl` — running the real rule set against it fires
`PEPPOL-EN16931-CL008` ("Electronic address identifier scheme must be from the codelist..."), even
though `docs/Antiphon-Rule-Reference.md`'s own `BR-CL-25` entry lists `EM` as an example of a valid
EAS scheme. This only surfaces when Scenario 1's fabricated data is reused for a *Peppol* validation
scenario (i.e., building Scenario 2 exactly as instructed — reuse Scenario 1's data, swap in the
Peppol `CustomizationID`). I resolved it in this sample by using scheme `0204` (DE Leitweg-ID,
confirmed present in the compiled Peppol UBL rule set's actual EAS codelist) instead of `EM` for
both parties' electronic addresses in `02-validate-peppol/Program.cs`. Two things worth
Wolfgang/Antiphon-team attention: (a) whether `BR-CL-25`'s doc example of `EM` as a valid EAS code
is simply wrong/stale relative to the real Peppol codelist, and (b) whether the getting-started
doc's Scenario 1 data being non-Peppol-EAS-valid is worth a note, since a reader who naively reuses
it for their own Peppol validation (as this phase's prompt explicitly asked for) hits the same
surprise.

## 9. Suggested follow-up tasks

- Swap the local NuGet feed for real `nuget.org` `PackageReference` versions once Antiphon
  publishes (removes `NuGet.config`'s local source and the `local-feed/` setup step entirely).
- Once EAS-codelist accuracy in `docs/Antiphon-Rule-Reference.md` (see §8) is resolved upstream,
  consider whether `02-validate-peppol/Program.cs`'s scheme-code comment needs updating to match.
- `Antiphon.Rules.Cen.Cii`, `Antiphon.Rules.Peppol.Cii`, `Antiphon.Rules.XRechnung.{Cii,Ubl}`,
  `Antiphon.Parsing.{Ubl,Cii}`, and `Antiphon.Xsd.Cii` are all "not needed for these samples" per
  `PACKAGES.md` — a natural fourth sample (CII validation, or UBL⇄CII conversion) would exercise
  them, if a future phase wants broader package coverage than these three scenarios provide.
- Consider a CI workflow (GitHub Actions) that runs all three samples' `dotnet run` against a
  restored local feed on every push, once real nuget.org packages exist and a CI machine doesn't
  need private-repo access to populate `local-feed/`.

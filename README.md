# antiphon-examples

Three runnable console samples for evaluating the [Antiphon](https://clockworkotterfoundry.com/antiphon/)
SDK — a typed EN 16931 electronic invoice model for .NET (generate, validate, and convert
XRechnung, Peppol BIS Billing 3.0, and Factur-X/ZUGFeRD invoices natively, without a Java sidecar).
Clone this, run `dotnet run` in any of the three folders below, and see the documented output for
yourself — not just read code in a markdown file.

- **`01-generate-xrechnung/`** — generate an XRechnung UBL invoice.
- **`02-validate-peppol/`** — generate and validate a Peppol BIS Billing 3.0 invoice.
- **`03-facturx-pdf/`** — produce a Factur-X PDF (CII XML embedded into a PDF/A).

Each scenario runs entirely offline, in **evaluation mode** — no license key required, full
function, nothing sabotaged. See the [licensing note](#licensing) below.

## Prerequisites

- .NET 8 or .NET 10 SDK.

## Local-feed setup (read this first — it's the one non-obvious step)

Antiphon's NuGet packages are **not published to nuget.org yet** (blocked on EULA review and a
standards-completion decision — see [Release posture](#release-posture) below). So these samples
can't literally `dotnet restore` from nuget.org today. Instead:

1. You need access to the private `Antiphon` repository (source-available to Antiphon evaluators
   and customers under a separate arrangement — this public repo does not grant that access by
   itself). Build it and run its packing script:
   ```bash
   AntiphonStrongNameKeyFile=<path to the real .snk> \
     tools/pack-release.sh 2026-09-14T00:00:00Z
   ```
   This produces `.nupkg`/`.snupkg` files under each `src/<Project>/bin/Release/` in that repo.
2. Copy those files into this repo's own `local-feed/` folder (gitignored — never committed, since
   built binaries don't belong in a public git history; create it if it doesn't exist yet):
   ```bash
   mkdir -p local-feed
   cp ../Antiphon/src/*/bin/Release/*.nupkg local-feed/
   cp ../Antiphon/src/*/bin/Release/*.snupkg local-feed/
   ```
3. `NuGet.config` at this repo's root already points a `local-feed` package source at that folder —
   nothing else to configure. `dotnet run` in any of the three sample folders will restore from it.

**If you don't have access to the private `Antiphon` repository:** you can still read every
sample's source and see exactly what it does, but you can't currently build or run it yourself.
That's an honest, temporary limitation, not something this README is trying to paper over — once
real packages are published to nuget.org, this whole local-feed step goes away, `NuGet.config`'s
local source gets removed, and each sample's `PackageReference` versions get pinned to the real
published version instead (see [Suggested follow-ups](#suggested-follow-ups) in this phase's result
report, `docs/Results/EX-1-Console-Samples.md`).

## Running the samples

```bash
cd 01-generate-xrechnung && dotnet run
cd ../02-validate-peppol && dotnet run
cd ../03-facturx-pdf && dotnet run
```

Each folder's own `README.md` documents exactly what it does and its expected output.

## Which package(s) do you need?

See [`PACKAGES.md`](PACKAGES.md) — a map of all 19 Antiphon NuGet packages, grouped by concern,
with a "what do I reference for..." quick-answer table for generate-only, validate-only, both,
Factur-X/PDF production, and provenance/version exposure.

## Release posture

Antiphon's SDK, parser, and packaging are feature-complete, and these three samples run against
real packed NuGet output (see Local-feed setup above) — this is not vaporware. But the product as a
whole is **not yet published to nuget.org**, pending EULA legal review and a standards-completion
decision (finishing Peppol 3.0.21 + Factur-X/France standards reconciliation, or shipping an
older-baseline release with that limitation documented). Don't read "these samples run" as "go
install Antiphon from nuget.org today" — it isn't there yet. Current status:
<https://clockworkotterfoundry.com/antiphon/>.

## Licensing

These samples run with `licenseKey: null` throughout — Antiphon's evaluation mode, full function,
no sabotage of output, no key required. See <https://clockworkotterfoundry.com/antiphon/> for
license terms and how to acquire a key once you're ready to move past evaluation.

This repository's own code (the three sample projects, this README, `PACKAGES.md`) is MIT-licensed
— see `LICENSE`. Antiphon itself is proprietary, closed-source commercial software; these samples
merely consume its public API surface.

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

## Getting started

Antiphon's NuGet packages are published to nuget.org at version `1.0.0`. Clone this repo, then:

```bash
dotnet restore 01-generate-xrechnung
dotnet run --project 01-generate-xrechnung
```

Packages resolve straight from nuget.org — no private repo access, no local package feed, no
sibling checkout of the `Antiphon` repository required.

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

Antiphon `v1.0.0` **is published to nuget.org** and these three samples restore and run against
those real, published packages — this is not vaporware. Packages run in **evaluation mode**: no
license key required, full function, notice-only, nothing sabotaged. The full commercial EULA still
awaits a lawyer review pass before any paid license is sold, and standards-completion work (Peppol
3.0.21, Factur-X/France reconciliation) is still in progress for the full baseline. Don't read
"published to nuget.org" as "the paid commercial tier is available" or "full standards conformance
is complete" — neither is true yet. Current status:
<https://clockworkotterfoundry.com/antiphon/>.

## Licensing

These samples run with `licenseKey: null` throughout — Antiphon's evaluation mode, full function,
no sabotage of output, no key required. See <https://clockworkotterfoundry.com/antiphon/> for
license terms and how to acquire a key once you're ready to move past evaluation.

This repository's own code (the three sample projects, this README, `PACKAGES.md`) is MIT-licensed
— see `LICENSE`. Antiphon itself is proprietary, closed-source commercial software; these samples
merely consume its public API surface.

# How To Run The Examples

This repository contains three real, runnable .NET console samples for evaluating the Antiphon SDK:

- **`01-generate-xrechnung/`** — generate an XRechnung UBL invoice.
- **`02-validate-peppol/`** — generate and validate a Peppol BIS Billing 3.0 invoice.
- **`03-facturx-pdf/`** — produce a Factur-X PDF (CII XML embedded into a PDF/A).

Each scenario runs entirely offline, in **evaluation mode** — no license key required, full
function, nothing sabotaged. See [Licensing](Antiphon-Licensing.md) for what that means.

## Prerequisites

- .NET 8 or .NET 10 SDK.

## Getting started

Antiphon's NuGet packages are published to nuget.org at version `1.0.0`. Clone this repo, then:

```bash
dotnet restore 01-generate-xrechnung
dotnet run --project 01-generate-xrechnung
```

Packages resolve straight from nuget.org — no private repo access, no local package feed, no
sibling checkout of any other repository required.

## Running the samples

```bash
cd 01-generate-xrechnung && dotnet run
cd ../02-validate-peppol && dotnet run
cd ../03-facturx-pdf && dotnet run
```

Each folder's own `README.md` documents exactly what it does and its expected output.

## Which package(s) do you need?

See [`PACKAGES.md`](../PACKAGES.md) — a map of all of Antiphon's NuGet packages, grouped by
concern, with a "what do I reference for..." quick-answer table for generate-only, validate-only,
both, and Factur-X/PDF production.

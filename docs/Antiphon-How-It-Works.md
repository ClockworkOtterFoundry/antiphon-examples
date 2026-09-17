# How Antiphon Works

Antiphon is a typed .NET model for EN 16931-family electronic invoices. It lets you generate,
validate, and convert e-invoices natively — no Java sidecar, no shelling out to an external
validator process.

## What it validates and generates

Based on this repo's own three runnable samples and the package map in [`PACKAGES.md`](../PACKAGES.md),
Antiphon covers:

- **EN 16931 core** — the CEN/TC 434 semantic data model that every EU e-invoicing profile below is
  built on.
- **Peppol BIS Billing 3.0** — UBL and CII syntax bindings.
- **XRechnung** — UBL and CII syntax bindings (Germany's B2G profile).
- **CEN/EN 16931 baseline rules** — UBL and CII syntax bindings, independent of any national or
  network-specific profile layered on top.
- **Factur-X / ZUGFeRD** — generating UN/CEFACT CII XML and embedding it into a PDF/A-3 container
  (the Franco-German hybrid PDF+XML invoice format).

Generation and validation are independent capabilities — you can generate a document without
validating it, or validate a document you already have without ever generating anything.

## How the packages are organized

Antiphon ships as a set of focused NuGet packages rather than one monolithic library, so you only
pull in what a given scenario needs: a typed invoice model, syntax generators/parsers (UBL, CII), a
Schematron-based validation engine, per-standard compiled rule sets, pinned XSD schemas, and a
Factur-X PDF container layer.

See [`PACKAGES.md`](../PACKAGES.md) for the full package-by-package map, dependency graph, and a
"what do I reference for..." quick-answer table for the generate-only, validate-only, both, and
Factur-X/PDF scenarios.

## Seeing it run

This repository's three sample console projects are the fastest way to see the above in action
end to end — see [How To Run Examples](Antiphon-How-To-Run-Examples.md).

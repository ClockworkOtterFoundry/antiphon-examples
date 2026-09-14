# Antiphon package architecture

Which of Antiphon's NuGet packages do you actually reference? This map is built from the real,
current package list and each package's actual `.csproj`/`PackageReference` dependency graph (read
directly from the private Antiphon repo's `src/`, not from memory) — **19 shippable packages** as of
this writing.

> **Correction from an earlier draft of this document:** the package count assumed here was
> originally **18** (and "six `Antiphon.Rules.*` rule-set packages"). Re-derived directly against
> `../Antiphon/src/` at build time, the real count is **19** — the six rule-set packages
> (`Antiphon.Rules.Cen.{Cii,Ubl}`, `Antiphon.Rules.Peppol.{Cii,Ubl}`, `Antiphon.Rules.XRechnung.{Cii,Ubl}`)
> figure was correct, but `Antiphon.Rules.Ir` (the shared IR framework package those six compile
> against, not a rule set itself) had been left out of the earlier count. This document reflects the
> corrected, verified count.

## Quick reference table

| Package | Purpose | Used by these samples |
|---|---|---|
| `Antiphon` | Core typed EN 16931 invoice model (`Invoice`, `InvoiceLine`, code lists, ...). | 01, 02, 03 |
| `Antiphon.Generation.Ubl` | Generate UBL 2.1 Invoice/CreditNote XML from the model. | 01, 02 |
| `Antiphon.Generation.Cii` | Generate UN/CEFACT CII XML from the model. | 03 |
| `Antiphon.Parsing.Ubl` | Parse UBL XML back into the model (reverse of Generation.Ubl). | not needed |
| `Antiphon.Parsing.Cii` | Parse CII XML back into the model (reverse of Generation.Cii). | not needed |
| `Antiphon.Validation` | Runtime Schematron-IR evaluator (XPath 2.0-subset interpreter). | 02 (transitively, via Validation.Pipeline) |
| `Antiphon.Validation.Pipeline` | Orchestrates well-formedness → XSD → Schematron + format auto-detection. | 02 |
| `Antiphon.Rules.Ir` | Shared IR record types + JSON (de)serialization for compiled rule sets. | 02 (transitively) |
| `Antiphon.Rules.Cen.Ubl` | Compiled IR: CEN/TC 434 EN 16931 UBL Schematron (validation-1.3.16). | 02 |
| `Antiphon.Rules.Cen.Cii` | Compiled IR: CEN/TC 434 EN 16931 CII Schematron (validation-1.3.16). | not needed (CII validation is out of scope for these 3 samples) |
| `Antiphon.Rules.Peppol.Ubl` | Compiled IR: OpenPeppol BIS Billing 3.0 UBL Schematron (v3.0.20). | 02 |
| `Antiphon.Rules.Peppol.Cii` | Compiled IR: OpenPeppol BIS Billing 3.0 CII Schematron (v3.0.20). | not needed |
| `Antiphon.Rules.XRechnung.Ubl` | Compiled IR: KoSIT XRechnung 3.0.2 Schematron, UBL binding (2.6.0 + 2.5.0 side by side). | not needed (Scenario 1 only generates, doesn't validate) |
| `Antiphon.Rules.XRechnung.Cii` | Compiled IR: KoSIT XRechnung 3.0.2 Schematron, CII binding (2.6.0 + 2.5.0 side by side). | not needed |
| `Antiphon.Xsd.Core` | Shared `XmlSchemaSet`-loading utility for the XSD packages. | 02 (transitively) |
| `Antiphon.Xsd.Ubl` | Pinned OASIS UBL 2.1 XSD schemas (Invoice + CreditNote + transitive imports). | 02 |
| `Antiphon.Xsd.Cii` | Pinned UN/CEFACT CII D16B XSD schema. | not needed (no CII validation in these samples) |
| `Antiphon.Pdf` | Factur-X/ZUGFeRD PDF/A-3 container layer (`FacturXEmbedder`/`FacturXExtractor`). | 03 |
| `Antiphon.Licensing` | Offline license enforcement (evaluation mode, `*WithLicense` overloads). | not directly referenced by any sample, but transitively pulled in by Generation.*, Validation.Pipeline, and Pdf |

## Grouped by concern

**Core model**
- `Antiphon` — the typed invoice model everything else builds on. No dependency on any other
  `Antiphon.*` package (only `Microsoft.SourceLink.GitHub`, a build-time-only dev dependency).

**Rule compilation output, consumed at runtime**
- `Antiphon.Rules.Ir` — shared IR types + JSON (de)serialization. Depends on nothing else in the
  solution; every rule-set package below depends on it.
- Six rule-set packages, each `Antiphon.Rules.Ir`'s only runtime dependency, each compiling one
  upstream standard/binding pair:
  - `Antiphon.Rules.Cen.Ubl` / `Antiphon.Rules.Cen.Cii` — CEN/TC 434 EN 16931 Schematron (UBL / CII
    binding), from ConnectingEurope/eInvoicing-EN16931 (EUPL-1.2).
  - `Antiphon.Rules.Peppol.Ubl` / `Antiphon.Rules.Peppol.Cii` — OpenPeppol BIS Billing 3.0 Schematron
    (UBL / CII binding), from OpenPEPPOL/peppol-bis-invoice-3 (Apache-2.0, per repository).
  - `Antiphon.Rules.XRechnung.Ubl` / `Antiphon.Rules.XRechnung.Cii` — KoSIT XRechnung 3.0.2
    Schematron (UBL / CII binding), current (2.6.0) and previous (2.5.0) versions embedded side by
    side.
  - These packages are "describe-only": no execution API, just compiled IR + metadata. They mean
    nothing without the validation engine below to run them.

**Validation engine**
- `Antiphon.Validation` — the runtime evaluator: an XPath 2.0-subset interpreter over the compiled
  IR, producing typed violations and SVRL. Depends on `Antiphon.Rules.Ir` only.
- `Antiphon.Validation.Pipeline` — orchestration: well-formedness → XSD → Schematron, plus format
  auto-detection (root element + namespace + `CustomizationID`/guideline ID). Depends on
  `Antiphon.Rules.Ir`, `Antiphon.Validation`, `Antiphon.Xsd.Ubl`, `Antiphon.Xsd.Cii`, and
  `Antiphon.Licensing`. This is the package whose `LayeredValidator` you actually call.

**Generation**
- `Antiphon.Generation.Ubl` — model → UBL 2.1 XML (XRechnung UBL, Peppol BIS Billing 3.0 UBL).
  Depends on `Antiphon` and `Antiphon.Licensing`.
- `Antiphon.Generation.Cii` — model → UN/CEFACT CII XML (the syntax behind Factur-X/ZUGFeRD).
  Depends on `Antiphon` and `Antiphon.Licensing`.

**Parsing**
- `Antiphon.Parsing.Ubl` / `Antiphon.Parsing.Cii` — the reverse direction of the two generators
  above, for UBL⇄CII conversion. Each depends only on `Antiphon`. Not used by any of these three
  samples (none of them parse an existing document).

**PDF / Factur-X**
- `Antiphon.Pdf` — the Factur-X/ZUGFeRD PDF/A-3 container layer (`FacturXEmbedder`,
  `FacturXExtractor`). Depends on `Antiphon.Licensing` and the third-party `PdfSharp` package (MIT)
  — the only third-party runtime dependency among any shippable Antiphon package, isolated here so
  it never leaks into the SDK core.

**Licensing**
- `Antiphon.Licensing` — offline license enforcement (signed keys, evaluation mode, 14-day grace
  period). Depends on no other `Antiphon.*` project. Every entry point used by these samples has a
  `*WithLicense` sibling; all three samples use the plain, non-`WithLicense` methods, which is
  exactly `licenseKey: null` / `LicenseStatus.Evaluation` — full function, no key required.

**XSD schema layer**
- `Antiphon.Xsd.Core` — shared `XmlSchemaSet`-loading utility; resolves embedded, filename-keyed
  resources so relative `<xsd:import>`/`<xsd:include>` `schemaLocation`s work without touching disk.
  Depends on nothing else in the solution.
- `Antiphon.Xsd.Ubl` — pinned OASIS UBL 2.1 XSD schemas. Depends on `Antiphon.Xsd.Core`.
- `Antiphon.Xsd.Cii` — pinned UN/CEFACT CII D16B XSD schema. Depends on `Antiphon.Xsd.Core`.

## "What do I reference for..."

- **Generate only (no validation)** — `Antiphon` + whichever of `Antiphon.Generation.Ubl` /
  `Antiphon.Generation.Cii` matches your target syntax. (Scenario 1.)
- **Validate only (no generation)** — `Antiphon.Validation.Pipeline` + `Antiphon.Xsd.Ubl` and/or
  `Antiphon.Xsd.Cii` (whichever syntax you're validating) + every `Antiphon.Rules.*` package for the
  rule sets you want available (e.g. `Antiphon.Rules.Cen.Ubl` + `Antiphon.Rules.Peppol.Ubl` for
  Peppol UBL — Peppol always implies running both). You do **not** need `Antiphon.Generation.*` or
  `Antiphon.Parsing.*` if you're validating a document you already have.
- **Both (generate then validate)** — the union of the two above. (Scenario 2, in its
  self-contained "generate then validate" form.)
- **Factur-X/PDF production** — `Antiphon` + `Antiphon.Generation.Cii` + `Antiphon.Pdf`. (Scenario
  3.) Add `Antiphon.Validation.Pipeline` (+ CII rule/XSD packages) only if you also want to validate
  the extracted CII XML after round-tripping — `Antiphon.Pdf` deliberately never references
  `Antiphon.Validation.Pipeline` itself, so that wiring is the caller's choice.
- **Provenance/version exposure in your results** — `Antiphon.Validation.Pipeline`'s
  `LayeredValidator.ValidateWithSelection(xml, RuleSetCatalog.Discover(), new RuleSetSelectionOptions())`,
  which resolves the rule-set version that was in force on the invoice's own issue date (BT-2) and
  returns the full version triple (`AntiphonPackageVersion`, `ResolvedFormat`, per-rule-set
  `RulesPackageVersion`/`SourceVersion`/`UpstreamSha256`). See
  `docs/Antiphon-Getting-Started.md`'s "Selecting rules for a historical invoice" section in the
  private Antiphon repo.

## Not on nuget.org yet

None of these 19 packages are published to nuget.org yet — see the root `README.md`'s
release-posture note for why and what's blocking it. Everything in this document was verified
against the real packed `.nupkg` output built by `../Antiphon`'s own `tools/pack-release.sh` and
served locally (see `README.md`'s Local-feed setup), not from source alone.

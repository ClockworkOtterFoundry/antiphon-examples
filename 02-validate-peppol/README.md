# Scenario 2 — Validate a Peppol BIS Billing 3.0 invoice

Self-contained: generates a valid Peppol BIS Billing 3.0 UBL invoice in memory (reusing Scenario
1's fabricated invoice data with the Peppol `CustomizationID` swapped in), then validates that
in-memory XML with `RuleSetCatalog.Discover()` → `LayeredValidator.Validate` — no external file
dependency. Format auto-detection picks the CEN + Peppol rule sets from the document's own
`CustomizationID`.

Packages used: `Antiphon`, `Antiphon.Generation.Ubl`, `Antiphon.Validation.Pipeline`,
`Antiphon.Rules.Cen.Ubl`, `Antiphon.Rules.Peppol.Ubl`, `Antiphon.Xsd.Ubl`.

## Run

```bash
dotnet run
```

## Expected output

```
Generated a Peppol BIS Billing 3.0 UBL invoice in memory.

Well-formedness: WellFormed
Schema: Valid
Validated against cen-ubl validation-1.3.16 (ConnectingEurope/eInvoicing-EN16931, EUPL-1.2)
Validated against peppol-ubl v3.0.20 (OpenPEPPOL/peppol-bis-invoice-3, Apache-2.0 (per repository))
```

Zero `[Severity] RuleId: Message` lines print between "Schema: Valid" and the provenance lines —
that means zero violations against the sample data. All invoice data is fabricated — no real
company, VAT ID, or bank account.

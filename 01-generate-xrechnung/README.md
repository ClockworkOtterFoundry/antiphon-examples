# Scenario 1 — Generate an XRechnung UBL invoice

Builds a fabricated `Invoice` in memory and generates a UBL 2.1 XRechnung 3.0-profile XML document
with `Antiphon.Generation.Ubl`. Runs entirely offline, in evaluation mode — no license key.

Packages used: `Antiphon`, `Antiphon.Generation.Ubl`.

## Run

```bash
dotnet run
```

## Expected output

```
Wrote invoice.xml (**** bytes)
Generation is deterministic: running this again produces byte-identical output.
```

(exact byte count depends only on the invoice data below, and is identical on every run)

`invoice.xml` is written to the current directory. Generation is deterministic — running this
twice produces byte-identical XML, as the underlying `Antiphon` SDK guarantees.

All invoice data (`Antiphon Software GmbH`, `Musterbehörde Berlin`, IBAN, VAT IDs, etc.) is
fabricated — reused verbatim from `docs/Antiphon-Getting-Started.md`'s Scenario 1 in the private
Antiphon repo. No real company, VAT ID, or bank account appears anywhere in this sample.

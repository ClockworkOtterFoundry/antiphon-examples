# Scenario 3 — Produce a Factur-X PDF

Builds a fabricated `Invoice`, generates UN/CEFACT CII XML with `Antiphon.Generation.Cii`, embeds
it into `sample-input.pdf` as a Factur-X attachment + PDF/A-3 metadata layer with
`Antiphon.Pdf`'s `FacturXEmbedder`, writes `facturx-output.pdf`, then round-trips it with
`FacturXExtractor` to prove the embed worked.

Packages used: `Antiphon`, `Antiphon.Generation.Cii`, `Antiphon.Pdf`.

## The seed PDF (`sample-input.pdf`)

Antiphon's Factur-X layer **adds to** an already-PDF/A-conformant PDF; it does not convert an
arbitrary PDF into one (font embedding, output intents, colour spaces are a separate, out-of-scope
concern — see `docs/Antiphon-Pdf.md` in the private Antiphon repo). `sample-input.pdf` here is a
minimal, single-page, **genuinely PDF/A-1b conformant** placeholder (a couple of filled rectangles,
no text/fonts, so no font-embedding question even arises) — not a file that merely claims
conformance.

**Verified, not assumed:** validated with [veraPDF](https://verapdf.org/) 1.30.2 against the PDF/A-1b
profile — **129/129 rules passed, 0 failed** (`isCompliant="true"`). The full report is at
`../tools/sample-input-verapdf-1b-report.xml`; the generator script (hand-rolled PDF, no external
PDF-authoring library) is at `../tools/make-sample-pdfa.py`.

## Run

```bash
dotnet run
```

## Expected output

```
Wrote facturx-output.pdf (**** bytes)
round-trip OK
```

`facturx-output.pdf` is written to the current directory. The extracted CII XML matches the XML
that was embedded, byte-for-byte, so the sample prints `round-trip OK`.

All invoice data (`Antiphon Software GmbH`, `Boulangerie Dupont SARL`, IBAN, VAT IDs, etc.) is
fabricated — reused verbatim from `docs/Antiphon-Getting-Started.md`'s Scenario 3 in the private
Antiphon repo.

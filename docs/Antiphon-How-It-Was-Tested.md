# How Antiphon Was Tested

This page summarizes conformance evidence you can verify yourself, using this repository's own
samples — not vendor claims you have to take on faith.

## Standards-body rule sets, not a home-grown validator

Antiphon's validation engine doesn't reimplement e-invoicing rules from scratch. It compiles and
runs the actual Schematron rule sets published by the relevant standards bodies:

- CEN/TC 434's EN 16931 Schematron, from `ConnectingEurope/eInvoicing-EN16931` (EUPL-1.2).
- OpenPeppol's BIS Billing 3.0 Schematron, from `OpenPEPPOL/peppol-bis-invoice-3` (Apache-2.0, per
  that repository).
- KoSIT's XRechnung Schematron.

Sample 2 in this repo (`02-validate-peppol/`) generates a Peppol BIS Billing 3.0 UBL invoice in
memory and validates it through this pipeline — well-formedness, then XSD schema, then Schematron
— reporting exactly which rule-set versions it validated against and how many rule violations were
found. Run it yourself; the sample's own `README.md` documents the exact expected output.

## PDF/A conformance, independently verified

Antiphon's Factur-X support (Sample 3, `03-facturx-pdf/`) embeds invoice XML into a PDF/A container.
Both the seed PDF and the generated output have been checked with an independent, third-party tool —
[veraPDF](https://verapdf.org/) — not just asserted by the SDK itself:

- The sample's seed file, `sample-input.pdf`, is genuinely PDF/A-1b conformant: veraPDF 1.30.2
  reports **129/129 rules passed, 0 failed** (`isCompliant="true"`).
- The sample's generated output, `facturx-output.pdf`, is genuinely PDF/A-3b conformant: veraPDF
  reports **146/146 rules passed, 0 failed**.

The full veraPDF report for the seed file is committed in this repository at
`tools/sample-input-verapdf-1b-report.xml`, alongside the script that built the seed PDF
(`tools/make-sample-pdfa.py`).

## What this means for you

Every number above comes from this repository's own runnable code and a third-party validator's
own output — clone the repo, run the samples, and reproduce it. See
[How To Run Examples](Antiphon-How-To-Run-Examples.md) to do exactly that.

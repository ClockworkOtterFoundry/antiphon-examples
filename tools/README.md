# tools/

Dev-time-only scripts, not part of any sample's runtime dependency graph.

- `make-sample-pdfa.py` — generates `03-facturx-pdf/sample-input.pdf`: a minimal, standalone
  PDF/A-1b conformant single-page PDF (a couple of filled rectangles, no text/fonts, a proper
  sRGB `OutputIntent`, and synchronized Info/XMP metadata carrying the `pdfaid` identification).
  Hand-built (classic xref, no dependency on any PDF-authoring library) so the sample repo doesn't
  need a PDF-generation package just to produce its own seed fixture.
- `sRGB-v2-micro.icc` — the 456-byte sRGB v2 ICC profile embedded in the seed PDF's OutputIntent.
  From [saucecontrol/Compact-ICC-Profiles](https://github.com/saucecontrol/Compact-ICC-Profiles)
  (CC0 / public domain).
- `sample-input-verapdf-1b-report.xml` — the [veraPDF](https://verapdf.org/) 1.30.2 PDF/A-1b
  validation report for `03-facturx-pdf/sample-input.pdf`: **129/129 rules passed, 0 failed,
  `isCompliant="true"`**. Regenerate with:

  ```bash
  python3 tools/make-sample-pdfa.py   # writes 03-facturx-pdf/sample-input.pdf
  verapdf -f 1b 03-facturx-pdf/sample-input.pdf
  ```

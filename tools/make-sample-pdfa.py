#!/usr/bin/env python3
"""Hand-build a minimal, standalone PDF/A-1b conformant PDF.

No text/fonts (avoids font-embedding requirements entirely) — just a page with a
couple of filled rectangles, a proper sRGB OutputIntent, and synchronized
Info/XMP metadata carrying the pdfaid identification. Classic (non-stream) xref,
targeting PDF 1.4, per PDF/A-1's base-format requirement.
"""
import os

_here = os.path.dirname(os.path.abspath(__file__))
ICC_PATH = os.path.join(_here, "sRGB-v2-micro.icc")
OUT_PATH = os.path.join(_here, "..", "03-facturx-pdf", "sample-input.pdf")

with open(ICC_PATH, "rb") as f:
    icc_bytes = f.read()

create_date = "2026-07-01T00:00:00Z"
create_date_pdf = "D:20260701000000Z"

xmp = f"""<?xpacket begin="﻿" id="W5M0MpCehiHzreSzNTczkc9d"?>
<x:xmpmeta xmlns:x="adobe:ns:meta/">
 <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
  <rdf:Description rdf:about=""
    xmlns:pdfaid="http://www.aiim.org/pdfa/ns/id/"
    xmlns:dc="http://purl.org/dc/elements/1.1/"
    xmlns:xmp="http://ns.adobe.com/xap/1.0/"
    xmlns:pdf="http://ns.adobe.com/pdf/1.3/">
   <pdfaid:part>1</pdfaid:part>
   <pdfaid:conformance>B</pdfaid:conformance>
   <dc:format>application/pdf</dc:format>
   <dc:title>
    <rdf:Alt>
     <rdf:li xml:lang="x-default">Antiphon sample seed PDF/A</rdf:li>
    </rdf:Alt>
   </dc:title>
   <xmp:CreateDate>{create_date}</xmp:CreateDate>
   <xmp:ModifyDate>{create_date}</xmp:ModifyDate>
   <xmp:CreatorTool>antiphon-examples/tools/make-sample-pdfa.py</xmp:CreatorTool>
   <pdf:Producer>antiphon-examples/tools/make-sample-pdfa.py</pdf:Producer>
  </rdf:Description>
 </rdf:RDF>
</x:xmpmeta>
<?xpacket end="w"?>"""
xmp_bytes = xmp.encode("utf-8")

content = b"q\n1 0.6 0.2 rg\n72 648 468 72 re f\nQ\n"

objects = []  # list of bytes, index 0 unused (object numbers are 1-based)

def add_obj(body: bytes) -> int:
    objects.append(body)
    return len(objects)

# Reserve numbers in the order we build them (order matters for cross-refs below,
# but PDF allows forward references, so we just need every object number resolved
# before writing the file).
obj_catalog = 1
obj_pages = 2
obj_page = 3
obj_content = 4
obj_outputintent = 5
obj_metadata = 6
obj_iccprofile = 7
obj_info = 8

body_catalog = (
    f"<< /Type /Catalog /Pages {obj_pages} 0 R "
    f"/OutputIntents [{obj_outputintent} 0 R] /Metadata {obj_metadata} 0 R >>"
).encode("ascii")

body_pages = f"<< /Type /Pages /Kids [{obj_page} 0 R] /Count 1 >>".encode("ascii")

body_page = (
    f"<< /Type /Page /Parent {obj_pages} 0 R /MediaBox [0 0 612 792] "
    f"/Resources << /ProcSet [/PDF] >> /Contents {obj_content} 0 R >>"
).encode("ascii")

body_content = (
    f"<< /Length {len(content)} >>\nstream\n".encode("ascii") + content + b"\nendstream"
)

body_outputintent = (
    f"<< /Type /OutputIntent /S /GTS_PDFA1 "
    f"/OutputConditionIdentifier (sRGB IEC61966-2.1) "
    f"/Info (sRGB IEC61966-2.1) /RegistryName (http://www.color.org) "
    f"/DestOutputProfile {obj_iccprofile} 0 R >>"
).encode("ascii")

body_metadata = (
    f"<< /Type /Metadata /Subtype /XML /Length {len(xmp_bytes)} >>\nstream\n".encode("ascii")
    + xmp_bytes
    + b"\nendstream"
)

body_iccprofile = (
    f"<< /N 3 /Alternate /DeviceRGB /Length {len(icc_bytes)} >>\nstream\n".encode("ascii")
    + icc_bytes
    + b"\nendstream"
)

body_info = (
    f"<< /Title (Antiphon sample seed PDF/A) "
    f"/Producer (antiphon-examples/tools/make-sample-pdfa.py) "
    f"/CreationDate ({create_date_pdf}) /ModDate ({create_date_pdf}) >>"
).encode("ascii")

bodies = {
    obj_catalog: body_catalog,
    obj_pages: body_pages,
    obj_page: body_page,
    obj_content: body_content,
    obj_outputintent: body_outputintent,
    obj_metadata: body_metadata,
    obj_iccprofile: body_iccprofile,
    obj_info: body_info,
}

n_objects = max(bodies.keys())

out = bytearray()
out += b"%PDF-1.4\n%\xe2\xe3\xcf\xd3\n"  # binary marker comment (common convention)

offsets = {}
for num in range(1, n_objects + 1):
    offsets[num] = len(out)
    out += f"{num} 0 obj\n".encode("ascii")
    out += bodies[num]
    out += b"\nendobj\n"

xref_offset = len(out)
out += f"xref\n0 {n_objects + 1}\n".encode("ascii")
out += b"0000000000 65535 f \n"
for num in range(1, n_objects + 1):
    out += f"{offsets[num]:010d} 00000 n \n".encode("ascii")

doc_id = "0011223344556677889900112233445566"[:32]
out += (
    f"trailer\n<< /Size {n_objects + 1} /Root {obj_catalog} 0 R /Info {obj_info} 0 R "
    f"/ID [<{doc_id}> <{doc_id}>] >>\nstartxref\n{xref_offset}\n%%EOF\n"
).encode("ascii")

with open(OUT_PATH, "wb") as f:
    f.write(out)

print(f"Wrote {OUT_PATH}: {len(out)} bytes, {n_objects} objects")

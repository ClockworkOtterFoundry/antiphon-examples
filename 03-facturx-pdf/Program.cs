// Scenario 3: Produce a Factur-X PDF.
//
// Source of truth: docs/Antiphon-Getting-Started.md (private Antiphon repo), Scenario 3 — the
// Invoice model and call below are reproduced as documented, wrapped into a runnable console app.
//
// Precondition (from the getting-started doc, stated up front because it's easy to miss): the
// input PDF must already be PDF/A-conformant. Antiphon adds the Factur-X attachment and metadata
// layer; it does not convert an arbitrary, non-PDF/A PDF into one. sample-input.pdf here is a
// genuinely PDF/A-1b conformant seed file — see README.md for how its conformance was verified.

using Antiphon.Generation.Cii;
using Antiphon.Model;
using Antiphon.Model.CodeLists;
using Antiphon.Pdf;

var invoice = new Invoice(
    Id: "FX-2026-0099",
    IssueDate: new DateOnly(2026, 7, 3),
    TypeCode: DocumentTypeCode.CommercialInvoice,
    CurrencyCode: CurrencyCode.EUR,
    SpecificationIdentifier: "urn:cen.eu:en16931:2017",
    Seller: new SellerParty("Antiphon Software GmbH", new PostalAddress(new CountryCode("DE"))
    {
        Line1 = "Musterstraße 1",
        City = "Berlin",
        PostCode = "10115",
    })
    {
        VatIdentifier = new Identifier("DE123456789"),
        LegalRegistrationIdentifier = new Identifier("HRB 123456"),
    },
    Buyer: new BuyerParty("Boulangerie Dupont SARL", new PostalAddress(new CountryCode("FR"))
    {
        Line1 = "12 Rue de la Paix",
        City = "Paris",
        PostCode = "75002",
    })
    {
        VatIdentifier = new Identifier("FR12345678901"),
    },
    Lines: new[]
    {
        new InvoiceLine(
            Id: "1",
            Quantity: 3m,
            UnitCode: "C62",
            NetAmount: 150.00m,
            Item: new Item("Support-Lizenz (Jahr)", VatCategoryCode.StandardRate) { VatRate = 19m },
            Price: new Price(50.00m)),
        new InvoiceLine(
            Id: "2",
            Quantity: 2m,
            UnitCode: "C62",
            NetAmount: 50.00m,
            Item: new Item("Onboarding-Workshop", VatCategoryCode.StandardRate) { VatRate = 19m },
            Price: new Price(25.00m)),
    },
    VatBreakdown: new[]
    {
        new VatBreakdownEntry(TaxableAmount: 200.00m, TaxAmount: 38.00m, CategoryCode: VatCategoryCode.StandardRate) { Rate = 19m },
    },
    Totals: new DocumentTotals(
        LineNetAmountSum: 200.00m, TaxExclusiveAmount: 200.00m, TaxInclusiveAmount: 238.00m, PayableAmount: 238.00m)
    {
        TotalVatAmount = 38.00m,
    })
{
    BuyerReference = "PO-2026-338",
    PaymentInstructions = new[]
    {
        new PaymentInstructions(PaymentMeansCode.CreditTransfer)
        {
            CreditTransfer = new CreditTransfer(new Identifier("DE75512108001245126199")) { AccountName = "Antiphon Software GmbH" },
        },
    },
};

string ciiXml = CiiInvoiceGenerator.GenerateXml(invoice);

byte[] callerPdf = File.ReadAllBytes("sample-input.pdf"); // already PDF/A-conformant
byte[] facturXPdf = FacturXEmbedder.Embed(callerPdf, ciiXml);
File.WriteAllBytes("facturx-output.pdf", facturXPdf);

Console.WriteLine($"Wrote facturx-output.pdf ({facturXPdf.Length} bytes)");

// Round-trip, to prove the embed worked (also how you'd read a Factur-X PDF someone sent you):
string? extracted = FacturXExtractor.Extract(facturXPdf);
Console.WriteLine(extracted == ciiXml ? "round-trip OK" : "mismatch");

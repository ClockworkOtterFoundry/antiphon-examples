// Scenario 1: Generate an XRechnung UBL invoice.
//
// Source of truth: docs/Antiphon-Getting-Started.md (private Antiphon repo), Scenario 1 — the
// Invoice model and call below are reproduced as documented, wrapped into a runnable console app.
// Runs in evaluation mode (no license key): UblInvoiceGenerator.GenerateXml never requires one.

using Antiphon.Generation.Ubl;
using Antiphon.Model;
using Antiphon.Model.CodeLists;

var invoice = new Invoice(
    Id: "RE-2026-1007",
    IssueDate: new DateOnly(2026, 7, 1),
    TypeCode: DocumentTypeCode.CommercialInvoice,
    CurrencyCode: CurrencyCode.EUR,
    SpecificationIdentifier: "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0",
    Seller: new SellerParty("Antiphon Software GmbH", new PostalAddress(new CountryCode("DE"))
    {
        Line1 = "Musterstraße 1",
        City = "Berlin",
        PostCode = "10115",
    })
    {
        ElectronicAddress = new Identifier("billing@antiphon.example", "EM"),
        VatIdentifier = new Identifier("DE123456789"),
        LegalRegistrationIdentifier = new Identifier("HRB 123456"),
        Contact = new Contact { Name = "Rechnungsstelle", Telephone = "+49 30 1234567", Email = "billing@antiphon.example" },
    },
    Buyer: new BuyerParty("Musterbehörde Berlin", new PostalAddress(new CountryCode("DE"))
    {
        Line1 = "Amtsstraße 5",
        City = "Berlin",
        PostCode = "10117",
    })
    {
        ElectronicAddress = new Identifier("einkauf@musterbehoerde.example", "EM"),
    },
    Lines: new[]
    {
        new InvoiceLine(
            Id: "1",
            Quantity: 10m,
            UnitCode: "C62",
            NetAmount: 1000.00m,
            Item: new Item("Beratungsleistung", VatCategoryCode.StandardRate) { VatRate = 19m },
            Price: new Price(100.00m)),
    },
    VatBreakdown: new[]
    {
        new VatBreakdownEntry(TaxableAmount: 1000.00m, TaxAmount: 190.00m, CategoryCode: VatCategoryCode.StandardRate) { Rate = 19m },
    },
    Totals: new DocumentTotals(
        LineNetAmountSum: 1000.00m, TaxExclusiveAmount: 1000.00m, TaxInclusiveAmount: 1190.00m, PayableAmount: 1190.00m)
    {
        TotalVatAmount = 190.00m,
    })
{
    BusinessProcessType = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0",
    BuyerReference = "04011000-12345-03",  // German public-sector Leitweg-ID, or your BT-10 reference
    PaymentDueDate = new DateOnly(2026, 7, 31),
    PaymentTerms = "Zahlbar innerhalb 30 Tagen netto.",
    PaymentInstructions = new[]
    {
        new PaymentInstructions(PaymentMeansCode.SepaCreditTransfer)
        {
            CreditTransfer = new CreditTransfer(new Identifier("DE75512108001245126199")) { AccountName = "Antiphon Software GmbH" },
        },
    },
};

string xml = UblInvoiceGenerator.GenerateXml(invoice);
File.WriteAllText("invoice.xml", xml);

var bytes = System.Text.Encoding.UTF8.GetByteCount(xml);
Console.WriteLine($"Wrote invoice.xml ({bytes} bytes)");
Console.WriteLine("Generation is deterministic: running this again produces byte-identical output.");

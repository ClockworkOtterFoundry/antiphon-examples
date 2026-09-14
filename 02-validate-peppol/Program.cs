// Scenario 2: Validate a Peppol BIS Billing 3.0 invoice.
//
// Source of truth: docs/Antiphon-Getting-Started.md (private Antiphon repo), Scenario 2. That
// scenario reads invoice.xml from disk; this sample is self-contained instead — it generates a
// valid Peppol-profile invoice in memory first (reusing Scenario 1's fabricated invoice data with
// the Peppol CustomizationID swapped in, per docs/Antiphon-Rule-Reference.md's PEPPOL-EN16931-R004),
// then validates that in-memory output. No external file dependency.

using Antiphon.Generation.Ubl;
using Antiphon.Model;
using Antiphon.Model.CodeLists;
using Antiphon.Rules.Ir;
using Antiphon.Rules.Ir.Discovery;
using Antiphon.Validation.Pipeline;

var invoice = new Invoice(
    Id: "RE-2026-1007",
    IssueDate: new DateOnly(2026, 7, 1),
    TypeCode: DocumentTypeCode.CommercialInvoice,
    CurrencyCode: CurrencyCode.EUR,
    // Peppol BIS Billing 3.0 CustomizationID (docs/Antiphon-Rule-Reference.md, PEPPOL-EN16931-R004)
    // — this is what routes format auto-detection to the Peppol + CEN rule sets below.
    SpecificationIdentifier: "urn:cen.eu:en16931:2017#compliant#urn:fdc:peppol.eu:2017:poacc:billing:3.0",
    Seller: new SellerParty("Antiphon Software GmbH", new PostalAddress(new CountryCode("DE"))
    {
        Line1 = "Musterstraße 1",
        City = "Berlin",
        PostCode = "10115",
    })
    {
        ElectronicAddress = new Identifier("billing@antiphon.example", "0204"),
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
        ElectronicAddress = new Identifier("einkauf@musterbehoerde.example", "0204"),
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
    BuyerReference = "04011000-12345-03",
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
Console.WriteLine("Generated a Peppol BIS Billing 3.0 UBL invoice in memory.");
Console.WriteLine();

// Discover every currently-referenced rule set (RuleSetCatalog scans loaded/available assemblies —
// no hardcoded list of which Antiphon.Rules.* packages you happen to reference).
var ruleSets = RuleSetCatalog.Discover()
    .Where(d => d.IsCurrent)
    .ToDictionary(d => d.SourceId, d => IrSerializer.Deserialize(d.GetCompiledIrJson()));

var result = LayeredValidator.Validate(xml, ruleSets);

Console.WriteLine($"Well-formedness: {result.WellFormedness}");
Console.WriteLine($"Schema: {result.Schema}");
foreach (var violation in result.Violations)
    Console.WriteLine($"[{violation.Severity}] {violation.RuleId}: {violation.Message}");

// FR-20: which upstream rule-set version(s) actually ran.
foreach (var p in result.Provenance)
    Console.WriteLine($"Validated against {p.SourceId} {p.SourceVersion} ({p.Upstream}, {p.License})");

using UblSharp.CommonAggregateComponents;
using UblSharp.UnqualifiedDataTypes;

namespace Common.Contracts;

public class InvoiceContract
{
    public IdentifierType UBLVersionID { get; set; }
    public IdentifierType ProfileID { get; set; }
    public IdentifierType ID { get; set; }
    public IdentifierType UUID { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime IssueTime { get; set; }
    public CodeType InvoiceTypeCode { get; set; }
    public CodeType DocumentCurrencyCode { get; set; }
    public CodeType TaxCurrencyCode { get; set; }
    public NumericType LineCountNumeric { get; set; }
    public List<DocumentReferenceType> AdditionalDocumentReference { get; set; }
    public List<SignatureType> Signature { get; set; }
    public SupplierPartyType AccountingSupplierParty { get; set; }
    public CustomerPartyType AccountingCustomerParty { get; set; }
    public DeliveryType Delivery { get; set; }
    public List<PaymentMeansType> PaymentMeans { get; set; }
    public TaxTotalType TaxTotal { get; set; }
    public MonetaryTotalType LegalMonetaryTotal { get; set; }
    public List<InvoiceLineType> InvoiceLine { get; set; }
    
}
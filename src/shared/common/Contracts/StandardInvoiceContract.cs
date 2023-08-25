using Common.Contracts.Base;

namespace Common.Contracts;

public class StandardInvoiceContract : BaseInvoiceContract
{
    public string CustomerName { get; set; }
    public string  InvoiceId { get; set; }
}
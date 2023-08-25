namespace Common.Contracts;

public record InvoiceReadyToSubmit
{
    public string InvoiceId { get; set; }
}
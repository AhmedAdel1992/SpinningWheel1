using Common.Contracts.Base.Enums;

namespace Common.Contracts.Base;

public class BaseInvoiceContract
{
    public string Id { get; set; }
    public string UUid { get; set; }
    public DateOnly IssueDate { get; set; }
    public TimeOnly IssueTime { get; set; }
    public string DocCurrency { get; set; } = "SAR";
    public string TaxCurrency { get; set; } = "SAR";
    public DateOnly DeliveryActualDate { get; set; }
    public PaymentMeans Payment { get; set; }
    public CustomerInfo CustomerInfo { get; set; }
    // public Tax TaxType { get; set; }
    public MonetaryType MonetaryTotalType { get; set; }
    public List<LineDto> Lines { get; set; }
    public string ApplicationName { get; set; }
}
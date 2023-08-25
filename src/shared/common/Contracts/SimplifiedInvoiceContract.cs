using Common.Contracts.Base;
using Mapster;
using UblSharp;
using UblSharp.CommonAggregateComponents;
using UblSharp.UnqualifiedDataTypes;

namespace Common.Contracts;

public class SimplifiedInvoiceContract:BaseInvoiceContract,IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DateOnly, DeliveryType>()
            .Map(dist=>dist.ActualDeliveryDate,src=>src.ToString("yyyy-MM-dd"))
            ;
            
        config.NewConfig<SimplifiedInvoiceContract, InvoiceType>()
            .Map(dist=>dist.ID,src=>src.Id)
            .Map(dist=>dist.UUID,src=>src.UUid)
            .Map(dist=>dist.IssueDate,src=>src.IssueDate.ToString("yyyy-MM-dd"))
            .Map(dist=>dist.IssueTime,src=>src.IssueTime.ToString("HH:mm:ss"))
            .Map(dist=>dist.DocumentCurrencyCode,src=>src.DocCurrency)
            .Map(dist=>dist.TaxCurrencyCode,src=>src.TaxCurrency)
            .Map(dist=>dist.Delivery,src=>new List<DateOnly>(){ src.DeliveryActualDate})
            .Map(dist=>dist.InvoiceLine,src=>src.Lines)
            ;
    }


}



public class MonetaryType
{
}

public class Line:IRegister
{
    public string Id { get; set; }
    public string Currency { get; set; }
    public decimal Price { get; set; }
    
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Line, PriceType>();
        config.NewConfig<Line, InvoiceLineType>()
            .Map(dist=>dist.ID,src=>src.Id)
            .Map(dist=>dist.Price.PriceAmount.Value,src=>src.Price)
            .Map(dist=>dist.Price.PriceAmount.currencyID,src=>src.Currency)
            ;
    }
}

public class CustomerInfo
{
}

public enum PaymentMeans
{
}
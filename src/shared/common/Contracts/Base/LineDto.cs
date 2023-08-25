using Mapster;
using UblSharp.CommonAggregateComponents;

namespace Common.Contracts.Base;

public class LineDto:IRegister
{
    public string Id { get; set; }
    public string Currency { get; set; }
    public decimal Price { get; set; }
    
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LineDto, InvoiceLineType>()
            .Map(dist=>dist.ID,src=>src.Id)
            .Map(dist=>dist.Price.PriceAmount.Value,src=>src.Price)
            .Map(dist=>dist.Price.PriceAmount.currencyID,src=>src.Currency)
            ;
        
        config.NewConfig<Line, InvoiceLineType>()
            .Map(dist=>dist.ID,src=>src.Id)
            .Map(dist=>dist.Price.PriceAmount.Value,src=>src.Price)
            .Map(dist=>dist.Price.PriceAmount.currencyID,src=>src.Currency)
            ;
    }
}
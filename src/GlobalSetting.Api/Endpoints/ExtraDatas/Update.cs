using Application.ExtraDatas.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.ExtraDatas;

public class Update : MyEndpoint<UpdateExtraDataCommand, ExtraDataDto>
{
    public Update(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Put("/extradata/");
        AllowAnonymous();
    }
}
using Application.ExtraDatas.Commands;
using Application.ExtraDatas.Queries;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.ExtraDatas;

public class GetExtraData : MyEndpoint<GetExtraDataByIdQuery, ExtraDataDto>
{
    public GetExtraData(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Get("/extradata/{id}");
        AllowAnonymous();
    }
}
using Application.ExtraDatas.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.ExtraDatas;

public class Delete : MyEndpoint<DeleteExtraDataCommand, ExtraDataDto>
{
    public Delete(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Delete("/extradata/{id}");
        AllowAnonymous();
    }
}
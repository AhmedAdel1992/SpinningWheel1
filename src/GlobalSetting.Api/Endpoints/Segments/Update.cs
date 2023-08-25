using Application.Segments.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Segments;

public class Update : MyEndpoint<UpdateSegmentCommand, SegmentDto>
{
    public Update(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Put("/segment/");
        AllowAnonymous();
    }
}
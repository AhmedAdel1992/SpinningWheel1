using Application.Segments.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Segments;

public class Delete : MyEndpoint<DeleteSegmentCommand, SegmentDto>
{
    public Delete(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Delete("/segment/{id}");
        AllowAnonymous();
    }
}
using Application.Segments.Commands;
using Application.Segments.Queries;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Segments;

public class GetSegment : MyEndpoint<GetSegmentByIdQuery, SegmentDto>
{
    public GetSegment(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Get("/segment/{id}");
        AllowAnonymous();
    }
}
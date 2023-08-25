using Application.Segments.Commands;
using Application.Segments.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Segments;

public class Search : MyEndpoint<SearchSegmentsQuery, PaginationResponse<SegmentDto>>
{
    public Search(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Post("/segment/search");
        AllowAnonymous();
    }
}
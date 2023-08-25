using Application.ExtraDatas.Commands;
using Application.ExtraDatas.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.ExtraDatas;

public class Search : MyEndpoint<SearchExtraDataQuery, PaginationResponse<ExtraDataDto>>
{
    public Search(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Post("/extradata/search");
        AllowAnonymous();
    }
}
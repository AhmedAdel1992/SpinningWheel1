using Application.SpinningWheels.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.SpinningWheels;

public class Search : MyEndpoint<SearchSpinningWheelsQuery, PaginationResponse<SpinningWheelDto>>
{
    public Search(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Post("/spinningwheel/search");
        AllowAnonymous();
    }
}
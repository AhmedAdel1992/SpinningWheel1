using Application.Segments.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Segments
{
    public class Create : MyEndpoint<CreateSegmentCommand, SegmentDto>
    {
        public Create(IMediator mediator) : base(mediator)
        {
        }

        public override void Configure()
        {
            Post("/segment");
            AllowAnonymous();
        }
    }

}

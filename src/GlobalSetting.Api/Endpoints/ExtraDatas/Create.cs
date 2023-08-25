using Application.ExtraDatas.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.ExtraDatas
{
    public class Create : MyEndpoint<CreateExtraDataCommand, ExtraDataDto>
    {
        public Create(IMediator mediator) : base(mediator)
        {
        }

        public override void Configure()
        {
            Post("/extradata");
            AllowAnonymous();
        }
    }

}

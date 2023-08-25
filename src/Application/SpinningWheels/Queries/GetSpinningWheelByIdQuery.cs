using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SpinningWheels.Queries;
using Domain.Dto;
using LanguageExt.Common;
using Mapster;

namespace Application.SpinningWheels.Queries;

public class GetSpinningWheelByIdQuery : IQuery<SpinningWheelDto>
{
    public string Id { get; set; }
}

public class GetByIdQueryHandler : IQueryHandler<GetSpinningWheelByIdQuery, SpinningWheelDto>
{
    private readonly ISpinningWheelRepository _spinningWheelRepository;

    public GetByIdQueryHandler(ISpinningWheelRepository invoiceRepository) => _spinningWheelRepository = invoiceRepository;

    public async Task<Result<SpinningWheelDto>> Handle(GetSpinningWheelByIdQuery request, CancellationToken cancellationToken)
    {
        var spinningWheel = await _spinningWheelRepository.GetByIdAsync(request.Id);
        return spinningWheel.Adapt<SpinningWheelDto>();
    }
}

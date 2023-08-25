using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ExtraDatas.Queries;
using Domain.Dto;
using LanguageExt.Common;
using Mapster;

namespace Application.ExtraDatas.Queries;

public class GetExtraDataByIdQuery : IQuery<ExtraDataDto>
{
    public string Id { get; set; }
}

public class GetByIdQueryHandler : IQueryHandler<GetExtraDataByIdQuery, ExtraDataDto>
{
    private readonly IExtraDataRepository _extraDataRepository;

    public GetByIdQueryHandler(IExtraDataRepository invoiceRepository) => _extraDataRepository = invoiceRepository;

    public async Task<Result<ExtraDataDto>> Handle(GetExtraDataByIdQuery request, CancellationToken cancellationToken)
    {
        var extraData = await _extraDataRepository.GetByIdAsync(request.Id);
        return extraData.Adapt<ExtraDataDto>();
    }
}

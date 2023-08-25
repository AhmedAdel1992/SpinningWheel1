using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Segments.Queries;
using Domain.Dto;
using LanguageExt.Common;
using Mapster;

namespace Application.Segments.Queries;

public class GetSegmentByIdQuery : IQuery<SegmentDto>
{
    public string Id { get; set; }
}

public class GetByIdQueryHandler : IQueryHandler<GetSegmentByIdQuery, SegmentDto>
{
    private readonly ISegmentRepository _segmentRepository;

    public GetByIdQueryHandler(ISegmentRepository invoiceRepository) => _segmentRepository = invoiceRepository;

    public async Task<Result<SegmentDto>> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
    {
        var segment = await _segmentRepository.GetByIdAsync(request.Id);
        return segment.Adapt<SegmentDto>();
    }
}

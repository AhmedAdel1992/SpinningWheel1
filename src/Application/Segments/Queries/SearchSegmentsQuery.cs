using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto;
using Domain.Extensions.Models;
using LanguageExt.Common;

namespace Application.Segments.Queries;

public class SearchSegmentsQuery : PaginationFilter, IQuery<PaginationResponse<SegmentDto>>
{
}

public class SearchInvoicesQueryHandler : IQueryHandler<SearchSegmentsQuery, PaginationResponse<SegmentDto>>
{
    private readonly ISegmentRepository _segmentRepository;

    public SearchInvoicesQueryHandler(ISegmentRepository invoiceRepository) => _segmentRepository = invoiceRepository;

    public async Task<Result<PaginationResponse<SegmentDto>>> Handle(SearchSegmentsQuery request,
        CancellationToken cancellationToken) =>
        await _segmentRepository
            .GetAll(request, cancellationToken);

    
}

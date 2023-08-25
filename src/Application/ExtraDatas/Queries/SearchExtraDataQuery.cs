using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ExtraDatas.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using LanguageExt.Common;

namespace Application.ExtraDatas.Queries;

public class SearchExtraDataQuery : PaginationFilter, IQuery<PaginationResponse<ExtraDataDto>>
{
}

public class SearchInvoicesQueryHandler : IQueryHandler<SearchExtraDataQuery, PaginationResponse<ExtraDataDto>>
{
    private readonly IExtraDataRepository _extraDataRepository;

    public SearchInvoicesQueryHandler(IExtraDataRepository invoiceRepository) => _extraDataRepository = invoiceRepository;

    public async Task<Result<PaginationResponse<ExtraDataDto>>> Handle(SearchExtraDataQuery request,
        CancellationToken cancellationToken) =>
        await _extraDataRepository
            .GetAll(request, cancellationToken);
}

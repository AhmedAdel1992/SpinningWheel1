using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SpinningWheels.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using LanguageExt.Common;

namespace Application.SpinningWheels.Queries;

public class SearchSpinningWheelsQuery : PaginationFilter, IQuery<PaginationResponse<SpinningWheelDto>>
{
}

public class SearchInvoicesQueryHandler : IQueryHandler<SearchSpinningWheelsQuery, PaginationResponse<SpinningWheelDto>>
{
    private readonly ISpinningWheelRepository _spinningWheelRepository;

    public SearchInvoicesQueryHandler(ISpinningWheelRepository invoiceRepository) => _spinningWheelRepository = invoiceRepository;

    public async Task<Result<PaginationResponse<SpinningWheelDto>>> Handle(SearchSpinningWheelsQuery request,
        CancellationToken cancellationToken) =>
        await _spinningWheelRepository
            .GetAll(request, cancellationToken);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ExtraDatas.Commands;
using Domain.Dto;
using FluentValidation;
using LanguageExt.Common;
using Mapster;

namespace Application.ExtraDatas.Commands;

public class DeleteExtraDataCommand : ICommand<ExtraDataDto>
{
    public string Id { get; set; }
}

public class DeleteExtraDataCommandValidator : AbstractValidator<DeleteExtraDataCommand>
{
    private readonly IExtraDataRepository _repository;

    public DeleteExtraDataCommandValidator(IExtraDataRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
            .MustAsync(IdMustExistAsync);
    }

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class DeleteExtraDataCommandHandler : ICommandHandler<DeleteExtraDataCommand, ExtraDataDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly IExtraDataRepository _repository;

    public DeleteExtraDataCommandHandler(IAppUnitOfWork uow, IExtraDataRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<ExtraDataDto>> Handle(DeleteExtraDataCommand request, CancellationToken cancellationToken)
    {
        var currentData = await _repository.GetByIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(currentData, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return currentData.Adapt<ExtraDataDto>();
    }
}
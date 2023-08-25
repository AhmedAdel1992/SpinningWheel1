using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Domain.Entities;
using Domain.Dto;
using FluentValidation;
using LanguageExt.Common;
using Mapster;
using Application.ExtraDatas.Commands;

namespace Application.ExtraDatas.Commands;

public class UpdateExtraDataCommand : ICommand<ExtraDataDto>
{
    public string Id { get; set; }
    public string ExtraDataName { get; set; }
    public string ExtraDataValue { get; set; }
    public string RewardId { get; set; }

}

public class UpdateExtraDataCommandValidator : AbstractValidator<UpdateExtraDataCommand>
{
    private readonly IExtraDataRepository _repository;

    public UpdateExtraDataCommandValidator(IExtraDataRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
        .MustAsync(IdMustExistAsync);
        RuleFor(x => x.ExtraDataName).NotNull()
        .MustAsync(NameNotExistAsync);
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class UpdateExtraDataCommandHandler : ICommandHandler<UpdateExtraDataCommand, ExtraDataDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly IExtraDataRepository _repository;

    public UpdateExtraDataCommandHandler(IAppUnitOfWork uow, IExtraDataRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<ExtraDataDto>> Handle(UpdateExtraDataCommand request, CancellationToken cancellationToken)
    {
        var currentData = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var updatedData = request.Adapt(currentData);
        await _repository.UpdateAsync(updatedData, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return updatedData.Adapt<ExtraDataDto>();
    }
}
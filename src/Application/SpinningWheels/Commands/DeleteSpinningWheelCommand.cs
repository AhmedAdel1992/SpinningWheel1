using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SpinningWheels.Commands;
using Domain.Dto;
using FluentValidation;
using LanguageExt.Common;
using Mapster;

namespace Application.SpinningWheels.Commands;

public class DeleteSpinningWheelCommand : ICommand<SpinningWheelDto>
{
    public string Id { get; set; }
}

public class DeleteSpinningWheelCommandValidator : AbstractValidator<DeleteSpinningWheelCommand>
{
    private readonly ISpinningWheelRepository _repository;

    public DeleteSpinningWheelCommandValidator(ISpinningWheelRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
            .MustAsync(IdMustExistAsync);
    }

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class DeleteSpinningWheelCommandHandler : ICommandHandler<DeleteSpinningWheelCommand, SpinningWheelDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly ISpinningWheelRepository _repository;

    public DeleteSpinningWheelCommandHandler(IAppUnitOfWork uow, ISpinningWheelRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<SpinningWheelDto>> Handle(DeleteSpinningWheelCommand request, CancellationToken cancellationToken)
    {
        var currentSpinningWheel = await _repository.GetByIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(currentSpinningWheel, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return currentSpinningWheel.Adapt<SpinningWheelDto>();
    }
}
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
using Application.SpinningWheels.Commands;

namespace Application.SpinningWheels.Commands;

public class UpdateSpinningWheelCommand : ICommand<SpinningWheelDto>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string BackgroundColor { get; set; }
    public string Color { get; set; }
    public string TextColor { get; set; }
    public string TopHeader { get; set; }
    public string BottomHeader { get; set; }
    public string ButtonText { get; set; }
    public string Type { get; set; }
    public bool IsActive { get; set; }
    public bool ShowRewardsImages { get; set; }
    public bool EnableRegistration { get; set; }
}

public class UpdateSpinningWheelCommandValidator : AbstractValidator<UpdateSpinningWheelCommand>
{
    private readonly ISpinningWheelRepository _repository;

    public UpdateSpinningWheelCommandValidator(ISpinningWheelRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
            .MustAsync(IdMustExistAsync);
        RuleFor(x => x.Name).NotNull()
            .MustAsync(NameNotExistAsync);
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class UpdateSpinningWheelCommandHandler : ICommandHandler<UpdateSpinningWheelCommand, SpinningWheelDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly ISpinningWheelRepository _repository;

    public UpdateSpinningWheelCommandHandler(IAppUnitOfWork uow, ISpinningWheelRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<SpinningWheelDto>> Handle(UpdateSpinningWheelCommand request, CancellationToken cancellationToken)
    {
        var currentSpinningWheel = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var updatedSpinningWheel = request.Adapt(currentSpinningWheel);
        await _repository.UpdateAsync(updatedSpinningWheel, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return updatedSpinningWheel.Adapt<SpinningWheelDto>();
    }
}
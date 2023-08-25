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


namespace Application.SpinningWheels.Commands;

public class CreateSpinningWheelCommand : ICommand<SpinningWheelDto>
{
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

public class CreateSpinningWheelCommandValidator : AbstractValidator<CreateSpinningWheelCommand>
{
    private readonly ISpinningWheelRepository _repository;

    public CreateSpinningWheelCommandValidator(ISpinningWheelRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Name).NotNull()
                  .MustAsync(NameNotExistAsync);
        //RuleFor(x => x.CountryName).NotNull();
        //RuleFor(x => x.CityName).NotNull();
        //RuleFor(x => x.CountryCode).NotNull().NotEmpty();
        //RuleFor(x => x.CityCode).NotNull().NotEmpty();
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);
}

public class CreateSpinningWheelCommandHandler : ICommandHandler<CreateSpinningWheelCommand, SpinningWheelDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly ISpinningWheelRepository _repository;

    public CreateSpinningWheelCommandHandler(IAppUnitOfWork uow, ISpinningWheelRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<SpinningWheelDto>> Handle(CreateSpinningWheelCommand request, CancellationToken cancellationToken)
    {
        var spinningWheel = request.Adapt<SpinningWheel>();
        await _repository.AddAsync(spinningWheel, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return spinningWheel.Adapt<SpinningWheelDto>();
    }
}
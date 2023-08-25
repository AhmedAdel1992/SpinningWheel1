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


namespace Application.ExtraDatas.Commands;

public class CreateExtraDataCommand : ICommand<ExtraDataDto>
{
    public string ExtraDataName { get; set; }
    public string ExtraDataValue { get; set; }
    public string RewardId { get; set; }
}

public class CreateExtraDataCommandValidator : AbstractValidator<CreateExtraDataCommand>
{
    private readonly IExtraDataRepository _repository;

    public CreateExtraDataCommandValidator(IExtraDataRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.ExtraDataName).NotNull()
                  .MustAsync(NameNotExistAsync);
        //RuleFor(x => x.CountryName).NotNull();
        //RuleFor(x => x.CityName).NotNull();
        //RuleFor(x => x.CountryCode).NotNull().NotEmpty();
        //RuleFor(x => x.CityCode).NotNull().NotEmpty();
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);
}

public class CreateExtraDataCommandHandler : ICommandHandler<CreateExtraDataCommand, ExtraDataDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly IExtraDataRepository _repository;

    public CreateExtraDataCommandHandler(IAppUnitOfWork uow, IExtraDataRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<ExtraDataDto>> Handle(CreateExtraDataCommand request, CancellationToken cancellationToken)
    {
        var extraData = request.Adapt<ExtraData>();
        await _repository.AddAsync(extraData, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return extraData.Adapt<ExtraDataDto>();
    }
}
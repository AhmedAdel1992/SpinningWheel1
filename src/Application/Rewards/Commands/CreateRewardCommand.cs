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


namespace Application.Rewards.Commands;

public class CreateRewardCommand : ICommand<RewardDto>
{
    public string RewardName { get; set; }
    public int Quantity { get; set; }
    public int Consumed { get; set; } = 0;
}

public class CreateRewardCommandValidator : AbstractValidator<CreateRewardCommand>
{
    private readonly IRewardRepository _repository;

    public CreateRewardCommandValidator(IRewardRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.RewardName).NotNull()
                  .MustAsync(NameNotExistAsync);
        //RuleFor(x => x.CountryName).NotNull();
        //RuleFor(x => x.CityName).NotNull();
        //RuleFor(x => x.CountryCode).NotNull().NotEmpty();
        //RuleFor(x => x.CityCode).NotNull().NotEmpty();
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);
}

public class CreateRewardCommandHandler : ICommandHandler<CreateRewardCommand, RewardDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly IRewardRepository _repository;

    public CreateRewardCommandHandler(IAppUnitOfWork uow, IRewardRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<RewardDto>> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
    {
        var reward = request.Adapt<Reward>();
        await _repository.AddAsync(reward, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return reward.Adapt<RewardDto>();
    }
}
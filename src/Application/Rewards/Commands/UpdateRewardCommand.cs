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

namespace Application.Rewards.Commands;

public class UpdateRewardCommand : ICommand<RewardDto>
{
    public string Id { get; set; }
    public string RewardName { get; set; }
    public int Quantity { get; set; }
    public int Consumed { get; set; } = 0;
}

public class UpdateRewardCommandValidator : AbstractValidator<UpdateRewardCommand>
{
    private readonly IRewardRepository _repository;

    public UpdateRewardCommandValidator(IRewardRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
            .MustAsync(IdMustExistAsync);
        RuleFor(x => x.RewardName).NotNull()
            .MustAsync(NameNotExistAsync);
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class UpdateRewardCommandHandler : ICommandHandler<UpdateRewardCommand, RewardDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly IRewardRepository _repository;

    public UpdateRewardCommandHandler(IAppUnitOfWork uow, IRewardRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<RewardDto>> Handle(UpdateRewardCommand request, CancellationToken cancellationToken)
    {
        var currentReward = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var updatedReward = request.Adapt(currentReward);
        await _repository.UpdateAsync(updatedReward, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return updatedReward.Adapt<RewardDto>();
    }
}
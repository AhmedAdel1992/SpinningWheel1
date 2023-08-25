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

namespace Application.Rewards.Commands;

public class DeleteRewardCommand : ICommand<RewardDto>
{
    public string Id { get; set; }
}

public class DeleteRewardCommandValidator : AbstractValidator<DeleteRewardCommand>
{
    private readonly IRewardRepository _repository;

    public DeleteRewardCommandValidator(IRewardRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
            .MustAsync(IdMustExistAsync);
    }

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class DeleteRewardCommandHandler : ICommandHandler<DeleteRewardCommand, RewardDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly IRewardRepository _repository;

    public DeleteRewardCommandHandler(IAppUnitOfWork uow, IRewardRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<RewardDto>> Handle(DeleteRewardCommand request, CancellationToken cancellationToken)
    {
        var currentReward = await _repository.GetByIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(currentReward, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return currentReward.Adapt<RewardDto>();
    }
}
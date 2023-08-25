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
using Application.Segments.Commands;

namespace Application.Segments.Commands;

public class UpdateSegmentCommand : ICommand<SegmentDto>
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string Color { get; set; }
    public string TextColor { get; set; }
    public string SpinningWheelId { get; set; }
    public string RewardId { get; set; }

}

public class UpdateSegmentCommandValidator : AbstractValidator<UpdateSegmentCommand>
{
    private readonly ISegmentRepository _repository;

    public UpdateSegmentCommandValidator(ISegmentRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
        .MustAsync(IdMustExistAsync);
        RuleFor(x => x.Label).NotNull()
        .MustAsync(NameNotExistAsync);
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class UpdateSegmentCommandHandler : ICommandHandler<UpdateSegmentCommand, SegmentDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly ISegmentRepository _repository;

    public UpdateSegmentCommandHandler(IAppUnitOfWork uow, ISegmentRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<SegmentDto>> Handle(UpdateSegmentCommand request, CancellationToken cancellationToken)
    {
        var currentSegment= await _repository.GetByIdAsync(request.Id, cancellationToken);
        var updatedSegment = request.Adapt(currentSegment);
        await _repository.UpdateAsync(updatedSegment, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return updatedSegment.Adapt<SegmentDto>();
    }
}
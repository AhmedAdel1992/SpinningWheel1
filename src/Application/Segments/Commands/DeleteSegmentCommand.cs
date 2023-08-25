using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Segments.Commands;
using Domain.Dto;
using FluentValidation;
using LanguageExt.Common;
using Mapster;

namespace Application.Segments.Commands;

public class DeleteSegmentCommand : ICommand<SegmentDto>
{
    public string Id { get; set; }
}

public class DeleteSegmentCommandValidator : AbstractValidator<DeleteSegmentCommand>
{
    private readonly ISegmentRepository _repository;

    public DeleteSegmentCommandValidator(ISegmentRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Id).NotNull()
            .MustAsync(IdMustExistAsync);
    }

    private async Task<bool> IdMustExistAsync(string id, CancellationToken cancellation) =>
        await _repository.IsIdExisted(id, cancellation);
}

public class DeleteSegmentCommandHandler : ICommandHandler<DeleteSegmentCommand, SegmentDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly ISegmentRepository _repository;

    public DeleteSegmentCommandHandler(IAppUnitOfWork uow, ISegmentRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<SegmentDto>> Handle(DeleteSegmentCommand request, CancellationToken cancellationToken)
    {
        var currentSegment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(currentSegment, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return currentSegment.Adapt<SegmentDto>();
    }
}
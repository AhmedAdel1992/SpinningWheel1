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


namespace Application.Segments.Commands;

public class CreateSegmentCommand : ICommand<SegmentDto>
{
    public string Label { get; set; }
    public string Color { get; set; }
    public string TextColor { get; set; }
    public string SpinningWheelId { get; set; }
    public string RewardId { get; set; }

}

public class CreateSegmentCommandValidator : AbstractValidator<CreateSegmentCommand>
{
    private readonly ISegmentRepository _repository;

    public CreateSegmentCommandValidator(ISegmentRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Label).NotNull()
                  .MustAsync(NameNotExistAsync);
        //RuleFor(x => x.CountryName).NotNull();
        //RuleFor(x => x.CityName).NotNull();
        //RuleFor(x => x.CountryCode).NotNull().NotEmpty();
        //RuleFor(x => x.CityCode).NotNull().NotEmpty();
    }

    private async Task<bool> NameNotExistAsync(string name, CancellationToken cancellation) =>
        !await _repository.IsNameExisted(name, cancellation);
}

public class CreateSegmentCommandHandler : ICommandHandler<CreateSegmentCommand, SegmentDto>
{
    private readonly IAppUnitOfWork _uow;
    private readonly ISegmentRepository _repository;

    public CreateSegmentCommandHandler(IAppUnitOfWork uow, ISegmentRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<SegmentDto>> Handle(CreateSegmentCommand request, CancellationToken cancellationToken)
    {
        var segment = request.Adapt<Segment>();
        await _repository.AddAsync(segment, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return segment.Adapt<SegmentDto>();
    }
}
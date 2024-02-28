using DocumentFormat.OpenXml.Wordprocessing;
using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Catalog.RuralGovs;
using FSH.WebApi.Application.Catalog.Totals;
using FSH.WebApi.Application.Catalog.Years;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using FSH.WebApi.Shared.Notifications;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Catalog;
public class ContributionRandomGeneratorJob : IRequestHandler<GenerateRandomContribution>
{
    private readonly ILogger<ContributionRandomGeneratorJob> _logger;
    private readonly ISender _mediator;
    private readonly IReadRepository<Contribution> _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;
    public ContributionRandomGeneratorJob(
       ILogger<ContributionRandomGeneratorJob> logger,
       ISender mediator,
       IReadRepository<Contribution> repository,
       IProgressBarFactory progressBar,
       PerformingContext performingContext,
       INotificationSender notifications,
       ICurrentUser currentUser)
    {
        _logger = logger;
        _mediator = mediator;
        _repository = repository;
        _progressBar = progressBar;
        _performingContext = performingContext;
        _notifications = notifications;
        _currentUser = currentUser;
        _progress = _progressBar.Create();
    }
    private async Task NotifyAsync(string message, int progress, CancellationToken cancellationToken)
    {
        _progress.SetValue(progress);
        await _notifications.SendToUserAsync(
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            },
            _currentUser.GetUserId().ToString(),
            cancellationToken);
    }
    public async Task<Unit> Handle(GenerateRandomContribution request, CancellationToken cancellationToken)
    {

        await NotifyAsync("Your job processing has started", 0, cancellationToken);
        var year = await _mediator.Send(new GetYearByYearRequest(2024));
        if (year == null) { return Unit.Value; }
        var rurals = await _mediator.Send(new GetllRuralGovsDtoRequest());
        if (rurals == null || rurals.Count == 0) { return Unit.Value; }
        Random rnd = new Random();
        var ruralgovIndex = rnd.Next(rurals.Count);

        var natives = await _mediator.Send(new GetAllNativesByRuralGovIdRequest(null));
        if (natives == null || natives.Count == 0) { return Unit.Value; }
        var nativeIndex = rnd.Next(natives.Count);
        var summaRnd = rnd.Next(250, 1000);
        var month = rnd.Next(1, 12);

        await _mediator.Send(
            new CreateContributionRequest
            {
                YearId = year.Id,
                RuralGovId = rurals[ruralgovIndex].Id,
                Date = DateTime.Now,
                NativeId = natives[nativeIndex].Id,
                Summa = summaRnd,
                Month = (Months)month
            },
        cancellationToken);

        await NotifyAsync($"Added {natives[nativeIndex].Name} summ: {summaRnd} ", 0, cancellationToken);


        await NotifyAsync("Job successfully completed", 0, cancellationToken);
        return Unit.Value;
    }
}

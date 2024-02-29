using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Catalog.RuralGovs;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Notifications;
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
public class NativeGeneratorJob : INativeGeneratorJob
{
    private readonly ILogger<NativeGeneratorJob> _logger;
    private readonly ISender _mediator;
    private readonly IReadRepository<Native> _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public NativeGeneratorJob(
        ILogger<NativeGeneratorJob> logger,
        ISender mediator,
        IReadRepository<Native> repository,
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

    public Task CleanAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task GenerateAsync(int nSeed, CancellationToken cancellationToken)
    {
        await NotifyAsync("Your job processing has started", 0, cancellationToken);
        var rurals = await _mediator.Send(new GetllRuralGovsDtoRequest());
        if (rurals == null || rurals.Count == 0) { return ; }
        Random rnd = new Random();

        var ruralgovIndex = rnd.Next(rurals.Count);

        foreach (int index in Enumerable.Range(1, nSeed))
        {
            ruralgovIndex = rnd.Next(rurals.Count);
            await _mediator.Send(
                new CreateNativeRequest
                {
                    Name = $"N_Random_{Guid.NewGuid()}",
                    Surname = $"S_Random_{Guid.NewGuid()}",
                    RuralGovId = rurals[ruralgovIndex].Id,
                    BirthDate=new DateTime(2000,01,01),
                    Description = "Funny description"
                },
                cancellationToken);

            await NotifyAsync("Progress: ", nSeed > 0 ? (index * 100 / nSeed) : 0, cancellationToken);
        }

        await NotifyAsync("Job successfully completed", 0, cancellationToken);
        
    }
}

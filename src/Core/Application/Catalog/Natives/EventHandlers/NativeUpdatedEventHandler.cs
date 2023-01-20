using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives.EventHandlers;
public class NativeUpdatedEventedHandler : EventNotificationHandler<EntityUpdatedEvent<Native>>
{
    private readonly ILogger<Native> _logger;
    public NativeUpdatedEventedHandler(ILogger<Native> logger)
    {
        _logger = logger;
    }

    public override Task Handle(EntityUpdatedEvent<Native> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
         return Task.CompletedTask;
    }
}

using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives.EventHandlers;
public class NativeCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Native>>
{
    private readonly ILogger<NativeCreatedEventHandler> _logger;
    public NativeCreatedEventHandler(ILogger<NativeCreatedEventHandler> logger)=>_logger = logger;
    

    public override Task Handle(EntityCreatedEvent<Native> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
         return Task.CompletedTask;

    }
}

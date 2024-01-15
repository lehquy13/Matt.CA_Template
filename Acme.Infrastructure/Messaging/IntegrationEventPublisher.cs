using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace Acme.Infrastructure.Messaging;

internal sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublisher _publisher;

    public IntegrationEventPublisher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishAsync(IIntegrationEvent integrationEvent)
    {
        await _publisher.Publish(integrationEvent);
    }
}

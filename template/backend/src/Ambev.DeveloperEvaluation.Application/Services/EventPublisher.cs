using System.Text.Json;
using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;
    private readonly IDatabase _redisDb;
    private readonly ISaleEventRepository _saleEventRepository;

    public EventPublisher(ILogger<EventPublisher> logger, IDatabase redisDb, ISaleEventRepository saleEventRepository)
    {
        _logger = logger;
        _redisDb = redisDb;
        _saleEventRepository = saleEventRepository;
    }

    public async Task PublishAsync<TEvent>(string channel, TEvent @event) where TEvent : IEvent
    {
        try
        {
            _logger.LogInformation("Publishing event: {EventType}", @event.EventType);

            Guid? saleId = @event switch
            {
                SaleCreatedEvent e => e.SaleId,
                SaleModifiedEvent e => e.SaleId,
                SaleCancelledEvent e => e.SaleId,
                ItemCancelledEvent e => e.SaleId,
                _ => null
            };

            if (saleId.HasValue)
            {
                await _saleEventRepository.AddAsync(@event);
                _logger.LogDebug("Event persisted for Sale {SaleId}", saleId.Value);
            }

            var jsonEvent = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            await _redisDb.PublishAsync(channel, jsonEvent);

            _logger.LogInformation(
                "Event published successfully on channel {Channel}. Event: {Event}",
                channel,
                jsonEvent
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error publishing event on channel {Channel}. Event: {Event}",
                channel,
                JsonSerializer.Serialize(@event)
            );
        }
    }
}

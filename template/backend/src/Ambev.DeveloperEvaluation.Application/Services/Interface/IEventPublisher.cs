using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Services.Interface;

/// <summary>
/// Defines the contract for publishing domain events to message channels.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event to the specified channel asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of event being published, must implement IEvent</typeparam>
    /// <param name="channel">The channel/topic to publish the event to</param>
    /// <param name="event">The event data to be published</param>
    /// <returns>A task representing the asynchronous publish operation</returns>
    Task PublishAsync<TEvent>(string channel, TEvent @event) where TEvent : IEvent;
}

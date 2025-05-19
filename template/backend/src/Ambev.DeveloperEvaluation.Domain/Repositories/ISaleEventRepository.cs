using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for managing sale-related events.
/// </summary>
public interface ISaleEventRepository
{
    /// <summary>
    /// Adds a new event to the repository asynchronously.
    /// </summary>
    /// <param name="event">The event to be added</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task AddAsync(IEvent @event);
    
    /// <summary>
    /// Retrieves all events associated with a specific sale.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale</param>
    /// <returns>A collection of events related to the specified sale</returns>
    Task<IEnumerable<IEvent>> GetEventsBySaleIdAsync(Guid saleId);
    
    /// <summary>
    /// Retrieves all events of a specific type.
    /// </summary>
    /// <param name="eventType">The type of event to filter</param>
    /// <returns>A collection of events of the specified type</returns>
    Task<IEnumerable<IEvent>> GetEventsByTypeAsync(string eventType);
    
    /// <summary>
    /// Retrieves all events that occurred within a specific time period.
    /// </summary>
    /// <param name="startDate">The start date of the period</param>
    /// <param name="endDate">The end date of the period</param>
    /// <returns>A collection of events that occurred between the specified dates</returns>
    Task<IEnumerable<IEvent>> GetEventsInPeriodAsync(DateTime startDate, DateTime endDate);
}

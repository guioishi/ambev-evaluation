using Ambev.DeveloperEvaluation.Common.Settings;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Repositories;

public class SaleEventMongoRepository : ISaleEventRepository
{
    private readonly IMongoCollection<BsonDocument> _eventsCollection;

    public SaleEventMongoRepository(IMongoDatabase database, MongoDBSettings settings)
    {
        _eventsCollection = database.GetCollection<BsonDocument>(settings.EventsCollectionName);
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var saleIdIndex = Builders<BsonDocument>.IndexKeys.Ascending("SaleId");
        var eventTypeIndex = Builders<BsonDocument>.IndexKeys.Ascending("EventType");
        var occurredOnIndex = Builders<BsonDocument>.IndexKeys.Ascending("OccurredOn");

        _eventsCollection.Indexes.CreateMany(new[]
        {
            new CreateIndexModel<BsonDocument>(saleIdIndex),
            new CreateIndexModel<BsonDocument>(eventTypeIndex),
            new CreateIndexModel<BsonDocument>(occurredOnIndex)
        });
    }

    public async Task AddAsync(IEvent @event)
    {
        var document = new BsonDocument
        {
            { "_id", ObjectId.GenerateNewId() },
            { "EventId", @event.EventId.ToString() },
            { "EventType", @event.EventType },
            { "OccurredOn", @event.OccurredOn },
            { "Data", SerializeEventData(@event) }
        };

        await _eventsCollection.InsertOneAsync(document);
    }

    public async Task<IEnumerable<IEvent>> GetEventsBySaleIdAsync(Guid saleId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("SaleId", saleId);
        var events = await _eventsCollection.Find(filter)
            .Sort(Builders<BsonDocument>.Sort.Ascending("OccurredOn"))
            .ToListAsync();

        return events.Select(DeserializeEvent);
    }

    public async Task<IEnumerable<IEvent>> GetEventsByTypeAsync(string eventType)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("EventType", eventType);
        var events = await _eventsCollection.Find(filter)
            .Sort(Builders<BsonDocument>.Sort.Ascending("OccurredOn"))
            .ToListAsync();

        return events.Select(DeserializeEvent);
    }

    public async Task<IEnumerable<IEvent>> GetEventsInPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Gte("OccurredOn", startDate),
            Builders<BsonDocument>.Filter.Lte("OccurredOn", endDate)
        );

        var events = await _eventsCollection.Find(filter)
            .Sort(Builders<BsonDocument>.Sort.Ascending("OccurredOn"))
            .ToListAsync();

        return events.Select(DeserializeEvent);
    }

    private IEvent DeserializeEvent(BsonDocument document)
    {
        var eventType = document["EventType"].AsString;
        var data = document["Data"].AsBsonDocument;

        return eventType switch
        {
            "SaleCreated" => new SaleCreatedEvent(
                Guid.Parse(data["SaleId"].AsString),
                data["CreatedDate"].ToUniversalTime())
            {
                EventId = Guid.Parse(document["EventId"].AsString),
                OccurredOn = document["OccurredOn"].ToUniversalTime()
            },

            _ => throw new InvalidOperationException($"Unknown event type: {eventType}")
        };
    }

    private BsonDocument SerializeEventData(IEvent @event)
    {
        var data = new BsonDocument();

        switch (@event)
        {
            case SaleCreatedEvent e:
                data.Add("SaleId", e.SaleId.ToString());
                data.Add("CreatedDate", e.CreatedDate);
                break;

            case SaleModifiedEvent e:
                data.Add("SaleId", e.SaleId.ToString());
                data.Add("ModifiedDate", e.ModifiedDate);
                data.Add("ModifiedBy", e.ModifiedBy);
                data.Add("ChangeDetails", e.ChangeDetails);
                break;

            case SaleCancelledEvent e:
                data.Add("SaleId", e.SaleId.ToString());
                data.Add("CancellationDate", e.CancellationDate);
                data.Add("CancelledBy", e.CancelledBy.ToString());
                data.Add("Reason", e.Reason);
                break;

            case ItemCancelledEvent e:
                data.Add("SaleId", e.SaleId.ToString());
                data.Add("ItemId", e.ItemId.ToString());
                data.Add("Quantity", e.Quantity);
                data.Add("CancellationDate", e.CancellationDate);
                data.Add("CancelledBy", e.CancelledBy);
                data.Add("Reason", e.Reason);
                break;

            default:
                throw new NotSupportedException($"Event type {@event.GetType().Name} not supported");
        }

        return data;
    }
}

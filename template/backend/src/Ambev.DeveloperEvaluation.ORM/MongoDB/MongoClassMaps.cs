using Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;
using MongoDB.Bson.Serialization;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB;

public static class MongoClassMaps
{
    public static void RegisterClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(SaleCreatedEvent)))
        {
            BsonClassMap.RegisterClassMap<SaleCreatedEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(e => new SaleCreatedEvent(e.SaleId, e.CreatedDate));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(SaleModifiedEvent)))
        {
            BsonClassMap.RegisterClassMap<SaleModifiedEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(e => new SaleModifiedEvent(
                    e.SaleId, e.ModifiedDate, e.ModifiedBy, e.ChangeDetails));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(SaleCancelledEvent)))
        {
            BsonClassMap.RegisterClassMap<SaleCancelledEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(e => new SaleCancelledEvent(
                    e.SaleId, e.CancellationDate, e.CancelledBy, e.Reason));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(ItemCancelledEvent)))
        {
            BsonClassMap.RegisterClassMap<ItemCancelledEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(e => new ItemCancelledEvent(
                    e.SaleId, e.ItemId, e.Quantity, e.CancellationDate,
                    e.CancelledBy, e.Reason));
            });
        }
    }
}

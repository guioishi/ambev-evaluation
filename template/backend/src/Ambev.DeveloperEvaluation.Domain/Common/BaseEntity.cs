using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Base class for all domain entities, providing common functionality for identity, 
/// audit timestamps, validation and comparison.
/// </summary>
public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last update to the entity, null if never updated
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Validates the entity asynchronously using the validation system
    /// </summary>
    /// <returns>Collection of validation errors, empty if validation succeeds</returns>
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
        => Validator.ValidateAsync(this);

    /// <summary>
    /// Implements comparison between entities based on their Id
    /// </summary>
    /// <param name="other">Entity to compare with</param>
    /// <returns>
    /// 1 if other is null;
    /// otherwise, result of Id comparison
    /// </returns>
    public int CompareTo(BaseEntity? other)
        => other is null ? 1 : other.Id.CompareTo(Id);

    /// <summary>
    /// Updates the UpdatedAt timestamp to current UTC time
    /// </summary>
    protected void MarkAsUpdated()
        => UpdatedAt = DateTime.UtcNow;
}

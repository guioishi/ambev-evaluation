namespace Ambev.DeveloperEvaluation.Domain.Entities.Sale;

/// <summary>
/// Represents immutable information about a branch where a sale occurred.
/// Part of the External Identities pattern for referencing entities from other domains.
/// </summary>
/// <param name="BranchId">The unique identifier of the branch</param>
/// <param name="BranchName">The display name of the branch</param>
public record BranchInfo(Guid BranchId, string BranchName);

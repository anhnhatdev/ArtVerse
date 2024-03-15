namespace ArtVerse.Domain.Common;

/// <summary>
/// Class cha cho tất cả Entity — chứa Id, timestamps, và soft delete.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public bool IsDeleted { get; protected set; } = false;
    public DateTimeOffset? DeletedAt { get; protected set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    protected void SetUpdated() => UpdatedAt = DateTimeOffset.UtcNow;
}

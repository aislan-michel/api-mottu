namespace Mottu.Api.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Active = true;
    }

    public string Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool Active { get; private set; } = false;

    public void SetCreatedAt(DateTime dateTime)
    {
        CreatedAt = dateTime;
    }

    public void SetUpdatedAt(DateTime dateTime)
    {
        UpdatedAt = dateTime;
    }
}
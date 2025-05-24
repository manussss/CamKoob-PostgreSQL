namespace CamKoob.PostgreSQL.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }

    public Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}
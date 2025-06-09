namespace Hotelium.Shared.Entities;

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    public virtual TPrimaryKey Id { get; set; }

    public virtual bool IsTransient()
    {
        if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default))
            return true;

        return typeof(TPrimaryKey) switch
        {
            Type t when t == typeof(int) => Convert.ToInt32(Id) <= 0,
            Type t when t == typeof(long) => Convert.ToInt64(Id) <= 0,
            _ => false
        };
    }
}
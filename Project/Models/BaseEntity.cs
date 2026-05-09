namespace Project.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public virtual string GetInfo()
    {
        return $"Entity Id: {Id}";
    }

}
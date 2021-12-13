using Core.SeedWork;

namespace Core.Domain;

public class Training : Entity, IAggregateRoot
{
    public string Name { get; set; }
}

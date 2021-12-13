using Core.SeedWork;

namespace Core.Domain;

public class Trainer : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public Trainer(string name)
    {
        Name = name;
    }
}

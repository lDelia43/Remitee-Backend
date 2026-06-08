using SweetMedical.Domain.Models;

namespace SweetMedical.Domain.AggregateModels;

public abstract class Person<TId> : AggregateRoot<TId> where TId : notnull
{
    public string Name { get; protected set; } = null!;

    protected Person(TId id, string name) : base(id)
    {
        Name = name;
    }

    protected Person() { }
}

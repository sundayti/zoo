namespace Domain.ValueObjects;

public sealed record AnimalId
{
    public Guid Value { get; }

    private AnimalId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("AnimalId не может быть пустым.", nameof(value));
        Value = value;
    }

    public static AnimalId New() => new(Guid.NewGuid());
    public static AnimalId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
namespace Domain.ValueObjects;

public sealed record EnclosureId
{
    public Guid Value { get; }

    private EnclosureId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("EnclosureId не может быть пустым.", nameof(value));
        Value = value;
    }

    public static EnclosureId New() => new(Guid.NewGuid());
    public static EnclosureId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}

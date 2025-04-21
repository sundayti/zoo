namespace Domain.ValueObjects;

public sealed record FoodType
{
    public string Value { get; }

    private FoodType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FoodType не может быть пустым.", nameof(value));
        Value = value;
    }

    /// <summary>Создать произвольный тип корма (например, "Meat", "Grass" и т.д.).</summary>
    public static FoodType From(string value) => new(value);

    public override string ToString() => Value;
}

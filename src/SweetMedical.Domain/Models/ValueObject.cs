namespace SweetMedical.Domain.Models;

public abstract class  ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }
        var valueObject = (ValueObject)obj;
        return valueObject.GetEqualityComponents().SequenceEqual(GetEqualityComponents());
    }

    public static bool operator ==(ValueObject leftObj, ValueObject rightObj)
    {
        return Equals(leftObj, rightObj);
    }
    
    public static bool operator !=(ValueObject leftObj, ValueObject rightObj)
    {
        return !Equals(leftObj, rightObj);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
    }
}
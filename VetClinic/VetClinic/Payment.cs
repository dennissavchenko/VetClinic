namespace VetClinic;

public enum Type { Cash, Card }

public class Payment: StoredObject<Payment>, IIdentifiable
{ 
    public int Id { get; set; }

    private int _amount;

    public int Amount
    {
        get => _amount;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Amount), "Amount must be greater than zero.");
            _amount = value;
        }
    }

    private Type _type;
    public Type Type
    {
        get => _type;
        set
        {
            if (!Enum.IsDefined(typeof(Type), value))
                throw new ArgumentException("Invalid type of payment.");
            _type = value;
        }
    }
    
    public DateTime DateTime { get; set; }
    
    public Payment() {}

    public Payment(int amount, Type paymentType, DateTime dateTime)
    {
        Amount = amount;
        Type = paymentType;
        DateTime = dateTime;
        AddToExtent(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, Amount={Amount}, Type={Type}, DateTime={DateTime}";
    }
    
}
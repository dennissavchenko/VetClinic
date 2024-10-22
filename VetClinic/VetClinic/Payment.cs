namespace VetClinic;

public enum Type { Cash, Card }

public class Payment: StoredObject<Payment>, IIdentifiable
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public Type Type { get; set; }
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
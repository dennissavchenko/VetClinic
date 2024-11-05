namespace VetClinic;

public enum PaymentType { Cash, Card }

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

    private PaymentType _paymentType;
    public PaymentType PaymentType
    {
        get => _paymentType;
        set
        {
            if (!Enum.IsDefined(typeof(PaymentType), value))
                throw new ArgumentException("Invalid type of payment.");
            _paymentType = value;
        }
    }
    
    public DateTime DateTime { get; set; }
    
    public Payment() {}

    public Payment(int amount, PaymentType paymentPaymentType, DateTime dateTime)
    {
        Amount = amount;
        PaymentType = paymentPaymentType;
        DateTime = dateTime;
        AddToExtent(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, Amount={Amount}, Type={PaymentType}, DateTime={DateTime}";
    }
    
}
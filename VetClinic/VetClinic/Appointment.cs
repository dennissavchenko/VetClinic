namespace VetClinic;

public class Appointment: StoredObject<Appointment>, IIdentifiable
{
    public int Id { get; set; }
    
    public DateTime DateTime { get; set; }

    private int _price;
    public int Price
    {
        get => _price;
        set
        {
            if(value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "Price must be greater than zero");
            _price = value;
        }
    }
    
    public Appointment() {}

    public Appointment(DateTime dateTime, int price)
    {
        DateTime = dateTime;
        Price = price;
        AddToExtent(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, DateTime={DateTime}, Price={Price}";
    }
    
}
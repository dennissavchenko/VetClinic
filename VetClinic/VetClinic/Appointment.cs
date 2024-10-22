namespace VetClinic;

public class Appointment: StoredObject<Appointment>, IIdentifiable
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int Price { get; set; }
    
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
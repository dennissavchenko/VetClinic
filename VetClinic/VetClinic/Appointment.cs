namespace VetClinic;

public enum AppointmentState { Scheduled, ClientWaiting, InProgress, Canceled, NoShow, Completed }
public class Appointment: StoredObject<Appointment>, IIdentifiable
{
    public int Id { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public AppointmentState State { get; set; }

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

    public Appointment(DateTime dateTime, AppointmentState state, int price)
    {
        DateTime = dateTime;
        State = state;
        Price = price;
        AddToExtent(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, DateTime={DateTime:yyyy-MM-ddTHH:mm:ss}, State={State.ToString()} Price={Price}";
    }
    
}
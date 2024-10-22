namespace VetClinic;

public class Prescription: StoredObject<Prescription>, IIdentifiable
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Prescription() {}

    public Prescription( DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
        AddToExtent(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, StartDate={StartDate.ToShortDateString()}, EndDate={EndDate.ToShortDateString()}";
    }

}


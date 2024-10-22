namespace VetClinic;

public class Dose : StoredObject<Dose>, IIdentifiable
{
    public int Id { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }

    public Dose() {}

    public Dose(string description, double amount)
    {
        Description = description;
        Amount = amount;
        AddToExtent(this);
    }

    public override string ToString()
    {
        return $"Description={Description}, Amount={Amount}";
    }
    
}
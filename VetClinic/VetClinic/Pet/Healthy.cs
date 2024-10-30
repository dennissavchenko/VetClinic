namespace VetClinic;

public enum ActivityLevel { Low, Medium, High }
public class Healthy : Pet
{
    public ActivityLevel ActivityLevel { get; set; }
    public DateTime LastVaccinationDate { get; set; }
    
    public Healthy(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, ActivityLevel activityLevel, DateTime lastVaccinationDate) : base(name, sex, weight, dateOfBirth, colors)
    {
        ActivityLevel = activityLevel;
        LastVaccinationDate = lastVaccinationDate;
        StoredObject<Healthy>.AddToExtent(this);
    }
    
    public Healthy() {}

    public override string ToString()
    {
        return "Healthy: " + base.ToString() + $", ActivityLevel={ActivityLevel.ToString()} LastVaccinationDate={LastVaccinationDate.ToShortDateString()}";
    }
    
    public new static List<Healthy> GetExtent()
    {
        return StoredObject<Healthy>.GetExtent();
    }
    
    public new static void PrintExtent()
    {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(nameof(Healthy) + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var healthy in GetExtent())
        {
            Console.WriteLine(healthy);
        }
    }
    
}
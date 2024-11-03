using VetClinic.Exceptions;

namespace VetClinic;

public enum ActivityLevel { Low, Medium, High }
public class Healthy : Pet
{
    public ActivityLevel ActivityLevel { get; set; }
    public DateTime? LastVaccinationDate { get; set; } // instead of nocturnal and canFly now lastVaccinationDate is optional
    
    public Healthy(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, ActivityLevel activityLevel, DateTime lastVaccinationDate) : base(name, sex, weight, dateOfBirth, colors)
    {
        ActivityLevel = activityLevel;
        if (lastVaccinationDate > DateTime.Now)
        {
            throw new InvalidDateException("Last vaccination date cannot be in the future.");
        }
        if (lastVaccinationDate < dateOfBirth)
        {
            throw new InvalidDateException("Last vaccination date cannot be before the date of birth.");
        }
        LastVaccinationDate = lastVaccinationDate;
        StoredObject<Healthy>.AddToExtent(this);
    }
    
    public Healthy() {}

    public override string ToString()
    {
        return "Healthy: " + base.ToString() + $", ActivityLevel={ActivityLevel.ToString()} LastVaccinationDate={LastVaccinationDate.ToString()}";
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
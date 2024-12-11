using VetClinic.Exceptions;

namespace VetClinic;

public enum ActivityLevel { Low, Medium, High }
public class Healthy : Pet
{
    public ActivityLevel ActivityLevel { get; set; }
    private DateTime? _lastVaccinationDate;
    public DateTime? LastVaccinationDate
    {
        get => _lastVaccinationDate;
        set
        {
            if (value > DateTime.Now)
            {
                throw new InvalidDateException("Last vaccination date cannot be in the future.");
            }
            if (value < DateOfBirth)
            {
                throw new InvalidDateException("Last vaccination date cannot be before the date of birth.");
            }

            _lastVaccinationDate = value;
        } 
    } // instead of nocturnal and canFly now lastVaccinationDate is optional
    
    public Healthy(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, ActivityLevel activityLevel, DateTime? lastVaccinationDate) : base(name, sex, weight, dateOfBirth, colors)
    {
        ActivityLevel = activityLevel;
        LastVaccinationDate = lastVaccinationDate;
        StoredObject<Healthy>.AddToExtent(this);
    }
    
    public Healthy() {}

    public override string ToString()
    {
        return "Healthy: " + base.ToString() +
               $", ActivityLevel={ActivityLevel.ToString()}, LastVaccinationDate={(LastVaccinationDate == null ? "NotVaccinated" : LastVaccinationDate.Value.ToString("yyyy-MM-dd"))}";
    }
    
    private new static List<Healthy> GetExtent()
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
    
    public new static List<string> GetExtentAsString()
    {
        List<string> list = new();
        foreach (var healthy in GetExtent())
        {
            list.Add(healthy.ToString());
        }
        return list;
    }
    
}
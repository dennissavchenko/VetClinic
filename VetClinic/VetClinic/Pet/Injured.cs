using VetClinic.Exceptions;

namespace VetClinic;

public enum InjuryType { Fracture, Wound, Sprain }
public class Injured : Pet
{
    public InjuryType InjuryType { get; set; }
    private DateTime _injuryDate;
    public DateTime InjuryDate
    {
        get => _injuryDate;
        set
        {
            if (value > DateTime.Now)
            {
                throw new InvalidDateException("Injury date cannot be in the future.");
            }
            if (value < DateOfBirth)
            {
                throw new InvalidDateException("Injury date cannot be before the date of birth.");
            }

            _injuryDate = value;
        } 
    }
    
    public Injured(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, Client client, InjuryType injuryType, DateTime injuryDate) : base(name, sex, weight, dateOfBirth, colors, client)
    {
        InjuryType = injuryType;
        InjuryDate = injuryDate;
        StoredObject<Injured>.AddToExtent(this);
    }
    
    public Injured(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, InjuryType injuryType, DateTime injuryDate) : base(name, sex, weight, dateOfBirth, colors)
    {
        InjuryType = injuryType;
        InjuryDate = injuryDate;
        StoredObject<Injured>.AddToExtent(this);
    }
    
    public Injured() {}

    public override string ToString()
    {
        return "Injured: " + base.ToString() + $", InjuryType={InjuryType.ToString()} InjuryDate={InjuryDate:yyyy-MM-dd}";
    }
    
    private new static List<Injured> GetExtent()
    {
        return StoredObject<Injured>.GetExtent();
    }
    
    public new static void PrintExtent()
    {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(nameof(Injured) + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var injured in GetExtent())
        {
            Console.WriteLine(injured);
        }
    }
    
    public new static List<string> GetExtentAsString()
    {
        List<string> list = new();
        foreach (var injured in GetExtent())
        {
            list.Add(injured.ToString());
        }
        return list;
    }
    
}
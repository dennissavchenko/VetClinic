namespace VetClinic;

public enum InjuryType { Fracture, Wound, Sprain }
public class Injured : Pet
{
    public InjuryType InjuryType { get; set; }
    public DateTime InjuryDate { get; set; }
    
    public Injured(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, InjuryType injuryType, DateTime injuryDate) : base(name, sex, weight, dateOfBirth, colors)
    {
        InjuryType = injuryType;
        InjuryDate = injuryDate;
        StoredObject<Injured>.AddToExtent(this);
    }
    
    public Injured() {}

    public override string ToString()
    {
        return "Injured: " + base.ToString() + $", InjuryType={InjuryType.ToString()} InjuryDate={InjuryDate.ToShortDateString()}";
    }
    
    public new static List<Injured> GetExtent()
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
    
}
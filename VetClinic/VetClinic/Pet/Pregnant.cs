using VetClinic.Exceptions;

namespace VetClinic;

public class Pregnant : Pet
{
    public DateTime DueDate { get; set; }
    public int LitterSize { get; set; }
    
    public Pregnant(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, DateTime dueDate, int litterSize) : base(name, sex, weight, dateOfBirth, colors)
    {
        if (dueDate < dateOfBirth)
        {
            throw new InvalidDateException("Due date cannot be before the date of birth.");
        }
        if (dueDate < DateTime.Now)
        {
            throw new InvalidDateException("Due date cannot be in the past.");
        }
        DueDate = dueDate;
        if (litterSize <= 0)
        {
            throw new NegativeValueException("Expected litter size must be greater than 0.");
        }
        LitterSize = litterSize;
        StoredObject<Pregnant>.AddToExtent(this);
    }
    
    public Pregnant() {}

    public override string ToString()
    {
        return "Pregnant: " + base.ToString() + $", DueDate={DueDate.ToShortDateString()} LitterSize={LitterSize}";
    }
    
    public new static List<Pregnant> GetExtent()
    {
        return StoredObject<Pregnant>.GetExtent();
    }
    
    public new static void PrintExtent()
    {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(nameof(Pregnant) + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var pregnant in GetExtent())
        {
            Console.WriteLine(pregnant);
        }
    }
    
}
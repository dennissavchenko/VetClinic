using VetClinic.Exceptions;

namespace VetClinic;

public class Pregnant : Pet
{
    private DateTime _dueDate;
    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            if (value < DateOfBirth)
            {
                throw new InvalidDateException("Due date cannot be before the date of birth.");
            }
            _dueDate = value;
        } 
    }
    private int _litterSize;
    public int LitterSize
    {
        get => _litterSize;
        set
        {
            if (value <= 0)
            {
                throw new NegativeValueException("Expected litter size must be greater than 0.");
            } 
            _litterSize = value;
        } 
    }
    
    public Pregnant(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, DateTime dueDate, int litterSize) : base(name, sex, weight, dateOfBirth, colors)
    {
        DueDate = dueDate;
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
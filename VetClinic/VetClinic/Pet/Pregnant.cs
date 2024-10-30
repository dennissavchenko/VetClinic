namespace VetClinic;

public class Pregnant : Pet
{
    public DateTime DueDate { get; set; }
    public int LitterSize { get; set; }
    
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
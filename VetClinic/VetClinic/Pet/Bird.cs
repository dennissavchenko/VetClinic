namespace VetClinic;

public class Bird : Pet
{
    
    public double WingsSpan { get; set; }
    public bool CanFly { get; set; }
    
    public Bird(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, double wingsSpan, bool canFly) : base(name, sex, weight, dateOfBirth, colors)
    {
        WingsSpan = wingsSpan;
        CanFly = canFly;
        StoredObject<Bird>.AddToExtent(this);
    }
    
    public Bird() {}

    public override string ToString()
    {
        return "Bird: " + base.ToString() + $", WingsSpan={WingsSpan}, CanFly={CanFly}";
    }
    
    public new static List<Bird> GetExtent()
    {
        return StoredObject<Bird>.GetExtent();
    }
    
    public new static void PrintExtent()
    {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(nameof(Bird) + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var bird in GetExtent())
        {
            Console.WriteLine(bird);
        }
    }
    
}
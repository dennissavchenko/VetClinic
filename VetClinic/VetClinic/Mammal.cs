namespace VetClinic;

public class Mammal : Pet
{
    public bool Nocturnal { get; set; }
    
    public Mammal(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, bool nocturnal) : base(name, sex, weight, dateOfBirth, colors)
    {
        Nocturnal = nocturnal;
        StoredObject<Mammal>.AddToExtent(this);
    }
    
    public Mammal() {}

    public override string ToString()
    {
        return "Mammal: " + base.ToString() + $", Nocturnal={Nocturnal}";
    }
    
    public static List<Mammal> GetExtent()
    {
        return StoredObject<Mammal>.GetExtent();
    }
    
    public static void PrintExtent()
    {
        foreach (var mammal in GetExtent())
        {
            Console.WriteLine(mammal);
        }
    }
    
}
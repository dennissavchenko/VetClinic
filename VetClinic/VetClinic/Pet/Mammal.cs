using System.Text;

namespace VetClinic;

public class Mammal : Pet
{
    public bool Nocturnal { get; set; } // it is just boolean now, not optional
    
    public Mammal(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, Client client, bool nocturnal) : base(name, sex, weight, dateOfBirth, colors, client)
    {
        Nocturnal = nocturnal;
        StoredObject<Mammal>.AddToExtent(this);
    }
    
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
    
    private new static List<Mammal> GetExtent()
    {
        return StoredObject<Mammal>.GetExtent();
    }
    
    public new static void PrintExtent()
    {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(nameof(Mammal) + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var mammal in GetExtent())
        {
            Console.WriteLine(mammal);
        }
    }
    
    public new static List<string> GetExtentAsString()
    {
        List<string> list = new();
        foreach (var mammal in GetExtent())
        {
            list.Add(mammal.ToString());
        }
        return list;
    }
    
}
using VetClinic.Exceptions;

namespace VetClinic;

public enum WaterType { Freshwater, Saltwater, Brackish }

public class Fish : Pet
{
    
    public WaterType WaterType { get; set; }
    private double _waterTemperature;
    public double WaterTemperature
    {
        get => _waterTemperature;
        set
        {
            if (value <= 0)
            {
                throw new NegativeValueException("Water temperature must be positive.");
            }
            _waterTemperature = value;
        } 
    }
    
    public Fish(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, Client client, WaterType waterType, double waterTemperature) : base(name, sex, weight, dateOfBirth, 
        colors, client)
    {
        WaterType = waterType;
        WaterTemperature = waterTemperature;
        StoredObject<Fish>.AddToExtent(this);
    }
    
    public Fish(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, WaterType waterType, double waterTemperature) : base(name, sex, weight, dateOfBirth, 
        colors)
    {
        WaterType = waterType;
        WaterTemperature = waterTemperature;
        StoredObject<Fish>.AddToExtent(this);
    }
    
    public Fish() {}

    public override string ToString()
    {
        return "Fish: " + base.ToString() + $", WaterType={WaterType}, WaterTemperature={WaterTemperature.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
    }
    
    private new static List<Fish> GetExtent()
    {
        return StoredObject<Fish>.GetExtent();
    }
    
    public new static void PrintExtent()
    {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(nameof(Fish) + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var fish in GetExtent())
        {
            Console.WriteLine(fish);
        }
    }
    
    public new static List<string> GetExtentAsString()
    {
        List<string> list = new();
        foreach (var fish in GetExtent())
        {
            list.Add(fish.ToString());
        }
        return list;
    }
    
}
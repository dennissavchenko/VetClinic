namespace VetClinic;

public enum WaterType { Freshwater, Saltwater, Brackish }

public class Fish : Pet
{
    
    public WaterType WaterType { get; set; }
    public double WaterTemperature { get; set; }
    
    public Fish(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, WaterType waterType, double waterTemperature) : base(name, sex, weight, dateOfBirth, colors)
    {
        WaterType = waterType;
        WaterTemperature = waterTemperature;
        StoredObject<Fish>.AddToExtent(this);
    }
    
    public Fish() {}

    public override string ToString()
    {
        return "Fish: " + base.ToString() + $", WaterType={WaterType}, WaterTemperature={WaterTemperature}";
    }
    
    public static List<Fish> GetExtent()
    {
        return StoredObject<Fish>.GetExtent();
    }
    
    public static void PrintExtent()
    {
        foreach (var fish in GetExtent())
        {
            Console.WriteLine(fish);
        }
    }
    
}
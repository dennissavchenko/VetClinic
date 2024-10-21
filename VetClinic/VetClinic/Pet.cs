namespace VetClinic;

public enum Sex { Male, Female }

public enum Color { Black, White, Brown, Gray, Golden, Red, Blue, Cream, Yellow, Green }
public class Pet: StoredObject<Pet>, IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Sex Sex { get; set; }
    public Double Weight { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<Color> Colors { get; set; }
    public int Age
    {
        get {
            return DateTime.Now.Year - DateOfBirth.Year;
        }
    }
    
    public Pet() {}

    public Pet(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors)
    {
        Name = name;
        Sex = sex;
        Weight = weight;
        DateOfBirth = dateOfBirth;
        Colors = colors;
        AddToExtent(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Sex={Sex}, Weight={Weight}, DateOfBirth={DateOfBirth.ToShortDateString()}, Colors=({string.Join(", ", Colors)}), Age={Age}";
    }
}
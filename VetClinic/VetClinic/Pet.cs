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
    public int SpecieId { get; set; }
    public List<Color> Colors { get; set; }
    public int Age
    {
        get {
            return DateTime.Now.Year - DateOfBirth.Year;
        }
    }
    
    public Pet() {}

    public Pet(string name, Sex sex, double weight, DateTime dateOfBirth, Specie specie, List<Color> colors)
    {
        Name = name;
        Sex = sex;
        Weight = weight;
        DateOfBirth = dateOfBirth;
        SpecieId = specie.Id;
        Colors = colors;
        AddToExtent(this);
        specie.AddPetId(Id);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Sex={Sex}, Weight={Weight}, DateOfBirth={DateOfBirth.ToShortDateString()}, Specie={Specie.GetSpecieById(SpecieId).Name}, Colors=({string.Join(", ", Colors)}), Age={Age}";
    }
}
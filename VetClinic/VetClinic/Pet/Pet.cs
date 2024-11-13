using VetClinic.Exceptions;

namespace VetClinic;

public enum Sex { Male, Female }

public enum Color { Black, White, Brown, Gray, Golden, Red, Blue, Cream, Yellow, Green }
public class Pet: StoredObject<Pet>, IIdentifiable
{
    public int Id { get; set; }
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new EmptyStringException("Pet's name cannot be empty!");
            }
            _name = value;
        } 
    }
    public Sex Sex { get; set; }
    private double _weight;
    public double Weight
    {
        get => _weight;
        set
        {
            if (value <= 0)
            {
                throw new NegativeValueException("Pet's weight must be greater than 0!");
            }
            _weight = value;
        } 
    }
    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if(value > DateTime.Now)
            {
                throw new InvalidDateException("Pet's date of birth cannot be in the future!");
            }
            _dateOfBirth = value;
        } 
    }
    public List<Color> Colors { get; set; } 
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
    
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
        return $"Id={Id}, Name={Name}, Sex={Sex}, Weight={Weight}, DateOfBirth={DateOfBirth:M/d/yyyy}, Colors=({string.Join(", ", Colors)}), Age={Age}";
    }
}
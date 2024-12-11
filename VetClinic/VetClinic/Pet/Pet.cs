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
            if (value > DateTime.Now)
            {
                throw new InvalidDateException("Pet's date of birth cannot be in the future!");
            }

            _dateOfBirth = value;
        }
    }
    private List<Color> _colors;
    public List<Color> Colors
    {
        get => _colors;
        set
        {
            if (value == null || value.Count == 0)
            {
                throw new EmptyListException("Pet must have at least one color!");
            }

            if (value.Count != value.Distinct().Count())
            {
                throw new DuplicatesException("Duplicate colors are not allowed!");
            }

            _colors = value;
        }
    }
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
    
    private Specie? _specie;
    
    private static List<Pet> _extent = new();
    
    public Specie? GetSpecie()
    {
        return _specie;
    }
    
    public void AddSpecie(Specie specie)
    {
        if (_specie != specie && _specie != null) _specie.RemovePet(this);
        _specie = specie;
        if (!_specie.GetPets().Contains(this)) _specie.AddPet(this);
    }
    
    public void RemoveSpecie()
    {
        if (_specie == null) throw new NullReferenceException("The pet is not assigned to any specie.");
        if(_specie.GetPets().Contains(this)) _specie.RemovePet(this);
        _specie = null;
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
        _extent.Add(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Sex={Sex}, Weight={Weight.ToString(System.Globalization.CultureInfo.InvariantCulture)}, DateOfBirth={DateOfBirth:yyyy-MM-dd}, Colors=({string.Join(", ", Colors)}), Age={Age}";
    }

    public static void PrintPetExtent()
    {
        Console.WriteLine("------------------------------------------------");
        foreach (var pet in _extent)
        {
            Console.WriteLine(pet);
        }
    }
    
}
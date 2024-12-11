using VetClinic.Exceptions;

namespace VetClinic;

/*THIS CLASS IS GOING TO BE INSTEAD OF STATUS; IT WILL HAVE THE SAME KIND OF ASSOCIATION WITH PET AS STATUS HAD*/

public class Specie: StoredObject<Specie>, IIdentifiable
{
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            _id = value;
        }
    }
    
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
        }
    }

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
        }
    }

    private List<Pet> _pets = new();
    
    private static List<Specie> _extent = new();

    public List<Pet> GetPets()
    {
        return new List<Pet>(_pets);
    }
    
    public void AddPet(Pet pet)
    {
        if (_pets.Contains(pet)) throw new DuplicatesException("Pet already exists in the list.");
        _pets.Add(pet);
        if (pet.GetSpecie() != this) pet.AddSpecie(this);
    }
    
    public void RemovePet (Pet pet)
    {
        if (!_pets.Contains(pet)) throw new NotFoundException("Pet not found in the list.");
        _pets.Remove(pet);
        if (pet.GetSpecie() != null) pet.RemoveSpecie();
    }
    
    public Specie() {}
    
    public Specie(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new EmptyStringException("Name cannot be empty!");
        if (string.IsNullOrWhiteSpace(description)) throw new EmptyStringException("Description cannot be empty!");
        Name = name;
        Description = description;
        AddToExtent(this);
        _extent.Add(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Description={Description}";
    }

    public static void PrintSpecieExtent()
    {
        Console.WriteLine("------------------------------------------------");
        foreach (var specie in _extent)
        {
            Console.WriteLine(specie);
            foreach (var pet in specie._pets)
            {
                Console.WriteLine("-->" + pet);
            }
        }
    }
    
}
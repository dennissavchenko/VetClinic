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

    public List<Pet> GetPets()
    {
        return new List<Pet>(_pets);
    }
    
    /// <summary>
    /// Adds the specified Pet to this Specie's list, maintaining bidirectional consistency.
    /// If the Pet is already in the list, a DuplicatesException is thrown.
    /// </summary>
    public void AddPet(Pet pet)
    {
        // Throw DuplicatesException if the pet already exists in _pets.
        if (_pets.Contains(pet)) 
            throw new DuplicatesException("Pet already exists in the list.");

        // Add the Pet to this Specie's collection.
        _pets.Add(pet);

        // Ensure the Pet also recognizes this Specie by calling pet.AddSpecie(this).
        // This maintains bidirectional consistency: Pet -> Specie, Specie -> Pet.
        if (pet.GetSpecie() != this) 
            pet.AddSpecie(this);
    }

    /// <summary>
    /// Removes the specified Pet from this Specie's list, ensuring bidirectional consistency.
    /// If the Pet does not exist in the list, a NotFoundException is thrown.
    /// </summary>
    public void RemovePet(Pet pet)
    {
        // Throw NotFoundException if the pet is not in the _pets list.
        if (!_pets.Contains(pet)) 
            throw new NotFoundException("Pet not found in the list.");

        // Remove the Pet from this Specie's collection.
        _pets.Remove(pet);

        // If the Pet is still associated with a Specie (which should be this one),
        // call pet.RemoveSpecie() to clear that association from the Pet side as well.
        if (pet.GetSpecie() != null) 
            pet.RemoveSpecie();
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
    
    /// <summary>
    /// Removes this Specie entirely from the system, along with disassociating every Pet that belongs to it.
    /// Each Pet is updated so it no longer references this Specie, ensuring bidirectional consistency.
    /// </summary>
    public void RemoveSpecie()
    {
        // Throw NotFoundException if this Specie is not found in the global extent.
        if (!_extent.Contains(this)) 
            throw new NotFoundException("Specie not found in the list.");

        // Copy the list of Pets so we can modify _pets as we iterate.
        var pets = new List<Pet>(_pets);

        // For each Pet that belongs to this Specie, call pet.RemoveSpecie(),
        // which breaks the association from the Pet's side.
        foreach (var pet in pets)
        {
            pet.RemoveSpecie();
        }

        // Finally, remove this Specie from the global extent.
        _extent.Remove(this);
    }
    
}
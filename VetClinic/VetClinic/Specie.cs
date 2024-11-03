using VetClinic.Exceptions;

namespace VetClinic;

/*THIS CLASS IS GOING TO BE INSTEAD OF STATUS; IT WILL HAVE THE SAME KIND OF ASSOCIATION WITH PET AS STATUS HAD*/

public class Specie: StoredObject<Specie>, IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Specie() {}
    
    public Specie(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new EmptyStringException("Name cannot be empty!");
        if (string.IsNullOrWhiteSpace(description)) throw new EmptyStringException("Description cannot be empty!");
        Name = name;
        Description = description;
        AddToExtent(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Description={Description}";;
    }
    
}
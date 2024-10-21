namespace VetClinic;

public class Specie: StoredObject<Specie>, IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> PetsIds { get; set; }
    
    public Specie() {}
    
    public Specie(string name, string description)
    {
        Name = name;
        Description = description;
        PetsIds = new List<int>();
        AddToExtent(this);
    }
    
    public static Specie GetSpecieById(int id)
    {
        var specie = GetExtent().Find(specie => specie.Id == id);
        if(specie == null)
        {
            throw new Exception($"Specie with id {id} not found");
        }
        return specie;
    }
    
    public void AddPetId(int petId)
    {
        var species = GetExtent();
        species[species.FindIndex(x => x.Id == Id)].PetsIds.Add(petId);
        UpdateExtent(species);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Description={Description}";;
    }
}
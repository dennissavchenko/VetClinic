namespace VetClinic;

public enum Form { Pill, Injection, Cream, Powder }

public class Medication : StoredObject<Medication>, IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Form Form { get; set; }
        
    public Medication() { }
        
    public Medication(string name, Form form)
    {
        Name = name;
        Form = form;
        AddToExtent(this);
    }
        
    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Form={Form}";
    }
        
}

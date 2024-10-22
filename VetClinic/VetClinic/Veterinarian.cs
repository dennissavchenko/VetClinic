namespace VetClinic;

public class Veterinarian: StoredObject<Veterinarian>, IIdentifiable
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Specialization { get; set; }
    public string ExperienceLevel { get; set; }
    
    public static int MaxAppointmentsPerDay = 8;
    
    public Veterinarian() {}

    public Veterinarian(string firstName, string lastName, string phoneNumber, string email, string specialization, string experienceLevel)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Specialization = specialization;
        ExperienceLevel = experienceLevel;
        AddToExtent(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, FirstName={FirstName}, LastName={LastName}, PhoneNumber={PhoneNumber}, Email={Email}, Specialization={Specialization}, ExperienceLevel={ExperienceLevel}, MaxAppointmentsPerDay={MaxAppointmentsPerDay}";
    }
}
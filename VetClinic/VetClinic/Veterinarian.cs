using VetClinic.Exceptions;

namespace VetClinic;

public enum ExperienceLevel { Intern, Junior, Intermediate, Advanced, Senior }
public enum Specialization { Surgery, Radiology, Dentistry, Ophthalmology, Dermatology }

public class Veterinarian: StoredObject<Veterinarian>, IIdentifiable
{
    public int Id { get; set; }

    private string _firstName;
    public string FirstName
    {
        get => _firstName;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("FirstName can't be empty");
            _firstName = value;
            
        }
    }

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("LastName can't be empty");
            _lastName = value; 
        }
    }

    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("PhoneNumber can't be empty.");
            if (value.Length != 9)
                throw new InvalidFormatException("PhoneNumber must have 9 digits.");
            _phoneNumber = value;
        }
    }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("Email can't be empty.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new InvalidFormatException("Email must be a valid format.");
            _email = value;
        }
    }
    
    public Specialization Specialization { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    
    private static readonly int MaxAppointmentsPerDay = 8;
    
    public Veterinarian() {}

    public Veterinarian(string firstName, string lastName, string phoneNumber, string email, Specialization specialization, ExperienceLevel experienceLevel)
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
        return $"Id={Id}, FirstName={FirstName}, LastName={LastName}, PhoneNumber={PhoneNumber}, Email={Email}, Specialization={Specialization.ToString()}, ExperienceLevel={ExperienceLevel.ToString()}, MaxAppointmentsPerDay={MaxAppointmentsPerDay}";
    }
}
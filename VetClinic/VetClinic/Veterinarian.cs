using VetClinic.Exceptions;

namespace VetClinic;

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

    private string _specialization;
    public string Specialization
    {
        get => _specialization;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("Specialization can't be empty.");
            _specialization = value;
        }
    }

    private string _experienceLevel;
    public string ExperienceLevel
    {
        get => _experienceLevel;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("Experience level can't be empty.");
            _experienceLevel = value;
        }
    }
    
    private static readonly int MaxAppointmentsPerDay = 8;
    
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
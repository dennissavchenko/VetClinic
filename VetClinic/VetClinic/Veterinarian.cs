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

    private List<Appointment> _appointments = new List<Appointment>();

    /// <summary>
    /// Retrieves a copy of the appointments list for this Veterinarian.
    /// </summary>
    public List<Appointment> GetAppointments()
    {
        if (_appointments.Count == 0)
            throw new EmptyListException("This Veterinarian has no appointments.");

        return new List<Appointment>(_appointments);
    }

    /// <summary>
    /// Adds an appointment to this Veterinarian and establishes the reverse connection.
    /// </summary>
    public void AddAppointment(Appointment appointment)
    {
        if (appointment == null)
            throw new NullReferenceException("Appointment can't be null.");

        if (_appointments.Contains(appointment))
            throw new DuplicatesException("Appointment is already associated with this Veterinarian.");
        
        _appointments.Add(appointment);

        // Set the reverse connection if it's not already established
        if (appointment.GetVeterinarian() != this)
        {
            appointment.SetVeterinarian(this);
        }    
    }

    /// <summary>
    /// Removes and appointment from this Veterinarian and updates the reverse connection.
    /// </summary>
    public void RemoveAppointment(Appointment appointment)
    {
        if (appointment == null)
            throw new NullReferenceException("Appointment can't be null.");

        if (_appointments.Contains(appointment))
            throw new DuplicatesException("Appointment is already associated with this Veterinarian.");

        _appointments.Remove(appointment);
        
        // Remove the reverse connection
        if (appointment.GetVeterinarian() == this)
        {
            appointment.ClearVeterinarian();
        }    
    }

    private List<Prescription> _prescriptions = new List<Prescription>();

    /// <summary>
    /// Retrieves a copy of the prescriptions list for this Veterinarian.
    /// </summary>
    public List<Prescription> GetPrescriptions()
    {
        if (_prescriptions.Count == 0)
            throw new EmptyListException("This Veterinarian has no prescriptions");

        return new List<Prescription>(_prescriptions);
    }

    /// <summary>
    /// Adds a prescription to this Veterinarian and establishes the reverse connection.
    /// </summary>
    public void AddPrescription(Prescription prescription)
    {
        if (prescription == null)
            throw new NullReferenceException("Prescription can't be null.");

        if (_prescriptions.Contains(prescription))
            throw new DuplicatesException("Prescription is already associated with this Veterinarian.");
        
        _prescriptions.Add(prescription);
        
        // Set reverse connection if it's not already established
        if (prescription.GetVeterinarian() != this)
        {
            prescription.SetVeterinarian(this);
        }    
    }
    
    /// <summary>
    /// Removes the prescription from this Veterinarian and updates the reverse connection.
    /// </summary>
    public void RemovePrescription(Prescription prescription)
    {
        if (prescription == null)
            throw new NullReferenceException("Prescription can't be null.");

        if (!_prescriptions.Contains(prescription))
            throw new NotFoundException("Prescription is not associated this Veterinarian.");
        
        _prescriptions.Remove(prescription);
        
        // Remove the reverse connection
        if (prescription.GetVeterinarian() == this)
        {
            prescription.ClearVeterinarian();
        }    
    }
    
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
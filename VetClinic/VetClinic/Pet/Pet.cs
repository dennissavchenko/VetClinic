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

    private Client _client;
    
    public Specie? GetSpecie()
    {
        return _specie;
    }
    
    public  Client GetClient()
    {
        return _client;
    }
    
    // Qualified association: A Pet can have multiple Appointments keyed by Appointment.Id
    private Dictionary<int, Appointment> _appointments = new();

    /// <summary>
    /// Returns the list of all appointments for this Pet.
    /// </summary>
    public List<Appointment> GetAppointments()
    {
        return _appointments.Values.ToList(); 
    }

    /// <summary>
    /// Retrieves a single Appointment by its Id (the qualifier).
    /// </summary>
    public Appointment GetAppointmentById(int appointmentId)
    {
        if (!_appointments.ContainsKey(appointmentId)) throw new NotFoundException($"No appointment found with Id {appointmentId} for this pet.");
        return _appointments[appointmentId];
    }

    /// <summary>
    /// Adds an Appointment to this Pet's dictionary, keyed by Appointment.Id.
    /// Pet can have multiple appointments, but each Appointment.Id is unique in this dictionary.
    /// </summary>
    public void AddAppointment(Appointment appointment)
    {
        if (appointment == null) throw new NullReferenceException("Appointment cannot be null.");

        if (_appointments.ContainsKey(appointment.Id)) throw new DuplicatesException($"Pet already has an appointment with Id {appointment.Id}.");

        // Store the appointment keyed by its Id
        _appointments[appointment.Id] = appointment;

        // Enforce bidirectional reference
        if (appointment.GetPet() != this) appointment.AssignPet(this);
    }

    /// <summary>
    /// Removes the Appointment identified by the given appointmentId from this Pet.
    /// </summary>
    public void RemoveAppointment(int appointmentId)
    {
        if (!_appointments.ContainsKey(appointmentId))
            throw new NotFoundException($"No appointment found with Id {appointmentId} to remove.");

        var appointment = _appointments[appointmentId];
        _appointments.Remove(appointmentId);

        // Maintain bidirectional consistency
        if (appointment.GetPet() == this) appointment.RemovePet();
    }

    /// <summary>
    /// Checks if this Pet has any appointments.
    /// </summary>
    public bool HasAppointments()
    {
        return _appointments.Count > 0;
    }

    /// <summary>
    /// Associates this Pet with a Specie, ensuring bidirectional consistency.
    /// If the Pet is already associated with a different Specie, it will be removed from that Specie first.
    /// </summary>
    public void AddSpecie(Specie specie)
    {
        // If the Pet is already associated with a different Specie, remove it from that Specie first.
        if (_specie != specie && _specie != null) 
        {
            _specie.RemovePet(this); // Removes this Pet from the previous Specie's list of pets.
        }

        // Update the Specie reference for this Pet.
        _specie = specie;

        // Add this Pet to the new Specie's list if it is not already in that list.
        if (!_specie.GetPets().Contains(this)) 
        {
            _specie.AddPet(this); // Ensures the Specie object recognizes this Pet in its list.
        }
    }

    /// <summary>
    /// Removes the association between this Pet and its Specie, if any.
    /// Ensures bidirectional consistency by removing this Pet from the Specie's list of pets.
    /// </summary>
    public void RemoveSpecie()
    {
        // If the Pet is not associated with any Specie, throw an exception.
        if (_specie == null) 
        {
            throw new NullReferenceException("The pet is not assigned to any specie.");
        }

        // Remove this Pet from the Specie's list of pets if it is in that list.
        if (_specie.GetPets().Contains(this)) 
        {
            _specie.RemovePet(this); // Updates the Specie to no longer track this Pet.
        }

        // Clear the Specie reference for this Pet.
        _specie = null;
    }

    /// <summary>
    /// Updates the association between this Pet and a new Client, ensuring bidirectional consistency.
    /// Removes this Pet from the previous Client's list (if any) and adds it to the new Client's list.
    /// </summary>
    public void ModifyClient(Client client)
    {
        // If the Pet is currently associated with a different Client, remove it from that Client's list.
        if (_client != null! && _client.GetPets().Contains(this)) 
        {
            _client.RemovePet(this); // Ensures the previous Client no longer tracks this Pet.
        }

        // Update the Client reference for this Pet.
        _client = client;

        // Add this Pet to the new Client's list if it is not already in that list.
        if (!_client.GetPets().Contains(this)) 
        {
            _client.AddPet(this); // Ensures the new Client tracks this Pet in its list.
        }
    }

    /// <summary>
    /// Removes this Pet from the system, including its associations with its Specie and Client.
    /// Ensures bidirectional consistency by removing this Pet from associated Specie and Client lists.
    /// </summary>
    public void RemovePet()
    {
        // If the Pet is not found in the global extent of Pets, throw an exception.
        if (!_extent.Contains(this)) 
        {
            throw new NotFoundException("Pet not found in the list.");
        }

        // If the Pet is associated with a Specie, remove it from that Specie's list.
        if (_specie != null) 
        {
            RemoveSpecie(); // Ensures the Specie no longer tracks this Pet.
        }

        // If the Pet is associated with a Client, remove it from that Client's list.
        if (_client.GetPets().Contains(this)) 
        {
            _client.RemovePet(this); // Ensures the Client no longer tracks this Pet.
        }

        // Remove this Pet from the global extent of Pets.
        _extent.Remove(this);
    }
    
    public Pet() {}

    public Pet(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors, Client client)
    {
        Name = name;
        Sex = sex;
        Weight = weight;
        DateOfBirth = dateOfBirth;
        Colors = colors;
        ModifyClient(client); // Establishes the association between the Pet and the specified Client.
        AddToExtent(this);
        _extent.Add(this);
    }
    
    public Pet(string name, Sex sex, double weight, DateTime dateOfBirth, List<Color> colors)
    {
        Name = name;
        Sex = sex;
        Weight = weight;
        DateOfBirth = dateOfBirth;
        Colors = colors;
        _client = new Client(); // Creates a dummy client for the Pet.
        _client.AddPet(this); // Adds this Pet to the dummy client's list, ensuring consistency.
        AddToExtent(this);
        _extent.Add(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Sex={Sex}, Weight={Weight.ToString(System.Globalization.CultureInfo.InvariantCulture)}, DateOfBirth={DateOfBirth:yyyy-MM-dd}, Colors=({string.Join(", ", Colors)}), Age={Age}";
    }
    
}
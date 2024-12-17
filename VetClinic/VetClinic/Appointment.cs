using System.Linq;
using VetClinic.Exceptions;

namespace VetClinic;

public enum AppointmentState { Scheduled, ClientWaiting, InProgress, Canceled, NoShow, Completed }
public class Appointment : StoredObject<Appointment>, IIdentifiable
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public AppointmentState State { get; set; }

    private int _price;
    public int Price
    {
        get => _price;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "Price must be greater than zero");
            _price = value;
        }
    }

    private Pet? _pet;  // One Appointment → One Pet, but can start null before assignment

    public Pet? GetPet()
    {
        return _pet;
    }

    /// <summary>
    /// Assigns this Appointment to a given Pet (1..* from Pet’s perspective, but 1..1 from Appointment’s perspective).
    /// </summary>
    public void AssignPet(Pet pet)
    {
        if (pet == null) throw new NullReferenceException("Pet cannot be null.");

        // Prevent reassigning this appointment to a different pet
        if (_pet != null && _pet != pet) throw new InvalidOperationException("This appointment is already assigned to another pet.");

        _pet = pet;

        // Make sure Pet has this appointment in its qualified dictionary
        if (!pet.GetAppointments().Contains(this)) pet.AddAppointment(this);
    }

    /// <summary>
    /// Unassigns this Appointment from its Pet.
    /// </summary>
    public void RemovePet()
    {
        if (_pet == null) throw new InvalidOperationException("This appointment is not assigned to any pet.");

        var pet = _pet;
        _pet = null;

        // Synchronize removal on Pet side
        if (pet.GetAppointments().Contains(this))
        {
            pet.RemoveAppointment(Id);
        }
    }
    
    private List<Payment> _payments = new();

    public List<Payment> GetPayments()
    {
        return new List<Payment>(_payments);
    }
    
    public void AddPayment(Payment payment)
    {
     if (payment == null) throw new NullReferenceException("Payment cannot be null.");

        if (_payments.Contains(payment)) throw new DuplicatesException("Payment already exists in the list.");

        _payments.Add(payment);
        
        if (!payment.GetAppointment().Equals(this)) payment.AddAppointment(this);
    }

    public void RemovePayment(Payment payment)
    {
        if (payment == null)
            throw new NullReferenceException("Payment cannot be null.");

        if (!_payments.Contains(payment))
            throw new NotFoundException("Payment not found in the Appointment.");

        _payments.Remove(payment);
        
        if (payment.GetAppointment() == this) payment.RemovePayment();
    }

    // One-to-one relationship from Appointment
    private Veterinarian? _veterinarian; 
    
    /// <summary>
    /// Gets the Veterinarian associated with this Appointment.
    /// </summary>
    public Veterinarian? GetVeterinarian()
    {
        return _veterinarian;
    }

    /// <summary>
    /// Sets the Veterinarian for this Appointment and ensures the reverse connection.
    /// </summary>
    public void AddVeterinarian(Veterinarian veterinarian)
    {
        if (veterinarian == null) throw new NullReferenceException("Veterinarian can't be null.");

        if (_veterinarian != null && _veterinarian != veterinarian) throw new MethodMisuseException("This appointment is already assigned to another Veterinarian.");

        _veterinarian = veterinarian;
        
        if (!veterinarian.GetAppointments().Contains(this)) veterinarian.AddAppointment(this);
    }

    /// <summary>
    /// Clears the Veterinarian that's associated with this Appointment.
    /// </summary>
    public void RemoveVeterinarian()
    {
        if (_veterinarian == null) throw new NotFoundException("This appointment is not associated with any Veterinarian.");

        var veterinarian = _veterinarian;
        _veterinarian = null;
        
        // Synchronize the removal on the Veterinarian side
        if (veterinarian.GetAppointments().Contains(this)) veterinarian.RemoveAppointment(this);
    }
    
    public Appointment() { }

    public Appointment(DateTime dateTime, AppointmentState state, int price)
    {
        DateTime = dateTime;
        State = state;
        Price = price;
        AddToExtent(this);
        _extent.Add(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, DateTime={DateTime:yyyy-MM-ddTHH:mm:ss}, State={State.ToString()} Price={Price}";
    }


    public void RemoveAppointment()
    {
        if (!_extent.Contains(this))
            throw new NotFoundException("Appointment not found in the list.");

        if (_pet != null)
        {
            var pet = _pet;
            _pet = null;

            if (pet.GetAppointments().Contains(this))
                pet.RemoveAppointment(Id);
        }

        var paymentsCopy = new List<Payment>(_payments); 
        foreach (var payment in paymentsCopy)
        {
            payment.RemovePayment();
        }

        _extent.Remove(this);
    }

}

using VetClinic.Exceptions;

namespace VetClinic;

public enum PaymentType { Cash, Card }

public class Payment: StoredObject<Payment>, IIdentifiable
{ 
    public int Id { get; set; }

    private double _amount;

    public double Amount
    {
        get => _amount;
        set
        {
            if (value <= 0)
                throw new NegativeValueException("Amount must be greater than zero.");
            _amount = value;
        }
    }

    private PaymentType _paymentType;
    public PaymentType PaymentType
    {
        get => _paymentType;
        set
        {
            if (!Enum.IsDefined(typeof(PaymentType), value))
                throw new ArgumentException("Invalid type of payment.");
            _paymentType = value;
        }
    }
    
    public DateTime DateTime { get; set; }


    private Appointment _appointment;

    public Appointment GetAppointment()
    {
        return _appointment;
    }

    public void AddAppointment(Appointment appointment)
    {
        if (appointment == null) throw new NullReferenceException("Appointment cannot be null.");
        if (_appointment != null && _appointment != appointment) throw new InvalidOperationException("This payment is already assigned to another appointment.");
        _appointment = appointment;
        if (!appointment.GetPayments().Contains(this)) appointment.AddPayment(this);
    }
    
    public Payment() {}

    public Payment(int amount, PaymentType paymentPaymentType, DateTime dateTime, Appointment appointment)
    {
        Amount = amount;
        PaymentType = paymentPaymentType;
        DateTime = dateTime;
        _appointment = appointment;
        AddAppointment(appointment);
        AddToExtent(this);
        _extent.Add(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, Amount={Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}, PaymentType={PaymentType.ToString()}, DateTime={DateTime:yyyy-MM-ddTHH:mm:ss}";
    }
    public void RemovePayment()
    {
        if (!_extent.Contains(this)) throw new NotFoundException("Payment not found in the list.");
        if (_appointment.GetPayments().Contains(this)) _appointment.RemovePayment(this); 
        _extent.Remove(this);
    }

}
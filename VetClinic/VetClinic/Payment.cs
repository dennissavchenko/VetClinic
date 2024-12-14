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

    public Payment() {}

    public Payment(int amount, PaymentType paymentPaymentType, DateTime dateTime, Appointment appointment)
    {
        Amount = amount;
        PaymentType = paymentPaymentType;
        DateTime = dateTime;
        _appointment = appointment;
        if (!_appointment.GetPayments().Contains(this))
            _appointment.AddPayment(this); 
        AddToExtent(this);
        _extent.Add(this);
    }
    
    public override string ToString()
    {
        return $"Id={Id}, Amount={Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}, PaymentType={PaymentType.ToString()}, DateTime={DateTime:yyyy-MM-ddTHH:mm:ss}";
    }
    public void RemovePayment()
    {
        if (!_extent.Contains(this))
            throw new NotFoundException("Payment not found in the list.");

        _appointment.RemovePayment(this); 

       
        _extent.Remove(this);
    }

}
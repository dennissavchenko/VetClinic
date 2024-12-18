using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class PaymentTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Payment.json";
        File.Delete(_testPath);
    }
    
    [TearDown]
    
    public void Teardown()
    {
        if (File.Exists(_testPath))
        {
            File.Delete(_testPath);
        }
    }
    
    [Test]
    public void AddToExtent_ShouldAddPaymentCorrectly()
    {
        // Arrange
        var appointment = new Appointment(new DateTime(2018, 5, 1), AppointmentState.Scheduled, 200);
        var payment1 = new Payment(150, PaymentType.Cash, new DateTime(2018, 5, 1, 16, 32, 34), appointment);
        var payment2 = new Payment(200, PaymentType.Card, new DateTime(2018, 5, 2, 16, 2, 51), appointment);

        // Act
        var extent = Payment.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("PaymentType=Cash"));
        Assert.That(extent[0].Contains("DateTime=2018-05-01T16:32:34"));
        Assert.That(extent[0].Contains("Amount=150"));
        Assert.That(extent[1].Contains("Id=2"));
        Assert.That(extent[1].Contains("PaymentType=Card"));
        Assert.That(extent[1].Contains("DateTime=2018-05-02T16:02:51"));
        Assert.That(extent[1].Contains("Amount=200"));
    }
    
    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"Amount\": 150, \"Type\": 0, \"DateTime\": \"2018-05-01T16:32:34\" }]");

        // Act
        var extent = Payment.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("PaymentType=Cash"));
        Assert.That(extent[0].Contains("DateTime=2018-05-01T16:32:34"));
        Assert.That(extent[0].Contains("Amount=150"));
    }

    [Test]
    public void Amount_ShouldThrowArgumentOutOfRangeException_ForZeroOrNegativeAmount()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var appointment = new Appointment(new DateTime(2018, 5, 1), AppointmentState.Scheduled, 200);
            var payment = new Payment(-100, PaymentType.Card, new DateTime(2018, 5, 1), appointment);
        });
    }

    [Test]
    public void Type_ShouldThrowArgumentException_ForInvalidType()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var appointment = new Appointment(new DateTime(2018, 5, 1), AppointmentState.Scheduled, 200);
            // Invalid cast to force an invalid Type enum value
            var payment = new Payment(200, (PaymentType)999, new DateTime(2018, 5, 1), appointment);
        });
    }

    [Test]
    public void Payment_ShouldAssociateWithAppointmentCorrectly()
    {
        // Arrange
        var appointment = new Appointment(new DateTime(2024, 12, 17), AppointmentState.Scheduled, 300); 
        var payment = new Payment(100, PaymentType.Card,DateTime.Now, appointment);  

        // Assert
        Assert.That(payment.GetAppointment().Equals(appointment)); 
        Assert.That(appointment.GetPayments().Contains(payment));  
    }
    
    [Test]
    public void AddAppointment_ShouldThrowMethodMisuseException_TryingToModifyWithAdd()
    {
        // Arrange
        var appointment = new Appointment(new DateTime(2024, 12, 17), AppointmentState.Scheduled, 300); 
        var appointment1 = new Appointment(new DateTime(2024, 12, 17), AppointmentState.Scheduled, 39);
        var payment = new Payment(100, PaymentType.Card,DateTime.Now, appointment); 

        // Assert
        Assert.Throws<MethodMisuseException>(() => payment.AddAppointment(appointment1));
    }
    
    [Test]
    public void AddAppointment_ShouldThrowNullReferenceException()
    {
        // Arrange
        var appointment = new Appointment(new DateTime(2024, 12, 17), AppointmentState.Scheduled, 300); 
        var payment = new Payment(100, PaymentType.Card,DateTime.Now, appointment); 

        // Assert
        Assert.Throws<NullReferenceException>(() => payment.AddAppointment(null!));
    }
    
    [Test]
    public void RemovePayment_ShouldRemovePaymentCorrectly_AndItsAssociations()
    {
        // Arrange
        var appointment = new Appointment(new DateTime(2024, 12, 17), AppointmentState.Scheduled, 300); 
        var payment = new Payment(100, PaymentType.Card,DateTime.Now, appointment); 

        // Act
        payment.RemovePayment();
        
        // Assert
        Assert.That(!appointment.GetPayments().Contains(payment));
        Assert.That(!Payment.GetCurrentExtent().Contains(payment));
    }
    
    [Test]
    public void AddAppointment_ShouldThrowNotFoundException_TryingToDeleteAlreadyDeletedObject()
    {
        // Arrange
        var appointment = new Appointment(new DateTime(2024, 12, 17), AppointmentState.Scheduled, 300); 
        var payment = new Payment(100, PaymentType.Card,DateTime.Now, appointment);
        payment.RemovePayment();

        // Assert
        Assert.Throws<NotFoundException>(() => payment.RemovePayment());
    }

}
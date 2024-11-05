using VetClinic;

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
        var paymentDate = new DateTime(2018, 5, 1);
        var payment = new Payment(200, PaymentType.Cash, paymentDate);

        // Act
        var extent = Payment.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Amount, Is.EqualTo(200));
        Assert.That(extent[0].PaymentType, Is.EqualTo(PaymentType.Cash));
        Assert.That(extent[0].DateTime, Is.EqualTo(paymentDate));
    }
    
    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"Amount\": 150, \"Type\": \"Cash\", \"DateTime\": \"2018-05-01T00:00:00\" }]");

        // Act
        var extent = Payment.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Amount, Is.EqualTo(150));
        Assert.That(extent[0].PaymentType, Is.EqualTo(PaymentType.Cash));
        Assert.That(extent[0].DateTime, Is.EqualTo(new DateTime(2018, 5, 1)));
    }

    [Test]
    public void Amount_ShouldThrowArgumentOutOfRangeException_ForZeroOrNegativeAmount()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            // Arrange
            var payment = new Payment(-100, PaymentType.Card, new DateTime(2018, 5, 1));
        });
    }

    [Test]
    public void Type_ShouldThrowArgumentException_ForInvalidType()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Invalid cast to force an invalid Type enum value
            var payment = new Payment(200, (PaymentType)999, new DateTime(2018, 5, 1));
        });
    }
}
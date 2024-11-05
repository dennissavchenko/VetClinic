using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class ClientTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Client.json";
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
    public void AddToExtent_ShouldAddClientCorrectly()
    {
        // Arrange
        var client = new Client("Maciej", "Dominiak", "334499950", "Maciej@gmail.com");

        // Act
        var extent = Client.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].FirstName, Is.EqualTo("Maciej"));
        Assert.That(extent[0].LastName, Is.EqualTo("Dominiak"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("334499950"));
        Assert.That(extent[0].Email, Is.EqualTo("Maciej@gmail.com"));

    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var client1 = new Client("Darma", "Bartoszewska", "234284882", "darma@gmail.com");
        var client2 = new Client("Joel", "Smith", "278811800", "joel@gmail.com");

        // Act
        var extent = Client.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var client = new Client("Marta", "Ostrowska", "773712000", "Marta@gmail.com");

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"FirstName\": \"Marta\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Ostrowska\""));
        Assert.IsTrue(json.Contains("\"PhoneNumber\": \"773712000\""));
        Assert.IsTrue(json.Contains("\"Email\": \"Marta@gmail.com\""));

    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"FirstName\": \"Marek\", \"LastName\": \"Kowalski\", \"PhoneNumber\": \"081821727\", \"Email\": \"marek@gmail.com\" }]");

        // Act
        var extent = Client.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].FirstName, Is.EqualTo("Marek"));
        Assert.That(extent[0].LastName, Is.EqualTo("Kowalski"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("081821727"));
        Assert.That(extent[0].Email, Is.EqualTo("marek@gmail.com"));


    }

    [Test]
    public void FirstName_ShouldThrowAnEmptyStringException_ForEmptyFirstNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var client = new Client("", "Kowalski", "828222222", "a@gmail.com");

        });
    }

    [Test]
    public void LastName_ShouldThrowAnEmptyStringException_ForEmptyLastNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var client = new Client("Anna", "", "828222222", "a@gmail.com");

        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnEmptyStringException_ForEmptyPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var client = new Client("Anna", "Kowalski", "", "a@gmail.com");

        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnInvalidFormatException_ForInvalidPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var client = new Client("Anna", "Kowalski", "828222", "annakowal@gmail.com");

        });
    }


    [Test]
    public void Email_ShouldThrowAnEmptyStringException_ForEmptyEmailString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var client = new Client("Anna", "Kowalski", "828222222", "");

        });
    }

    [Test]
    public void Email_ShouldThrowAnInvalidDataException_ForInvalidEmailString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var client = new Client("Anna", "Kowalski", "828222222", "annakowalgmail.com");

        });
    }
}
    


  







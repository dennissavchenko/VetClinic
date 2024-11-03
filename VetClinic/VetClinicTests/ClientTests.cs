using VetClinic;

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
        var client = new Client("Maciej", "Dominiak", "+33449995", "Maciej@gmail.com");

        // Act
        var extent = Client.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].FirstName, Is.EqualTo("Maciej"));
        Assert.That(extent[0].LastName, Is.EqualTo("Dominiak"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("+33449995"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("Maciej@gmail.com"));

    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var client1 = new Client("Darma", "Bartoszewska", "+2342848824", "darma@gmail.com");
        var client2 = new Client("Joel", "Smith", "+2788118", "joel@gmail.com");

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
        var client = new Client("Marta", "Ostrowska", "+773712", "Marta@gmail.com");

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"FirstName\": \"Marta\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Ostrowska\""));
        Assert.IsTrue(json.Contains("\"PhoneNumber\":\"+773712\""));
        Assert.IsTrue(json.Contains("\"Email\":\"Marta@gmail.com\""));

    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        var client = new Client("Sara", "Smith", "+82288181", "sara@gmail.com");
        typeof(Client).GetField("_extent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, new List<Client>());

        // Act
        var extent = Client.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].FirstName, Is.EqualTo("Sara"));
        Assert.That(extent[0].LastName, Is.EqualTo("Smith"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("+82288181"));
        Assert.That(extent[0].Email, Is.EqualTo("sara@gmail.com"));


    }
}



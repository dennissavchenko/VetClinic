using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class MammalTests
{
    private string _testPathPet, _testPathMammal;

    [SetUp]
    public void Setup()
    {
        _testPathMammal = "../../../Data/Mammal.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathMammal);
        File.Delete(_testPathPet);
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathMammal))
        {
            File.Delete(_testPathMammal);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    [Test]
    public void AddToExtent_ShouldAddMammalCorrectly()
    {
        // Arrange
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 6, 15), new List<Color> { Color.Brown, Color.White }, true);

        // Act
        var extent = Mammal.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(12.5));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2019, 6, 15)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.Brown, Color.White }));
        Assert.That(extent[0].Nocturnal, Is.EqualTo(true));
    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var mammal1 = new Mammal("Luna", Sex.Female, 15.0, new DateTime(2020, 1, 10), new List<Color> { Color.Gray }, true);
        var mammal2 = new Mammal("Max", Sex.Male, 20.5, new DateTime(2018, 3, 5), new List<Color> { Color.Black }, false);

        // Act
        var extent = Mammal.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 6, 15), new List<Color> { Color.Brown, Color.White }, true);

        // Act
        var json = File.ReadAllText(_testPathMammal);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Bella\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1"));
        Assert.IsTrue(json.Contains("\"Weight\": 12.5"));
        Assert.IsTrue(json.Contains("\"Nocturnal\": true"));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathMammal, "[{\"Nocturnal\":true,\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":12.5,\"DateOfBirth\":\"2019-06-15T00:00:00\",\"Colors\":[2,1],\"Age\":4}]");

        // Act
        var extent = Mammal.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(12.5));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2019, 6, 15)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.Brown, Color.White }));
        Assert.That(extent[0].Nocturnal, Is.EqualTo(true));
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 1, 1), new List<Color> { Color.Brown }, true);

        // Act
        int age = mammal.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2019));
    }

    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var mammal = new Mammal("", Sex.Female, 12.5, new DateTime(2019, 1, 1), new List<Color> { Color.Brown }, true);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, -12.5, new DateTime(2019, 1, 1), new List<Color> { Color.Brown }, true);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(1), new List<Color> { Color.Brown }, true);
        });
    }
}

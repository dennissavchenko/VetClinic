using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class PregnantTests
{
    private string _testPathPregnant, _testPathPet;

    [SetUp]
    public void Setup()
    {
        _testPathPregnant = "../../../Data/Pregnant.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathPregnant);
        File.Delete(_testPathPet);
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathPregnant))
        {
            File.Delete(_testPathPregnant);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    // Tests for adding pregnant pets
    [Test]
    public void AddToExtent_ShouldAddPregnantPetCorrectly()
    {
        // Arrange
        var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Today.AddMonths(1), 3);

        // Act
        var extent = Pregnant.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].DueDate, Is.EqualTo(DateTime.Today.AddMonths(1)));
        Assert.That(extent[0].LitterSize, Is.EqualTo(3));
    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var pregnantPet1 = new Pregnant("Luna", Sex.Female, 6.0, new DateTime(2019, 3, 15), new List<Color> { Color.Brown }, DateTime.Now.AddMonths(1), 2);
        var pregnantPet2 = new Pregnant("Max", Sex.Male, 9.5, new DateTime(2017, 8, 22), new List<Color> { Color.Gray }, DateTime.Now.AddMonths(2), 4);

        // Act
        var extent = Pregnant.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Today.AddMonths(1), 3);

        // Act
        var json = File.ReadAllText(_testPathPregnant);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Bella\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1"));
        Assert.IsTrue(json.Contains("\"Weight\": 8"));
        Assert.IsTrue(json.Contains($"\"DueDate\": \"{DateTime.Today.AddMonths(1):yyyy-MM-ddTHH:mm:ss}"));
        Assert.IsTrue(json.Contains("\"LitterSize\": 3"));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathPregnant, "[{\"DueDate\":\"2024-12-01T00:00:00\",\"LitterSize\":3,\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":8.0,\"DateOfBirth\":\"2018-05-01T00:00:00\",\"Colors\":[1],\"Age\":6}]");

        // Act
        var extent = Pregnant.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].DueDate, Is.EqualTo(new DateTime(2024, 12, 1)));
        Assert.That(extent[0].LitterSize, Is.EqualTo(3));
    }

    // Tests for invalid conditions
    [Test]
    public void DueDate_ShouldThrowInvalidDateException_ForPastDueDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Now.AddDays(-1), 3);
        });
    }

    [Test]
    public void DueDate_ShouldThrowInvalidDateException_ForDueDateBeforeDateOfBirth()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, new DateTime(2017, 5, 10), 3);
        });
    }

    [Test]
    public void LitterSize_ShouldThrowNegativeValueException_ForZeroOrNegativeLitterSize()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Now.AddMonths(1), 0);
        });

        Assert.Throws<NegativeValueException>(() =>
        {
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Now.AddMonths(1), -3);
        });
    }

    // Include Pet tests here (Assuming the Pet class has similar validations)
    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Now.AddMonths(1), 3);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, -8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, DateTime.Now.AddMonths(1), 3);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, DateTime.Now.AddDays(1), new List<Color> { Color.White }, DateTime.Now.AddMonths(1), 3);
        });
    }
}

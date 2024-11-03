using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class HealthyTests
{
    private string _testPathPet, _testPathHealthy;

    [SetUp]
    public void Setup()
    {
        _testPathHealthy = "../../../Data/Healthy.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathHealthy);
        File.Delete(_testPathPet);
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathHealthy))
        {
            File.Delete(_testPathHealthy);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    [Test]
    public void AddToExtent_ShouldAddHealthyPetCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, new DateTime(2020, 5, 10));

        // Act
        var extent = Healthy.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].ActivityLevel, Is.EqualTo(ActivityLevel.Medium));
        Assert.That(extent[0].LastVaccinationDate, Is.EqualTo(new DateTime(2020, 5, 10)));
    }
    
    [Test]
    public void AddToExtent_ShouldAddHealthyPetCorrectly_LastVaccinationDate_IsNull()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, null);

        // Act
        var extent = Healthy.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].ActivityLevel, Is.EqualTo(ActivityLevel.Medium));
        Assert.That(extent[0].LastVaccinationDate, Is.EqualTo(null));
    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var healthyPet1 = new Healthy("Luna", Sex.Female, 6.0, new DateTime(2019, 3, 15), new List<Color> { Color.Brown }, ActivityLevel.High, new DateTime(2021, 7, 10));
        var healthyPet2 = new Healthy("Max", Sex.Male, 9.5, new DateTime(2017, 8, 22), new List<Color> { Color.Gray }, ActivityLevel.Low, new DateTime(2020, 12, 5));

        // Act
        var extent = Healthy.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, new DateTime(2020, 5, 10));

        // Act
        var json = File.ReadAllText(_testPathHealthy);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Bella\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1"));
        Assert.IsTrue(json.Contains("\"Weight\": 8"));
        Assert.IsTrue(json.Contains("\"ActivityLevel\": 1"));
        Assert.IsTrue(json.Contains("\"LastVaccinationDate\": \"2020-05-10T00:00:00\""));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathHealthy, "[{\"ActivityLevel\":1,\"LastVaccinationDate\":\"2020-05-10T00:00:00\",\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":8.0,\"DateOfBirth\":\"2018-05-01T00:00:00\",\"Colors\":[1],\"Age\":5}]");

        // Act
        var extent = Healthy.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].ActivityLevel, Is.EqualTo(ActivityLevel.Medium));
        Assert.That(extent[0].LastVaccinationDate, Is.EqualTo(new DateTime(2020, 5, 10)));
    }
    
    [Test]
    public void Name_ShouldThrowEmptyStringException_ForEmptyName()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, new DateTime(2020, 5, 10));
        });
    }

    [Test]
    public void Weight_ShouldThrowNegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, -1.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, new DateTime(2020, 5, 10));
        });
    }
    
    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var healthyPet = new Healthy("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.Yellow], ActivityLevel.High, new DateTime(2020, 1, 1));
        });
    }

    [Test]
    public void LastVaccinationDate_ShouldThrowInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, DateTime.Now.AddDays(1));
        });
    }

    [Test]
    public void LastVaccinationDate_ShouldThrowInvalidDateException_IfBeforeDateOfBirth()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, new DateTime(2017, 1, 1));
        });
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, ActivityLevel.Medium, new DateTime(2020, 5, 10));

        // Act
        int age = healthyPet.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2018));
    }
}

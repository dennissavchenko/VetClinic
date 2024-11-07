using VetClinic;
using VetClinic.Exceptions;
using DateTime = System.DateTime;

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
        var healthyPet1 = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1),  [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));
        var healthyPet2 = new Healthy("Tom", Sex.Male, 12.6, new DateTime(2017, 1, 11), [Color.White], ActivityLevel.High, null);

        // Act
        var extent = Healthy.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=5/1/2018"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("ActivityLevel=Medium"));
        Assert.IsTrue(extent[0].Contains("LastVaccinationDate=5/10/2020"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Tom"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=12.6"));
        Assert.IsTrue(extent[1].Contains("ActivityLevel=High"));
        Assert.IsTrue(extent[1].Contains("LastVaccinationDate=NotVaccinated"));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));

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
        var extent = Healthy.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=5/1/2018"));
        Assert.IsTrue(extent[0].Contains("ActivityLevel=Medium"));
        Assert.IsTrue(extent[0].Contains("LastVaccinationDate=5/10/2020"));
        
    }
    
    [Test]
    public void Name_ShouldThrowEmptyStringException_ForEmptyName()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));
        });
    }

    [Test]
    public void Weight_ShouldThrowNegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, -1.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));
        });
    }
    
    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var healthyPet = new Healthy("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.White], ActivityLevel.High, new DateTime(2020, 1, 1));
        });
    }

    [Test]
    public void LastVaccinationDate_ShouldThrowInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, DateTime.Now.AddDays(1));
        });
    }

    [Test]
    public void LastVaccinationDate_ShouldThrowInvalidDateException_IfBeforeDateOfBirth()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2017, 1, 1));
        });
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));

        // Act
        int age = healthyPet.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2018));
    }
}

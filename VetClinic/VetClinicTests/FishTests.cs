using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class FishTests
{
    private string _testPathPet, _testPathFish;

    [SetUp]
    public void Setup()
    {
        _testPathFish = "../../../Data/Fish.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathFish);
        File.Delete(_testPathPet);
    }
        
    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathFish))
        {
            File.Delete(_testPathFish);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    [Test]
    public void AddToExtent_ShouldAddFishCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 3, 15), new List<Color> { Color.Golden }, WaterType.Freshwater, 22.5);

        // Act
        var extent = Fish.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Goldie"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(0.2));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2021, 3, 15)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.Golden }));
        Assert.That(extent[0].WaterType, Is.EqualTo(WaterType.Freshwater));
        Assert.That(extent[0].WaterTemperature, Is.EqualTo(22.5));
    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var fish1 = new Fish("Nemo", Sex.Male, 0.1, new DateTime(2022, 2, 5), new List<Color> { Color.Red }, WaterType.Saltwater, 25.0);
        var fish2 = new Fish("Dory", Sex.Female, 0.15, new DateTime(2021, 1, 20), new List<Color> { Color.Blue }, WaterType.Freshwater, 23.0);

        // Act
        var extent = Fish.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 3, 15), new List<Color> { Color.Golden }, WaterType.Freshwater, 22.5);

        // Act
        var json = File.ReadAllText(_testPathFish);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Goldie\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1"));
        Assert.IsTrue(json.Contains("\"Weight\": 0.2"));
        Assert.IsTrue(json.Contains("\"WaterType\": 0"));
        Assert.IsTrue(json.Contains("\"WaterTemperature\": 22.5"));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathFish, "[{\"WaterType\":0,\"WaterTemperature\":22.5,\"Id\":1,\"Name\":\"Goldie\",\"Sex\":1,\"Weight\":0.2,\"DateOfBirth\":\"2021-03-15T00:00:00\",\"Colors\":[5],\"Age\":2}]");

        // Act
        var extent = Fish.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Goldie"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(0.2));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2021, 3, 15)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.Red }));
        Assert.That(extent[0].WaterType, Is.EqualTo(WaterType.Freshwater));
        Assert.That(extent[0].WaterTemperature, Is.EqualTo(22.5));
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), new List<Color> { Color.Golden }, WaterType.Freshwater, 22.5);

        // Act
        int age = fish.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2021));
    }

    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var fish = new Fish("", Sex.Female, 0.2, new DateTime(2021, 1, 1), new List<Color> { Color.Golden }, WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, -0.2, new DateTime(2021, 1, 1), new List<Color> { Color.Golden }, WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, DateTime.Now.AddDays(1), new List<Color> { Color.Golden }, WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void WaterTemperature_ShouldThrowANegativeValueException_ForNegativeWaterTemperature()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), new List<Color> { Color.Golden }, WaterType.Freshwater, -5.0);
        });
    }
}

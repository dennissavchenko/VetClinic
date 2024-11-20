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
        var fish1 = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 3, 15), [Color.Golden], WaterType.Freshwater, 22.5);
        var fish2 = new Fish("Momo", Sex.Male, 0.45, new DateTime(2018, 6, 12), [Color.Black, Color.White], WaterType.Saltwater, 15.0);

        // Act
        var extent = Fish.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Goldie"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=0.2"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=3/15/2021"));
        Assert.IsTrue(extent[0].Contains("Colors=(Golden)"));
        Assert.IsTrue(extent[0].Contains("WaterType=Freshwater"));
        Assert.IsTrue(extent[0].Contains("WaterTemperature=22.5"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=0.45"));
        Assert.IsTrue(extent[1].Contains("WaterType=Saltwater"));
        Assert.IsTrue(extent[1].Contains("WaterTemperature=15"));
        
    }
    
    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 3, 15), [Color.Golden], WaterType.Freshwater, 22.5);

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
        var extent = Fish.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Goldie"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=0.2"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=3/15/2021"));
        Assert.IsTrue(extent[0].Contains("Colors=(Red)"));
        Assert.IsTrue(extent[0].Contains("WaterType=Freshwater"));
        Assert.IsTrue(extent[0].Contains("WaterTemperature=22.5"));
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, 22.5);

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
            var fish = new Fish("", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, -0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, DateTime.Now.AddDays(1), [Color.Golden], WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void WaterTemperature_ShouldThrowANegativeValueException_ForNegativeWaterTemperature()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, -5.0);
        });
    }
    
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [], WaterType.Freshwater, -5.0);
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden, Color.Golden], WaterType.Freshwater, -5.0);
        });
    }
}

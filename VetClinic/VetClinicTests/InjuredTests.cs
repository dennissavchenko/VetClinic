using VetClinic;
using VetClinic.Exceptions;
using DateTime = System.DateTime;

namespace VetClinicTests;

public class InjuredTests
{
    private string _testPathInjured, _testPathPet;

    [SetUp]
    public void Setup()
    {
        _testPathInjured = "../../../Data/Injured.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathInjured);
        File.Delete(_testPathPet);
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathInjured))
        {
            File.Delete(_testPathInjured);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    [Test]
    public void AddToExtent_ShouldAddInjuredPetCorrectly()
    {
        // Arrange
        var injuredPet1 = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Wound, new DateTime(2021, 5, 10));
        var injuredPet2 = new Injured("Momo", Sex.Male, 4.6, new DateTime(2017, 5, 1), [Color.White, Color.Black], InjuryType.Sprain, new DateTime(2020, 5, 10));

        // Act
        var extent = Injured.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=5/1/2018"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("InjuryType=Wound"));
        Assert.IsTrue(extent[0].Contains("InjuryDate=5/10/2021"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=4.6"));
        Assert.IsTrue(extent[1].Contains("InjuryType=Sprain"));
        Assert.IsTrue(extent[1].Contains("InjuryDate=5/10/2020"));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Wound, new DateTime(2021, 5, 10));
        
        // Act
        var json = File.ReadAllText(_testPathInjured);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Bella\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1")); 
        Assert.IsTrue(json.Contains("\"Weight\": 8"));
        Assert.IsTrue(json.Contains("\"InjuryType\": 1"));
        Assert.IsTrue(json.Contains("\"InjuryDate\": \"2021-05-10T00:00:00\""));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathInjured, "[{\"InjuryType\":0,\"InjuryDate\":\"2021-05-10T00:00:00\",\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":8.0,\"DateOfBirth\":\"2018-05-01T00:00:00\",\"Colors\":[1],\"Age\":3}]");

        // Act
        var extent = Injured.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=5/1/2018"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("InjuryType=Fracture"));
        Assert.IsTrue(extent[0].Contains("InjuryDate=5/10/2021"));
    }

    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var injuredPet = new Injured("", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Wound, new DateTime(2021, 5, 10));
        });
    }
    
    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, -8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Wound, new DateTime(2021, 5, 10));
        });
    }
    
    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, DateTime.Now.AddDays(1), [Color.White], InjuryType.Wound, new DateTime(2021, 5, 10));
        });
    }
    
    [Test]
    public void InjuryDate_ShouldThrowAnInvalidDateException_ForFutureInjuryDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Wound, DateTime.Now.AddDays(1));
        });
    }
    
    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Fracture, DateTime.Now.AddDays(-1));

        // Act
        int age = injuredPet.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2018));
    }
    
    [Test]
    public void InjuryDate_ShouldThrowAnInvalidDateException_ForInjuryDateBeforeDateOfBirth()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], InjuryType.Wound, new DateTime(2017, 5, 10));
        });
    }
    
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [], InjuryType.Wound, new DateTime(2020, 5, 10));
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White, Color.White], InjuryType.Wound, new DateTime(2020, 5, 10));
        });
    }
    
}

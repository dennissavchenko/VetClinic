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
        var pregnantPet1 = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], new DateTime(2020, 5, 1), 3);
        var pregnantPet2 = new Pregnant("Momo", Sex.Male, 4.6, new DateTime(2017, 5, 1), [Color.White, Color.Black], new DateTime(2021, 5, 1), 10);

        // Act
        var extent = Pregnant.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("DueDate=2020-05-01"));
        Assert.IsTrue(extent[0].Contains("LitterSize=3"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=4.6"));
        Assert.IsTrue(extent[1].Contains("DueDate=2021-05-01"));
        Assert.IsTrue(extent[1].Contains("LitterSize=10"));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], DateTime.Today.AddMonths(1), 3);

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
        File.WriteAllText(_testPathPregnant, "[{\"DueDate\":\"2020-05-01T00:00:00\",\"LitterSize\":3,\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":8.0,\"DateOfBirth\":\"2018-05-01T00:00:00\",\"Colors\":[1],\"Age\":6}]");

        // Act
        var extent = Pregnant.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("DueDate=2020-05-01"));
        Assert.IsTrue(extent[0].Contains("LitterSize=3"));
    }

    [Test]
    public void DueDate_ShouldThrowInvalidDateException_ForDueDateBeforeDateOfBirth()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White],  new DateTime(2017, 5, 10), 3);
        });
    }

    [Test]
    public void LitterSize_ShouldThrowNegativeValueException_ForZeroOrNegativeLitterSize()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White],  DateTime.Now.AddMonths(1), 0);
        });

        Assert.Throws<NegativeValueException>(() =>
        {
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White],  DateTime.Now.AddMonths(1), -3);
        });
    }
    
    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White],  DateTime.Now.AddMonths(1), 3);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, -8.0, new DateTime(2018, 5, 1), [Color.White],  DateTime.Now.AddMonths(1), 3);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, DateTime.Now.AddDays(1), [Color.White], DateTime.Now.AddMonths(1), 3);
        });
    }
    
        
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [],  DateTime.Now.AddMonths(1), 3);
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var pregnantPet = new Pregnant("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White, Color.White],  DateTime.Now.AddMonths(1), 3);
        });
    }

}

using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class MammalTests
{
    private string _testPathPet, _testPathMammal;
    private Specie _dog;

    [SetUp]
    public void Setup()
    {
        _testPathMammal = "../../../Data/Mammal.json";
        _testPathPet = "../../../Data/Pet.json";
        _dog = new Specie("Dog", "Canis lupus familiaris");
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
        var mammal1 = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 6, 15), [Color.Brown, Color.White], _dog, true);
        var mammal2 = new Mammal("Momo", Sex.Male, 4.6, new DateTime(2017, 5, 1), [Color.White, Color.Black], _dog, false);

        // Act
        var extent = Mammal.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=12.5"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2019-06-15"));
        Assert.IsTrue(extent[0].Contains("Colors=(Brown, White)"));
        Assert.IsTrue(extent[0].Contains("Nocturnal=True"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=4.6"));
        Assert.IsTrue(extent[1].Contains("Nocturnal=False"));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 6, 15), [Color.Brown, Color.White], _dog,true);

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
        var extent = Mammal.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=12.5"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2019-06-15"));
        Assert.IsTrue(extent[0].Contains("Colors=(Brown, White)"));
        Assert.IsTrue(extent[0].Contains("Nocturnal=True"));
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 1, 1), [Color.Brown], _dog, true);

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
            var mammal = new Mammal("", Sex.Female, 12.5, new DateTime(2019, 1, 1), [Color.Brown], _dog, true);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, -12.5, new DateTime(2019, 1, 1), [Color.Brown], _dog, true);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(1), [Color.Brown], _dog, true);
        });
    }
    
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(-10), [], _dog, true);
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(-10), [Color.Brown, Color.Brown], _dog, true);
        });
    }
    
}

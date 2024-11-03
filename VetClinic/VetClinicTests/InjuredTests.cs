using VetClinic;
using VetClinic.Exceptions;

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
        var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Wound, new DateTime(2021, 5, 10));

        // Act
        var extent = Injured.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].InjuryType, Is.EqualTo(InjuryType.Wound));
        Assert.That(extent[0].InjuryDate, Is.EqualTo(new DateTime(2021, 5, 10)));
    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var injuredPet1 = new Injured("Luna", Sex.Female, 6.0, new DateTime(2019, 3, 15), new List<Color> { Color.Brown }, InjuryType.Fracture, new DateTime(2021, 7, 10));
        var injuredPet2 = new Injured("Max", Sex.Male, 9.5, new DateTime(2017, 8, 22), new List<Color> { Color.Gray }, InjuryType.Sprain, new DateTime(2020, 12, 5));

        // Act
        var extent = Injured.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Wound, new DateTime(2021, 5, 10));
        
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
        var extent = Injured.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].Name, Is.EqualTo("Bella"));
        Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
        Assert.That(extent[0].Weight, Is.EqualTo(8.0));
        Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.White }));
        Assert.That(extent[0].InjuryType, Is.EqualTo(InjuryType.Fracture));
        Assert.That(extent[0].InjuryDate, Is.EqualTo(new DateTime(2021, 5, 10)));
    }

    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var injuredPet = new Injured("", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Wound, new DateTime(2021, 5, 10));
        });
    }
    
    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, -8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Wound, new DateTime(2021, 5, 10));
        });
    }
    
    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, DateTime.Now.AddDays(1), new List<Color> { Color.White }, InjuryType.Wound, new DateTime(2021, 5, 10));
        });
    }
    
    [Test]
    public void InjuryDate_ShouldThrowAnInvalidDateException_ForFutureInjuryDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Wound, DateTime.Now.AddDays(1));
        });
    }
    
    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Fracture, DateTime.Now.AddDays(-1));

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
            var injuredPet = new Injured("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), new List<Color> { Color.White }, InjuryType.Wound, new DateTime(2017, 5, 10));
        });
    }
}

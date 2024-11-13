using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class VeterinarianTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Veterinarian.json";
        File.Delete(_testPath);
    }
    
    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPath))
        {
            File.Delete(_testPath);
        }
    }
    
    [Test]
    public void AddToExtent_ShouldAddVeterinarianCorrectly()
    {
        // Arrange
        var veterinarian1 = new Veterinarian("Piotr", "Nowak", "545333211", "pnowak@gmail.com", Specialization.Dentistry, ExperienceLevel.Intermediate);
        var veterinarian2 = new Veterinarian("Marek", "Kowalski", "432567912", "kowalski@gmail.com", Specialization.Ophthalmology, ExperienceLevel.Junior);

        // Act
        var extent = Veterinarian.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("FirstName=Piotr"));
        Assert.That(extent[0].Contains("LastName=Nowak"));
        Assert.That(extent[0].Contains("PhoneNumber=545333211"));
        Assert.That(extent[0].Contains("Email=pnowak@gmail.com"));
        Assert.That(extent[0].Contains("Specialization=Dentistry"));
        Assert.That(extent[0].Contains("ExperienceLevel=Intermediate"));

        Assert.That(extent[1].Contains("Id=2"));
        Assert.That(extent[1].Contains("FirstName=Marek"));
        Assert.That(extent[1].Contains("LastName=Kowalski"));
        Assert.That(extent[1].Contains("PhoneNumber=432567912"));
        Assert.That(extent[1].Contains("Email=kowalski@gmail.com"));
        Assert.That(extent[1].Contains("Specialization=Ophthalmology"));
        Assert.That(extent[1].Contains("ExperienceLevel=Junior"));

    }
    
    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var veterinarian = new Veterinarian("Marta", "Nowicka", "112233445", "mnowicka@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"FirstName\": \"Marta\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Nowicka\""));
        Assert.IsTrue(json.Contains("\"PhoneNumber\": \"112233445\""));
        Assert.IsTrue(json.Contains("\"Email\": \"mnowicka@gmail.com\""));
        Assert.IsTrue(json.Contains("\"Specialization\": 1"));
        Assert.IsTrue(json.Contains("\"ExperienceLevel\": 2"));
    }
    
    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"FirstName\": \"Adam\", \"LastName\": \"Szulc\", \"PhoneNumber\": \"091234567\", \"Email\": \"adam.szulc@example.com\", \"Specialization\": 3, \"ExperienceLevel\": 4}]");

        // Act
        var extent = Veterinarian.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("FirstName=Adam"));
        Assert.That(extent[0].Contains("LastName=Szulc"));
        Assert.That(extent[0].Contains("PhoneNumber=091234567"));
        Assert.That(extent[0].Contains("Email=adam.szulc@example.com"));
        Assert.That(extent[0].Contains("Specialization=Ophthalmology"));
        Assert.That(extent[0].Contains("ExperienceLevel=Senior"));
    }

    [Test]
    public void FirstName_ShouldThrowAnEmptyStringException_ForEmptyFirstNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("", "Kowalska", "555666777", "aKowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }
    [Test]
    public void LastName_ShouldThrowAnEmptyStringException_ForEmptyLastNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "", "555666777", "aKowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnEmptyStringException_ForEmptyPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "", "aKowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnInvalidDataException_ForInvalidPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "55566", "anna@example.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }
    
    [Test]
    public void Email_ShouldThrowAnEmptyStringException_ForEmptyEmailString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }

    [Test]
    public void Email_ShouldThrowAnInvalidDataException_ForInvalidEmailString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "akowalgmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }
    
}
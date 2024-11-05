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
        var veterinarian = new Veterinarian("Piotr", "Nowak", "545333211", "pnowak@gmail.com", "Surgeon", "Advanced");

        // Act
        var extent = Veterinarian.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].FirstName, Is.EqualTo("Piotr"));
        Assert.That(extent[0].LastName, Is.EqualTo("Nowak"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("545333211"));
        Assert.That(extent[0].Email, Is.EqualTo("pnowak@gmail.com"));
        Assert.That(extent[0].Specialization, Is.EqualTo("Surgeon"));
        Assert.That(extent[0].ExperienceLevel, Is.EqualTo("Advanced"));
    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var veterinarian1 = new Veterinarian("Bart", "Riley", "987544467", "briley@gmail.com", "injection", "high");
        var veterinarian2 = new Veterinarian("Will", "Jonas", "673529321", "wjonas@gmail.com", "surgeon", "moderate");

        // Act
        var extent = Veterinarian.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }
    
    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var veterinarian = new Veterinarian("Marta", "Nowicka", "112233445", "mnowicka@gmail.com", "Radiology", "Intermediate");

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"FirstName\": \"Marta\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Nowicka\""));
        Assert.IsTrue(json.Contains("\"PhoneNumber\": \"112233445\""));
        Assert.IsTrue(json.Contains("\"Email\": \"mnowicka@gmail.com\""));
        Assert.IsTrue(json.Contains("\"Specialization\": \"Radiology\""));
        Assert.IsTrue(json.Contains("\"ExperienceLevel\": \"Intermediate\""));
    }
    
    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"FirstName\": \"Adam\", \"LastName\": \"Szulc\", \"PhoneNumber\": \"091234567\", \"Email\": \"adam.szulc@example.com\", \"Specialization\": \"Ophthalmology\", \"ExperienceLevel\": \"Senior\" }]");

        // Act
        var extent = Veterinarian.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].FirstName, Is.EqualTo("Adam"));
        Assert.That(extent[0].LastName, Is.EqualTo("Szulc"));
        Assert.That(extent[0].PhoneNumber, Is.EqualTo("091234567"));
        Assert.That(extent[0].Email, Is.EqualTo("aszulc@gmail.com"));
        Assert.That(extent[0].Specialization, Is.EqualTo("Ophthalmology"));
        Assert.That(extent[0].ExperienceLevel, Is.EqualTo("Senior"));
    }

    [Test]
    public void FirstName_ShouldThrowAnEmptyStringException_ForEmptyFirstNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("", "Kowalska", "555666777", "aKowalska@gmail.com", "Dentistry", "Intermediate");
        });
    }
    [Test]
    public void LastName_ShouldThrowAnEmptyStringException_ForEmptyLastNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "", "555666777", "aKowalska@gmail.com", "Dentistry", "Intermediate");
        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnEmptyStringException_ForEmptyPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "", "aKowalska@gmail.com", "Dentistry", "Intermediate");
        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnInvalidDataException_ForInvalidPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "55566", "anna@example.com", "Dentistry", "Intermediate");
        });
    }
    
    [Test]
    public void Email_ShouldThrowAnEmptyStringException_ForEmptyEmailString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "", "Dentistry", "Intermediate");
        });
    }

    [Test]
    public void Email_ShouldThrowAnInvalidDataException_ForInvalidEmailString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "akowal@gmail.com", "Dentistry", "Intermediate");
        });
    }

    [Test]
    public void Specialization_ShouldThrowAnEmptyStringException_ForEmptySpecializationString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "akowal@gmail.com", "", "Intermediate");
        });
    }

    [Test]
    public void ExperienceLevel_ShouldThrowAnEmptyStringException_ForEmptyExperienceLevelString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "akowal@gmail.com", "Surgery", "");
        });
    }
    
}
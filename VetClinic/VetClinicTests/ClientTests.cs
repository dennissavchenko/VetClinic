using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class ClientTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Client.json";
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
    public void AddToExtent_ShouldAddClientCorrectly()
    {
        // Arrange
        Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        Pet pet2 = new Pet("kk", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        var client1 = new Client("Maciej", "Dominiak", "334499950", "Maciej@gmail.com", [pet1]);
        var client2 = new Client("Marco", "Rossi", "334494350", "Marco@gmail.com", [pet2]);

        // Act
        var extent = Client.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("FirstName=Maciej"));
        Assert.That(extent[0].Contains("LastName=Dominiak"));
        Assert.That(extent[0].Contains("PhoneNumber=334499950"));
        Assert.That(extent[0].Contains("Email=Maciej@gmail.com"));
        Assert.That(extent[1].Contains("Id=2"));
        Assert.That(extent[1].Contains("FirstName=Marco"));
        Assert.That(extent[1].Contains("LastName=Rossi"));
        Assert.That(extent[1].Contains("PhoneNumber=334494350"));
        Assert.That(extent[1].Contains("Email=Marco@gmail.com"));

    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        var client = new Client("Marta", "Ostrowska", "773712000", "Marta@gmail.com", [pet1]);

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"FirstName\": \"Marta\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Ostrowska\""));
        Assert.IsTrue(json.Contains("\"PhoneNumber\": \"773712000\""));
        Assert.IsTrue(json.Contains("\"Email\": \"Marta@gmail.com\""));

    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"FirstName\": \"Marek\", \"LastName\": \"Kowalski\", \"PhoneNumber\": \"081821727\", \"Email\": \"marek@gmail.com\" }]");

        // Act
        var extent = Client.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("FirstName=Marek"));
        Assert.That(extent[0].Contains("LastName=Kowalski"));
        Assert.That(extent[0].Contains("PhoneNumber=081821727"));
        Assert.That(extent[0].Contains("Email=marek@gmail.com"));


    }

    [Test]
    public void FirstName_ShouldThrowAnEmptyStringException_ForEmptyFirstNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("", "Kowalski", "828222222", "a@gmail.com", [pet1]);

        });
    }

    [Test]
    public void LastName_ShouldThrowAnEmptyStringException_ForEmptyLastNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "", "828222222", "a@gmail.com", [pet1]);

        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnEmptyStringException_ForEmptyPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "", "a@gmail.com", [pet1]);

        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnInvalidFormatException_ForInvalidPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222", "annakowal@gmail.com", [pet1]);

        });
    }
    
    [Test]
    public void Email_ShouldThrowAnEmptyStringException_ForEmptyEmailString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "", [pet1]);

        });
    }

    [Test]
    public void Email_ShouldThrowAnInvalidDataException_ForInvalidEmailString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowalgmail.com", [pet1]);

        });
    }
    
    [Test]
    public void Pets_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() =>
        {
            // Arrange
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", []);

        });
    }
    
    [Test]
    public void Pets_ShouldThrowADuplicateException()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1, pet1]);

        });
    }

    [Test]
    public void ShouldCreateAssociationBetweenClientAndPetCorrectly()
    {
        // Arrange & Act
        Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1]);
        
        //Assert
        Assert.That(pet1.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet1));
    }
    
    [Test]
    public void AddPet_ShouldCreateAssociationBetweenClientAndPetCorrectly()
    {
        // Arrange & Act
        Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        Pet pet2 = new Pet("Piołun1", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1]);
        
        client.AddPet(pet2);
        
        //Assert
        Assert.That(pet1.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet1));
        Assert.That(pet2.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet2));
    }
    
    [Test]
    public void AddPet_ShouldThrowADuplicateException()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            // Arrange
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1, pet1]);
            client.AddPet(pet1);
        });
    }

    [Test]
    public void AddPet_ShouldThrowANullReferenceException()
    {
        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange & Act
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1]);
            client.AddPet(null!);
        });
    }

    [Test]
    public void RemovePet_ShouldRemoveAssociationBetweenClientAndPetCorrectly()
    {
        // Arrange
        Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        Pet pet2 = new Pet("Piołun1", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1, pet2]);
        
        // Act
        client.RemovePet(pet1);
        
        // Assert
        Assert.That(pet1.GetClient() != null);
        Assert.That(!client.GetPets().Contains(pet1));
        Assert.That(client.GetPets().Contains(pet2));
    }
    
    [Test]
    public void RemovePet_ShouldThrowANotFoundException()
    {
        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
            // Arrange & Act
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            Pet pet2 = new Pet("Piołun1", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            Pet pet3 = new Pet("Piołun2", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1, pet2]);
            client.RemovePet(pet3);
        });
    }
    
    [Test]
    public void RemovePet_ShouldThrowAnEmptyListException()
    {
        // Assert
        Assert.Throws<EmptyListException>(() =>
        {
            // Arrange & Act
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            Pet pet2 = new Pet("Piołun1", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1]);
            client.RemovePet(pet2);
        });
    }
    
    [Test]
    public void RemovePet_ShouldThrowANullReferenceException()
    {
        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange & Act
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            Pet pet2 = new Pet("Piołun1", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1, pet2]);
            client.RemovePet(null!);
        });
    }

    [Test]
    public void RemoveClient_ShouldRemoveClientCorrectly()
    {
        // Arrange
        Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
        var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1]);
        
        // Act
        client.RemoveClient();
        
        // Assert
        Assert.That(!Client.GetCurrentExtent().Contains(client));
        Assert.That(!client.GetPets().Contains(pet1));
        Assert.That(!Pet.GetCurrentExtent().Contains(pet1));
    }
    
    [Test]
    public void RemoveClient_ShouldThrowANotFoundException()
    {
        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
            // Arrange & Act
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            Pet pet2 = new Pet("Piołun1", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            var client = new Client("Anna", "Kowalski", "828222222", "annakowal@gmail.com", [pet1, pet2]);
            client.RemoveClient();
            client.RemoveClient();
        });
    }

}
    


  







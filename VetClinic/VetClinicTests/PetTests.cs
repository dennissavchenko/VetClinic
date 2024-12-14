using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class PetTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Pet.json";
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
    public void AddToExtent_ShouldAddPetCorrectly()
    {
        Specie cat = new Specie("Cat", "Felis catus");
        // Arrange
        var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
        var pat2 = new Pet("Momo", Sex.Male, 10, new DateTime(2018, 6, 12), [Color.Black, Color.White]);

        // Act
        var extent = Pet.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Buddy"));
        Assert.IsTrue(extent[0].Contains("Sex=Male"));
        Assert.IsTrue(extent[0].Contains("Weight=15.5"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("Colors=(Brown)"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=10"));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var pet = new Pet("Rex", Sex.Male, 22.0, new DateTime(2017, 3, 20), [Color.Black, Color.White]);

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Rex\""));
        Assert.IsTrue(json.Contains("\"Sex\": 0"));
        Assert.IsTrue(json.Contains("\"Weight\": 22"));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath,
            "[{\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":5.5,\"DateOfBirth\":\"2020-05-01T00:00:00\",\"Colors\":[9]}]");

        // Act
        var extent = Pet.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=5.5"));
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var pet = new Pet("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow]);

        // Act
        int age = pet.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2020));
    }

    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var pet = new Pet("", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow]);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, -0.5, new DateTime(2020, 1, 1), [Color.Yellow]);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.Yellow]);
        });
    }

    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() =>
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), []);
        });
    }

    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black, Color.Black]);
        });
    }

    [Test]
    public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black]);
            pet.AddSpecie(null!);
        });
    }

    [Test]
    public void AddSpecie_ShouldAddSpecieCorrectly()
    {
        // Arrange
        var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black]);
        var cat = new Specie("Cat", "Felis catus");
        // Act
        pet.AddSpecie(cat);

        // Assert
        Assert.That(pet.GetSpecie()!.Equals(cat));
    }

    [Test]
    public void RemoveSpecie_ShouldAddSpecieCorrectly()
    {
        // Arrange
        var pet1 = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black]);
        var cat = new Specie("Cat", "Felis catus");
        // Act
        pet1.AddSpecie(cat);
        pet1.RemoveSpecie();
        // Assert
        Assert.IsNull(pet1.GetSpecie());
    }

    [Test]
    public void RemoveSpecie_ShouldThrowANullReferenceException()
    {
        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange & Act
            var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black]);
            pet.RemoveSpecie();
        });
    }

    [Test]
    public void ModifyClient_ShouldAddClientCorrectly()
    {
        // Arrange & Act
        var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
        var client = new Client("Marek", "Kowalski", "828222222", "kw@gmail.com", [pet1]);

        // Assert
        Assert.That(pet1.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet1));

    }

    [Test]
    public void Pet_ShouldAddClientCorrectly_AssignToDummyClient()
    {
        // Arrange & Act
        var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);

        // Assert
        Assert.That(pet1.GetClient() != null);
    }

    [Test]
    public void ModifyClient_ShouldModifyClientCorrectly()
    {
        // Arrange & Act
        var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
        var pet2 = new Pet("Buddy1", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
        var client = new Client("Marek", "Kowalski", "828222222", "kw@gmail.com", [pet1]);
        pet2.ModifyClient(client);

        Assert.That(pet2.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet2));
        Assert.That(pet1.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet1));
    }

    [Test]
    public void ModifyClient_ShouldThrowANullReferenceException()
    {
        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange & Act
            var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
            pet1.ModifyClient(null!);
        });
    }

    [Test]
    public void Pet_ShouldThrowANullReferenceException_ForNullClient()
    {
        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange & Act
            var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown], null!);
        });
    }

    [Test]
    public void Pet_ShouldAddClientCorrectly_WithConstructor()
    {
        // Arrange
        var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
        var client = new Client("Marek", "Kowalski", "828222222", "kw@gmail.com", [pet1]);
        
        //Act
        var pet2 = new Pet("Buddy1", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown], client);
        
        //Assert
        Assert.That(pet2.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet2));
        Assert.That(pet1.GetClient().Equals(client));
        Assert.That(client.GetPets().Contains(pet1));
    }

    [Test]
    public void RemovePet_ShouldRemovePetCorrectly()
    {
        // Arrange
        var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
        var pet2 = new Pet("Momo", Sex.Male, 10, new DateTime(2018, 6, 12), [Color.Black, Color.White]);
        
        var cat = new Specie("Cat", "Felis catus");
        
        cat.AddPet(pet1);
        cat.AddPet(pet2);
        
        var client = new Client("Marek", "Kowalski", "828222222", "kw@gmail.com", [pet1, pet2]);
        
        //Act
        pet1.RemovePet();
        
        //Assert
        Assert.That(!Pet.GetCurrentExtent().Contains(pet1));
        Assert.That(!client.GetPets().Contains(pet1));
        Assert.That(pet1.GetSpecie() == null);
        Assert.That(!cat.GetPets().Contains(pet1));
        Assert.That(!pet1.GetClient().Equals(client));
    }
    
    [Test]
    public void RemovePet_ShouldThrowANotFoundException()
    {
        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
            // Arrange & Act
            Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
            pet1.RemovePet();
            pet1.RemovePet();
        });
    }
    
    [Test]
    public void AddAppointment_ShouldAssociateAppointmentWithPet()
        {
            // Arrange
            var pet = new Pet { Id = 1 };
            var appointment = new Appointment 
            { 
                Id = 100, 
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled, 
                Price = 150 
            };

            // Act
            pet.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(pet.HasAppointments(), "Pet should have at least one appointment after AddAppointment.");
            Assert.That(appointment == pet.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(pet == appointment.GetPet(), "Appointment should reference the Pet after assignment.");
        }

    [Test]
    public void AddAppointment_ShouldAllowMultipleAppointmentsForSamePet()
    {
        // Arrange
        var pet = new Pet { Id = 1 };
        var appointment1 = new Appointment 
        { 
            Id = 100, 
            DateTime = DateTime.Now.AddDays(1),
            State = AppointmentState.Scheduled, 
            Price = 100 
        };
        var appointment2 = new Appointment 
        { 
            Id = 200, 
            DateTime = DateTime.Now.AddDays(2),
            State = AppointmentState.Scheduled, 
            Price = 200 
        };

        // Act
        pet.AddAppointment(appointment1);
        pet.AddAppointment(appointment2);

        // Assert
        Assert.That(2 == pet.GetAppointments().Count, "Pet should have exactly two appointments.");
        Assert.Contains(appointment1, pet.GetAppointments(), "Pet should contain the first appointment.");
        Assert.Contains(appointment2, pet.GetAppointments(), "Pet should contain the second appointment.");
    }

    [Test]
    public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
    {
        // Arrange
        var pet = new Pet { Id = 1 };
        var appointment1 = new Appointment 
        { 
            Id = 100, 
            DateTime = DateTime.Now.AddDays(1),
            State = AppointmentState.Scheduled,
            Price = 150 
        };
        var appointment2 = new Appointment
        {
            Id = 100, // same ID as appointment1
            DateTime = DateTime.Now.AddDays(3),
            State = AppointmentState.Scheduled,
            Price = 200
        };

        pet.AddAppointment(appointment1);

        // Act & Assert
        Assert.Throws<DuplicatesException>(() => pet.AddAppointment(appointment2));
    }

    [Test]
    public void RemoveAppointment_ShouldRemoveExistingAppointment()
    {
        // Arrange
        var pet = new Pet { Id = 1 };
        var appointment = new Appointment 
        { 
            Id = 100,
            DateTime = DateTime.Now.AddDays(1),
            State = AppointmentState.Scheduled,
            Price = 150
        };

        pet.AddAppointment(appointment);

        // Act
        pet.RemoveAppointment(appointment.Id);

        // Assert
        Assert.IsFalse(pet.HasAppointments(), "Pet should have no appointments after removal.");
        Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference Pet after removal.");
    }

    [Test]
    public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
    {
        // Arrange
        var pet = new Pet { Id = 1 };

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => pet.RemoveAppointment(999));
    }

    [Test]
    public void GetAppointmentById_ShouldReturnCorrectAppointment()
    {
        // Arrange
        var pet = new Pet { Id = 1 };
        var appointment1 = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };
        var appointment2 = new Appointment { Id = 200, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 200 };

        pet.AddAppointment(appointment1);
        pet.AddAppointment(appointment2);

        // Act
        var fetchedAppointment = pet.GetAppointmentById(200);

        // Assert
        Assert.That(appointment2 == fetchedAppointment);
    }

    [Test]
    public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
    {
        // Arrange
        var pet = new Pet { Id = 1 };
        var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };
        pet.AddAppointment(appointment);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => pet.GetAppointmentById(999));
    }
    
}

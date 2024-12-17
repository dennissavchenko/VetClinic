using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class MammalTests
{
    private string _testPathPet, _testPathMammal;

    [SetUp]
    public void Setup()
    {
        _testPathMammal = "../../../Data/Mammal.json";
        _testPathPet = "../../../Data/Pet.json";
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
        var mammal1 = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 6, 15), [Color.Brown, Color.White], true);
        var mammal2 = new Mammal("Momo", Sex.Male, 4.6, new DateTime(2017, 5, 1), [Color.White, Color.Black], false);

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
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 6, 15), [Color.Brown, Color.White],true);

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
        var mammal = new Mammal("Bella", Sex.Female, 12.5, new DateTime(2019, 1, 1), [Color.Brown], true);

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
            var mammal = new Mammal("", Sex.Female, 12.5, new DateTime(2019, 1, 1), [Color.Brown], true);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, -12.5, new DateTime(2019, 1, 1), [Color.Brown], true);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(1), [Color.Brown], true);
        });
    }
    
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(-10), [], true);
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var mammal = new Mammal("Bella", Sex.Female, 12.5, DateTime.Now.AddDays(-10), [Color.Brown, Color.Brown], true);
        });
    }
    
            [Test]
        public void AddSpecie_ShouldAddSpecieCorrectly_BidirectionalCheck()
        {
            // Arrange
            var mammal = new Mammal(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                nocturnal: false
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            mammal.AddSpecie(cat);

            // Assert
            Assert.That(mammal.GetSpecie()!.Equals(cat), "Mammal should have the assigned Specie.");
            Assert.That(cat.GetPets().Contains(mammal), "Specie should also reference the Mammal (bidirectional).");
        }

        [Test]
        public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var mammal = new Mammal(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    nocturnal: false
                );

                // Act
                mammal.AddSpecie(null!);  // Should throw NullReferenceException
            });
        }

        [Test]
        public void RemoveSpecie_ShouldRemoveSpecieCorrectly()
        {
            // Arrange
            var mammal = new Mammal(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                nocturnal: true
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            mammal.AddSpecie(cat);
            mammal.RemoveSpecie();

            // Assert
            Assert.IsNull(mammal.GetSpecie(), "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(mammal), "Cat should not reference the Mammal after removal.");
        }

        [Test]
        public void RemoveSpecie_ShouldThrowANullReferenceException_WhenNoSpecieAssigned()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var mammal = new Mammal(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    nocturnal: false
                );

                // Attempting to remove a specie when none is assigned
                mammal.RemoveSpecie();
            });
        }

        [Test]
        public void ModifyClient_ShouldAddClientCorrectly()
        {
            // Arrange & Act
            var mammal = new Mammal(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                nocturnal: true
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { mammal }
            );

            // Assert
            Assert.That(mammal.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(mammal));
        }

        [Test]
        public void Mammal_ShouldAddClientCorrectly_AssignToDummyClient()
        {
            // Arrange & Act
            var mammal = new Mammal(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                nocturnal: false
            );

            // If base Pet constructor assigns a dummy client, just confirm non-null
            Assert.That(mammal.GetClient() != null, "Client should not be null if assigned by default constructor logic.");
        }

        [Test]
        public void ModifyClient_ShouldModifyClientCorrectly()
        {
            // Arrange
            var mammal1 = new Mammal(
                "Buddy1",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                nocturnal: false
            );

            var mammal2 = new Mammal(
                "Buddy2",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                nocturnal: true
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { mammal1 }
            );

            // Act
            mammal2.ModifyClient(client);

            // Assert
            Assert.That(mammal2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(mammal2));
            Assert.That(mammal1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(mammal1));
        }

        [Test]
        public void ModifyClient_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var mammal = new Mammal(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    nocturnal: true
                );

                mammal.ModifyClient(null!);
            });
        }

        [Test]
        public void Mammal_ShouldThrowANullReferenceException_ForNullClient()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var mammal = new Mammal(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    client: null!,
                    nocturnal: true
                );
            });
        }

        [Test]
        public void Mammal_ShouldAddClientCorrectly_WithConstructor()
        {
            // Arrange
            var mammal1 = new Mammal(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                nocturnal: true
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { mammal1 }
            );

            //Act
            var mammal2 = new Mammal(
                "Buddy2",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                client,
                nocturnal: false
            );

            //Assert
            Assert.That(mammal2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(mammal2));
            Assert.That(mammal1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(mammal1));
        }

        [Test]
        public void RemovePet_ShouldRemoveMammalCorrectly()
        {
            // Arrange
            var mammal1 = new Mammal(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                nocturnal: true
            );
            var mammal2 = new Mammal(
                "Momo",
                Sex.Male,
                10,
                new DateTime(2018, 6, 12),
                new List<Color> { Color.Black, Color.White },
                nocturnal: false
            );

            var cat = new Specie("Cat", "Felis catus");
            cat.AddPet(mammal1);
            cat.AddPet(mammal2);

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { mammal1, mammal2 }
            );

            //Act
            mammal1.RemovePet();

            //Assert
            Assert.That(!Mammal.GetCurrentExtent().Contains(mammal1), "Removed mammal1 should not be in Mammal’s extent anymore.");
            Assert.That(!client.GetPets().Contains(mammal1), "Client should no longer reference mammal1 after removal.");
            Assert.That(mammal1.GetSpecie() == null, "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(mammal1), "Cat should no longer contain mammal1.");
            Assert.That(mammal1.GetClient() != client, "mammal1’s client reference should be cleared after removal.");
        }

        [Test]
        public void RemovePet_ShouldThrowANotFoundException()
        {
            // Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange & Act
                var mammal1 = new Mammal(
                    "Piołun",
                    Sex.Male,
                    5.2,
                    new DateTime(2019, 5, 12),
                    new List<Color> { Color.Black, Color.White },
                    nocturnal: true
                );

                // First removal is valid
                mammal1.RemovePet();

                // Second removal should throw NotFoundException (already removed)
                mammal1.RemovePet();
            });
        }

        [Test]
        public void AddAppointment_ShouldAssociateAppointmentWithMammal()
        {
            // Arrange
            var mammal = new Mammal
            {
                Id = 1,
                Nocturnal = true
            };

            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            // Act
            mammal.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(mammal.HasAppointments(), "Mammal should have at least one appointment after AddAppointment.");
            Assert.That(appointment == mammal.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(mammal == appointment.GetPet(), "Appointment should reference the Mammal after assignment.");
        }

        [Test]
        public void AddAppointment_ShouldAllowMultipleAppointmentsForSameMammal()
        {
            // Arrange
            var mammal = new Mammal
            {
                Id = 1,
                Nocturnal = false
            };

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
            mammal.AddAppointment(appointment1);
            mammal.AddAppointment(appointment2);

            // Assert
            Assert.That(mammal.GetAppointments().Count == 2, "Mammal should have exactly two appointments.");
            Assert.Contains(appointment1, mammal.GetAppointments(), "Should contain the first appointment.");
            Assert.Contains(appointment2, mammal.GetAppointments(), "Should contain the second appointment.");
        }

        [Test]
        public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
        {
            // Arrange
            var mammal = new Mammal { Id = 1, Nocturnal = true };

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

            mammal.AddAppointment(appointment1);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => mammal.AddAppointment(appointment2));
        }

        [Test]
        public void RemoveAppointment_ShouldRemoveExistingAppointment()
        {
            // Arrange
            var mammal = new Mammal { Id = 1, Nocturnal = false };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            mammal.AddAppointment(appointment);

            // Act
            mammal.RemoveAppointment(appointment.Id);

            // Assert
            Assert.IsFalse(mammal.HasAppointments(), "Mammal should have no appointments after removal.");
            Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference the Mammal after removal.");
        }

        [Test]
        public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
        {
            // Arrange
            var mammal = new Mammal { Id = 1, Nocturnal = true };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => mammal.RemoveAppointment(999));
        }

        [Test]
        public void GetAppointmentById_ShouldReturnCorrectAppointment()
        {
            // Arrange
            var mammal = new Mammal { Id = 1, Nocturnal = true };
            var appointment1 = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            var appointment2 = new Appointment
            {
                Id = 200,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 200
            };

            mammal.AddAppointment(appointment1);
            mammal.AddAppointment(appointment2);

            // Act
            var fetchedAppointment = mammal.GetAppointmentById(200);

            // Assert
            Assert.That(appointment2 == fetchedAppointment);
        }

        [Test]
        public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
        {
            // Arrange
            var mammal = new Mammal { Id = 1, Nocturnal = true };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            mammal.AddAppointment(appointment);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => mammal.GetAppointmentById(999));
        }
    
}


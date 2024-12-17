using VetClinic;
using VetClinic.Exceptions;
using DateTime = System.DateTime;

namespace VetClinicTests;

public class HealthyTests
{
    private string _testPathPet, _testPathHealthy;

    [SetUp]
    public void Setup()
    {
        _testPathHealthy = "../../../Data/Healthy.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathHealthy);
        File.Delete(_testPathPet);
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathHealthy))
        {
            File.Delete(_testPathHealthy);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    [Test]
    public void AddToExtent_ShouldAddHealthyPetCorrectly()
    {
        // Arrange
        var healthyPet1 = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1),  [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));
        var healthyPet2 = new Healthy("Tom", Sex.Male, 12.6, new DateTime(2017, 1, 11), [Color.White], ActivityLevel.High, null);

        // Act
        var extent = Healthy.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("ActivityLevel=Medium"));
        Assert.IsTrue(extent[0].Contains("LastVaccinationDate=2020-05-10"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Tom"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=12.6"));
        Assert.IsTrue(extent[1].Contains("ActivityLevel=High"));
        Assert.IsTrue(extent[1].Contains("LastVaccinationDate=NotVaccinated"));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));

        // Act
        var json = File.ReadAllText(_testPathHealthy);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Bella\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1"));
        Assert.IsTrue(json.Contains("\"Weight\": 8"));
        Assert.IsTrue(json.Contains("\"ActivityLevel\": 1"));
        Assert.IsTrue(json.Contains("\"LastVaccinationDate\": \"2020-05-10T00:00:00\""));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathHealthy, "[{\"ActivityLevel\":1,\"LastVaccinationDate\":\"2020-05-10T00:00:00\",\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":8.0,\"DateOfBirth\":\"2018-05-01T00:00:00\",\"Colors\":[1],\"Age\":5}]");

        // Act
        var extent = Healthy.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Bella"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=8"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("ActivityLevel=Medium"));
        Assert.IsTrue(extent[0].Contains("LastVaccinationDate=2020-05-10"));
        
    }
    
    [Test]
    public void Name_ShouldThrowEmptyStringException_ForEmptyName()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));
        });
    }

    [Test]
    public void Weight_ShouldThrowNegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, -1.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));
        });
    }
    
    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var healthyPet = new Healthy("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.White], ActivityLevel.High, new DateTime(2020, 1, 1));
        });
    }

    [Test]
    public void LastVaccinationDate_ShouldThrowInvalidDateException_ForFutureDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, DateTime.Now.AddDays(1));
        });
    }

    [Test]
    public void LastVaccinationDate_ShouldThrowInvalidDateException_IfBeforeDateOfBirth()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2017, 1, 1));
        });
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.White], ActivityLevel.Medium, new DateTime(2020, 5, 10));

        // Act
        int age = healthyPet.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2018));
    }
    
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [], ActivityLevel.Medium, new DateTime(2020, 1, 1));
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var healthyPet = new Healthy("Bella", Sex.Female, 8.0, new DateTime(2018, 5, 1), [Color.Black, Color.Black], ActivityLevel.Medium, new DateTime(2020, 1, 1));
        });
    }
    
    [Test]
        public void AddSpecie_ShouldAddSpecieCorrectly_BidirectionalCheck()
        {
            // Arrange
            var healthy = new Healthy(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                ActivityLevel.Low,
                null  // LastVaccinationDate is optional, passing null
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            healthy.AddSpecie(cat);

            // Assert
            Assert.That(healthy.GetSpecie()!.Equals(cat), "Healthy pet should have the assigned Specie.");
            Assert.That(cat.GetPets().Contains(healthy), "Specie should also reference the Healthy pet (bidirectional).");
        }

        [Test]
        public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var healthy = new Healthy(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    ActivityLevel.Low,
                    null
                );

                // Act
                healthy.AddSpecie(null!);  // Should throw NullReferenceException
            });
        }

        [Test]
        public void RemoveSpecie_ShouldRemoveSpecieCorrectly()
        {
            // Arrange
            var healthy = new Healthy(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                ActivityLevel.Medium,
                null
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            healthy.AddSpecie(cat);
            healthy.RemoveSpecie();

            // Assert
            Assert.IsNull(healthy.GetSpecie(), "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(healthy), "Cat should not reference the Healthy pet after removal.");
        }

        [Test]
        public void RemoveSpecie_ShouldThrowANullReferenceException_WhenNoSpecieAssigned()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var healthy = new Healthy(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    ActivityLevel.Medium,
                    null
                );

                // Attempting to remove a specie when none is assigned
                healthy.RemoveSpecie();
            });
        }

        [Test]
        public void ModifyClient_ShouldAddClientCorrectly()
        {
            // Arrange & Act
            var healthy = new Healthy(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                ActivityLevel.High,
                DateTime.Now.AddMonths(-6)
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { healthy }
            );

            // Assert
            Assert.That(healthy.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(healthy));
        }

        [Test]
        public void Healthy_ShouldAddClientCorrectly_AssignToDummyClient()
        {
            // Arrange & Act
            var healthy = new Healthy(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                ActivityLevel.Medium,
                null
            );

            // If base constructor for Pet automatically assigns a dummy client, just confirm non-null
            Assert.That(healthy.GetClient() != null, "Client should not be null if assigned by default constructor logic.");
        }

        [Test]
        public void ModifyClient_ShouldModifyClientCorrectly()
        {
            // Arrange
            var healthy1 = new Healthy(
                "Buddy1",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                ActivityLevel.Low,
                null
            );

            var healthy2 = new Healthy(
                "Buddy2",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                ActivityLevel.High,
                null
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { healthy1 }
            );

            // Act
            healthy2.ModifyClient(client);

            // Assert
            Assert.That(healthy2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(healthy2));
            Assert.That(healthy1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(healthy1));
        }

        [Test]
        public void ModifyClient_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var healthy = new Healthy(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    ActivityLevel.Medium,
                    null
                );

                healthy.ModifyClient(null!);
            });
        }

        [Test]
        public void Healthy_ShouldThrowANullReferenceException_ForNullClient()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var healthy = new Healthy(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    client: null!,
                    activityLevel: ActivityLevel.Low,
                    lastVaccinationDate: null
                );
            });
        }

        [Test]
        public void Healthy_ShouldAddClientCorrectly_WithConstructor()
        {
            // Arrange
            var healthy1 = new Healthy(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                ActivityLevel.High,
                null
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { healthy1 }
            );

            //Act
            var healthy2 = new Healthy(
                "Buddy2",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                client,
                activityLevel: ActivityLevel.Low,
                lastVaccinationDate: DateTime.Now.AddMonths(-1)
            );

            //Assert
            Assert.That(healthy2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(healthy2));
            Assert.That(healthy1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(healthy1));
        }

        [Test]
        public void RemovePet_ShouldRemoveHealthyCorrectly()
        {
            // Arrange
            var healthy1 = new Healthy(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                ActivityLevel.Low,
                null
            );
            var healthy2 = new Healthy(
                "Momo",
                Sex.Male,
                10,
                new DateTime(2018, 6, 12),
                new List<Color> { Color.Black, Color.White },
                ActivityLevel.High,
                null
            );

            var cat = new Specie("Cat", "Felis catus");
            cat.AddPet(healthy1);
            cat.AddPet(healthy2);

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { healthy1, healthy2 }
            );

            //Act
            healthy1.RemovePet();

            //Assert
            Assert.That(!Healthy.GetCurrentExtent().Contains(healthy1), "Removed healthy1 should not be in Healthy’s extent anymore.");
            Assert.That(!client.GetPets().Contains(healthy1), "Client should no longer reference healthy1 after removal.");
            Assert.That(healthy1.GetSpecie() == null, "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(healthy1), "Cat should no longer contain healthy1.");
            Assert.That(healthy1.GetClient() != client, "healthy1’s client reference should be cleared after removal.");
        }

        [Test]
        public void RemovePet_ShouldThrowANotFoundException()
        {
            // Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange & Act
                var healthy1 = new Healthy(
                    "Piołun",
                    Sex.Male,
                    5.2,
                    new DateTime(2019, 5, 12),
                    new List<Color> { Color.Black, Color.White },
                    ActivityLevel.Medium,
                    null
                );

                // First removal is valid
                healthy1.RemovePet();

                // Second removal should throw NotFoundException (already removed)
                healthy1.RemovePet();
            });
        }

        [Test]
        public void AddAppointment_ShouldAssociateAppointmentWithHealthy()
        {
            // Arrange
            var healthy = new Healthy
            {
                Id = 1,
                ActivityLevel = ActivityLevel.High,
                LastVaccinationDate = null
            };

            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            // Act
            healthy.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(healthy.HasAppointments(), "Healthy pet should have at least one appointment after AddAppointment.");
            Assert.That(appointment == healthy.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(healthy == appointment.GetPet(), "Appointment should reference the Healthy pet after assignment.");
        }

        [Test]
        public void AddAppointment_ShouldAllowMultipleAppointmentsForSameHealthy()
        {
            // Arrange
            var healthy = new Healthy
            {
                Id = 1,
                ActivityLevel = ActivityLevel.Medium,
                LastVaccinationDate = null
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
            healthy.AddAppointment(appointment1);
            healthy.AddAppointment(appointment2);

            // Assert
            Assert.That(healthy.GetAppointments().Count == 2, "Healthy pet should have exactly two appointments.");
            Assert.Contains(appointment1, healthy.GetAppointments(), "Should contain the first appointment.");
            Assert.Contains(appointment2, healthy.GetAppointments(), "Should contain the second appointment.");
        }

        [Test]
        public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
        {
            // Arrange
            var healthy = new Healthy { Id = 1, ActivityLevel = ActivityLevel.Low };

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

            healthy.AddAppointment(appointment1);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => healthy.AddAppointment(appointment2));
        }

        [Test]
        public void RemoveAppointment_ShouldRemoveExistingAppointment()
        {
            // Arrange
            var healthy = new Healthy { Id = 1, ActivityLevel = ActivityLevel.Medium };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            healthy.AddAppointment(appointment);

            // Act
            healthy.RemoveAppointment(appointment.Id);

            // Assert
            Assert.IsFalse(healthy.HasAppointments(), "Healthy pet should have no appointments after removal.");
            Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference the Healthy pet after removal.");
        }

        [Test]
        public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
        {
            // Arrange
            var healthy = new Healthy { Id = 1, ActivityLevel = ActivityLevel.Low };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => healthy.RemoveAppointment(999));
        }

        [Test]
        public void GetAppointmentById_ShouldReturnCorrectAppointment()
        {
            // Arrange
            var healthy = new Healthy { Id = 1, ActivityLevel = ActivityLevel.High };
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

            healthy.AddAppointment(appointment1);
            healthy.AddAppointment(appointment2);

            // Act
            var fetchedAppointment = healthy.GetAppointmentById(200);

            // Assert
            Assert.That(appointment2 == fetchedAppointment);
        }

        [Test]
        public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
        {
            // Arrange
            var healthy = new Healthy { Id = 1, ActivityLevel = ActivityLevel.Medium };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            healthy.AddAppointment(appointment);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => healthy.GetAppointmentById(999));
        }
    
}

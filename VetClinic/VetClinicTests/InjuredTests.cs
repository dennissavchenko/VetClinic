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
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("InjuryType=Wound"));
        Assert.IsTrue(extent[0].Contains("InjuryDate=2021-05-10"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=4.6"));
        Assert.IsTrue(extent[1].Contains("InjuryType=Sprain"));
        Assert.IsTrue(extent[1].Contains("InjuryDate=2020-05-10"));
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
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
        Assert.IsTrue(extent[0].Contains("Colors=(White)"));
        Assert.IsTrue(extent[0].Contains("InjuryType=Fracture"));
        Assert.IsTrue(extent[0].Contains("InjuryDate=2021-05-10"));
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
    
    [Test]
        public void AddSpecie_ShouldAddSpecieCorrectly_BidirectionalCheck()
        {
            // Arrange
            var injured = new Injured(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                injuryType: InjuryType.Wound,
                injuryDate: DateTime.Now.AddDays(-5)
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            injured.AddSpecie(cat);

            // Assert
            Assert.That(injured.GetSpecie()!.Equals(cat), "Injured pet should have the assigned Specie.");
            Assert.That(cat.GetPets().Contains(injured), "Specie should also reference the Injured pet (bidirectional).");
        }

        [Test]
        public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var injured = new Injured(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    injuryType: InjuryType.Wound,
                    injuryDate: DateTime.Now.AddDays(-5)
                );

                // Act
                injured.AddSpecie(null!);  // Should throw NullReferenceException
            });
        }

        [Test]
        public void RemoveSpecie_ShouldRemoveSpecieCorrectly()
        {
            // Arrange
            var injured = new Injured(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                injuryType: InjuryType.Wound,
                injuryDate: DateTime.Now.AddDays(-5)
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            injured.AddSpecie(cat);
            injured.RemoveSpecie();

            // Assert
            Assert.IsNull(injured.GetSpecie(), "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(injured), "Cat should not reference the Injured pet after removal.");
        }

        [Test]
        public void RemoveSpecie_ShouldThrowANullReferenceException_WhenNoSpecieAssigned()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var injured = new Injured(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    injuryType: InjuryType.Wound,
                    injuryDate: DateTime.Now.AddDays(-5)
                );

                // Attempting to remove a specie when none is assigned
                injured.RemoveSpecie();
            });
        }

        [Test]
        public void ModifyClient_ShouldAddClientCorrectly()
        {
            // Arrange & Act
            var injured = new Injured(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                InjuryType.Fracture,
                new DateTime(2018, 6, 1) // injury date
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { injured }
            );

            // Assert
            Assert.That(injured.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(injured));
        }

        [Test]
        public void Injured_ShouldAddClientCorrectly_AssignToDummyClient()
        {
            // Arrange & Act
            var injured = new Injured(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                InjuryType.Fracture,
                new DateTime(2018, 5, 10)
            );

            // If base Pet constructor assigns a dummy client, just confirm non-null
            Assert.That(injured.GetClient() != null, "Client should not be null if assigned by default constructor logic.");
        }

        [Test]
        public void ModifyClient_ShouldModifyClientCorrectly()
        {
            // Arrange
            var injured1 = new Injured(
                "Buddy1",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                InjuryType.Fracture,
                new DateTime(2018, 5, 2)
            );

            var injured2 = new Injured(
                "Buddy2",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                InjuryType.Sprain,
                new DateTime(2018, 5, 3)
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { injured1 }
            );

            // Act
            injured2.ModifyClient(client);

            // Assert
            Assert.That(injured2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(injured2));
            Assert.That(injured1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(injured1));
        }

        [Test]
        public void ModifyClient_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var injured = new Injured(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    InjuryType.Fracture,
                    new DateTime(2018, 5, 2)
                );

                injured.ModifyClient(null!);
            });
        }

        [Test]
        public void Injured_ShouldThrowANullReferenceException_ForNullClient()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var injured = new Injured(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    client: null!,   // explicitly passing null
                    injuryType: InjuryType.Wound,
                    injuryDate: new DateTime(2018, 5, 5)
                );
            });
        }

        [Test]
        public void Injured_ShouldAddClientCorrectly_WithConstructor()
        {
            // Arrange
            var injured1 = new Injured(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                InjuryType.Sprain,
                new DateTime(2018, 5, 10)
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { injured1 }
            );

            //Act
            var injured2 = new Injured(
                "Buddy2",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                client, // pass client as parameter
                InjuryType.Wound,
                new DateTime(2018, 6, 15)
            );

            //Assert
            Assert.That(injured2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(injured2));
            Assert.That(injured1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(injured1));
        }

        [Test]
        public void RemovePet_ShouldRemoveInjuredCorrectly()
        {
            // Arrange
            var injured1 = new Injured(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                InjuryType.Fracture,
                new DateTime(2018, 5, 2)
            );
            var injured2 = new Injured(
                "Momo",
                Sex.Male,
                10,
                new DateTime(2018, 6, 12),
                new List<Color> { Color.Black, Color.White },
                InjuryType.Sprain,
                new DateTime(2018, 7, 1)
            );

            var cat = new Specie("Cat", "Felis catus");
            cat.AddPet(injured1);
            cat.AddPet(injured2);

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { injured1, injured2 }
            );

            //Act
            injured1.RemovePet();

            //Assert
            Assert.That(!Injured.GetCurrentExtent().Contains(injured1), "Removed injured1 should not be in Injured’s extent anymore.");
            Assert.That(!client.GetPets().Contains(injured1), "Client should no longer reference injured1 after removal.");
            Assert.That(injured1.GetSpecie() == null, "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(injured1), "Cat should no longer contain injured1.");
            Assert.That(injured1.GetClient() != client, "injured1’s client reference should be cleared after removal.");
        }

        [Test]
        public void RemovePet_ShouldThrowANotFoundException()
        {
            // Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange & Act
                var injured1 = new Injured(
                    "Piołun",
                    Sex.Male,
                    5.2,
                    new DateTime(2019, 5, 12),
                    new List<Color> { Color.Black, Color.White },
                    InjuryType.Fracture,
                    new DateTime(2019, 6, 1)
                );

                // First removal is valid
                injured1.RemovePet();

                // Second removal should throw NotFoundException (already removed)
                injured1.RemovePet();
            });
        }

        [Test]
        public void AddAppointment_ShouldAssociateAppointmentWithInjured()
        {
            // Arrange
            var injured = new Injured
            {
                Id = 1,
                InjuryType = InjuryType.Wound,
                InjuryDate = new DateTime(2023, 1, 1)
            };

            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            // Act
            injured.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(injured.HasAppointments(), "Injured pet should have at least one appointment after AddAppointment.");
            Assert.That(appointment == injured.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(injured == appointment.GetPet(), "Appointment should reference the Injured pet after assignment.");
        }

        [Test]
        public void AddAppointment_ShouldAllowMultipleAppointmentsForSameInjured()
        {
            // Arrange
            var injured = new Injured
            {
                Id = 1,
                InjuryType = InjuryType.Sprain,
                InjuryDate = new DateTime(2023, 1, 10)
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
            injured.AddAppointment(appointment1);
            injured.AddAppointment(appointment2);

            // Assert
            Assert.That(injured.GetAppointments().Count == 2, "Injured pet should have exactly two appointments.");
            Assert.Contains(appointment1, injured.GetAppointments(), "Should contain the first appointment.");
            Assert.Contains(appointment2, injured.GetAppointments(), "Should contain the second appointment.");
        }

        [Test]
        public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
        {
            // Arrange
            var injured = new Injured 
            { 
                Id = 1, 
                InjuryType = InjuryType.Fracture, 
                InjuryDate = new DateTime(2022, 5, 1) 
            };

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

            injured.AddAppointment(appointment1);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => injured.AddAppointment(appointment2));
        }

        [Test]
        public void RemoveAppointment_ShouldRemoveExistingAppointment()
        {
            // Arrange
            var injured = new Injured 
            { 
                Id = 1,
                InjuryType = InjuryType.Wound,
                InjuryDate = new DateTime(2023, 2, 1)
            };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            injured.AddAppointment(appointment);

            // Act
            injured.RemoveAppointment(appointment.Id);

            // Assert
            Assert.IsFalse(injured.HasAppointments(), "Injured pet should have no appointments after removal.");
            Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference the Injured pet after removal.");
        }

        [Test]
        public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
        {
            // Arrange
            var injured = new Injured 
            { 
                Id = 1, 
                InjuryType = InjuryType.Wound,
                InjuryDate = new DateTime(2022, 6, 1)
            };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => injured.RemoveAppointment(999));
        }

        [Test]
        public void GetAppointmentById_ShouldReturnCorrectAppointment()
        {
            // Arrange
            var injured = new Injured 
            { 
                Id = 1,
                InjuryType = InjuryType.Sprain,
                InjuryDate = new DateTime(2023, 1, 10)
            };
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

            injured.AddAppointment(appointment1);
            injured.AddAppointment(appointment2);

            // Act
            var fetchedAppointment = injured.GetAppointmentById(200);

            // Assert
            Assert.That(appointment2 == fetchedAppointment);
        }

        [Test]
        public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
        {
            // Arrange
            var injured = new Injured 
            { 
                Id = 1,
                InjuryType = InjuryType.Fracture,
                InjuryDate = new DateTime(2023, 1, 15)
            };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            injured.AddAppointment(appointment);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => injured.GetAppointmentById(999));
        }
    
}

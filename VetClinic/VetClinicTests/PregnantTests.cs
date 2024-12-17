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
    
            [Test]
        public void AddSpecie_ShouldAddSpecieCorrectly_BidirectionalCheck()
        {
            // Arrange
            var pregnant = new Pregnant(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                dueDate: DateTime.Now.AddDays(10),
                litterSize: 3
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            pregnant.AddSpecie(cat);

            // Assert
            Assert.That(pregnant.GetSpecie()!.Equals(cat), "Pregnant pet should have the assigned Specie.");
            Assert.That(cat.GetPets().Contains(pregnant), "Specie should also reference the Pregnant pet (bidirectional).");
        }

        [Test]
        public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var pregnant = new Pregnant(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    dueDate: DateTime.Now.AddDays(10),
                    litterSize: 3
                );

                // Act
                pregnant.AddSpecie(null!);
            });
        }

        [Test]
        public void RemoveSpecie_ShouldRemoveSpecieCorrectly()
        {
            // Arrange
            var pregnant = new Pregnant(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                dueDate: DateTime.Now.AddDays(10),
                litterSize: 3
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            pregnant.AddSpecie(cat);
            pregnant.RemoveSpecie();

            // Assert
            Assert.IsNull(pregnant.GetSpecie(), "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(pregnant), "Cat should not reference the Pregnant pet after removal.");
        }

        [Test]
        public void RemoveSpecie_ShouldThrowANullReferenceException_WhenNoSpecieAssigned()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var pregnant = new Pregnant(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    dueDate: DateTime.Now.AddDays(10),
                    litterSize: 3
                );

                // Act
                pregnant.RemoveSpecie();
            });
        }

        [Test]
        public void ModifyClient_ShouldAddClientCorrectly()
        {
            // Arrange & Act
            var pregnant = new Pregnant(
                "Buddy",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                dueDate: new DateTime(2018, 8, 1),
                litterSize: 5
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { pregnant }
            );

            // Assert
            Assert.That(pregnant.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(pregnant));
        }

        [Test]
        public void Pregnant_ShouldAddClientCorrectly_AssignToDummyClient()
        {
            // Arrange & Act
            var pregnant = new Pregnant(
                "Buddy",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                dueDate: new DateTime(2018, 8, 1),
                litterSize: 3
            );

            // If base Pet constructor (or your domain logic) assigns a dummy client automatically,
            // just confirm non-null
            Assert.That(pregnant.GetClient() != null);
        }

        [Test]
        public void ModifyClient_ShouldModifyClientCorrectly()
        {
            // Arrange
            var pregnant1 = new Pregnant(
                "Buddy1",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                dueDate: new DateTime(2018, 8, 1),
                litterSize: 3
            );

            var pregnant2 = new Pregnant(
                "Buddy2",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                dueDate: new DateTime(2018, 8, 1),
                litterSize: 4
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { pregnant1 }
            );

            // Act
            pregnant2.ModifyClient(client);

            // Assert
            Assert.That(pregnant2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(pregnant2));
            Assert.That(pregnant1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(pregnant1));
        }

        [Test]
        public void ModifyClient_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var pregnant = new Pregnant(
                    "Buddy",
                    Sex.Female,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    dueDate: new DateTime(2018, 8, 1),
                    litterSize: 3
                );

                pregnant.ModifyClient(null!);
            });
        }

        [Test]
        public void Pregnant_ShouldThrowANullReferenceException_ForNullClient()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var pregnant = new Pregnant(
                    "Buddy",
                    Sex.Female,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    client: null!, // explicitly passing null
                    dueDate: new DateTime(2018, 8, 1),
                    litterSize: 3
                );
            });
        }

        [Test]
        public void Pregnant_ShouldAddClientCorrectly_WithConstructor()
        {
            // Arrange
            var pregnant1 = new Pregnant(
                "Buddy",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                dueDate: new DateTime(2018, 8, 1),
                litterSize: 3
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { pregnant1 }
            );

            //Act
            var pregnant2 = new Pregnant(
                "Buddy2",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                client,
                dueDate: new DateTime(2018, 9, 1),
                litterSize: 5
            );

            //Assert
            Assert.That(pregnant2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(pregnant2));
            Assert.That(pregnant1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(pregnant1));
        }

        [Test]
        public void RemovePet_ShouldRemovePregnantCorrectly()
        {
            // Arrange
            var pregnant1 = new Pregnant(
                "Buddy",
                Sex.Female,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                dueDate: new DateTime(2018, 8, 1),
                litterSize: 3
            );
            var pregnant2 = new Pregnant(
                "Momo",
                Sex.Female,
                10,
                new DateTime(2018, 6, 12),
                new List<Color> { Color.Black, Color.White },
                dueDate: new DateTime(2018, 9, 1),
                litterSize: 4
            );

            var cat = new Specie("Cat", "Felis catus");
            cat.AddPet(pregnant1);
            cat.AddPet(pregnant2);

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { pregnant1, pregnant2 }
            );

            //Act
            pregnant1.RemovePet();

            //Assert
            Assert.That(!Pregnant.GetCurrentExtent().Contains(pregnant1), "Removed pregnant1 should not be in Pregnant's extent anymore.");
            Assert.That(!client.GetPets().Contains(pregnant1), "Client should no longer reference pregnant1 after removal.");
            Assert.That(pregnant1.GetSpecie() == null, "Specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(pregnant1), "Cat should no longer contain pregnant1.");
            Assert.That(pregnant1.GetClient() != client, "pregnant1’s client reference should be cleared after removal.");
        }

        [Test]
        public void RemovePet_ShouldThrowANotFoundException()
        {
            // Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange & Act
                var pregnant1 = new Pregnant(
                    "Piołun",
                    Sex.Female,
                    5.2,
                    new DateTime(2019, 5, 12),
                    new List<Color> { Color.Black, Color.White },
                    dueDate: new DateTime(2019, 9, 1),
                    litterSize: 3
                );

                // First removal is valid
                pregnant1.RemovePet();

                // Second removal should throw NotFoundException (already removed)
                pregnant1.RemovePet();
            });
        }

        [Test]
        public void AddAppointment_ShouldAssociateAppointmentWithPregnant()
        {
            // Arrange
            var pregnant = new Pregnant
            {
                Id = 1,
                DueDate = DateTime.Now.AddMonths(1),
                LitterSize = 3
            };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            // Act
            pregnant.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(pregnant.HasAppointments(), "Pregnant pet should have at least one appointment after AddAppointment.");
            Assert.That(appointment == pregnant.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(pregnant == appointment.GetPet(), "Appointment should reference the Pregnant pet after assignment.");
        }

        [Test]
        public void AddAppointment_ShouldAllowMultipleAppointmentsForSamePregnant()
        {
            // Arrange
            var pregnant = new Pregnant
            {
                Id = 1,
                DueDate = DateTime.Now.AddMonths(1),
                LitterSize = 3
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
            pregnant.AddAppointment(appointment1);
            pregnant.AddAppointment(appointment2);

            // Assert
            Assert.That(pregnant.GetAppointments().Count == 2, "Pregnant pet should have exactly two appointments.");
            Assert.Contains(appointment1, pregnant.GetAppointments(), "Should contain the first appointment.");
            Assert.Contains(appointment2, pregnant.GetAppointments(), "Should contain the second appointment.");
        }

        [Test]
        public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
        {
            // Arrange
            var pregnant = new Pregnant { Id = 1, DueDate = DateTime.Now.AddMonths(1), LitterSize = 3 };

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

            pregnant.AddAppointment(appointment1);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => pregnant.AddAppointment(appointment2));
        }

        [Test]
        public void RemoveAppointment_ShouldRemoveExistingAppointment()
        {
            // Arrange
            var pregnant = new Pregnant { Id = 1, DueDate = DateTime.Now.AddMonths(1), LitterSize = 3 };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            pregnant.AddAppointment(appointment);

            // Act
            pregnant.RemoveAppointment(appointment.Id);

            // Assert
            Assert.IsFalse(pregnant.HasAppointments(), "Pregnant pet should have no appointments after removal.");
            Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference the Pregnant pet after removal.");
        }

        [Test]
        public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
        {
            // Arrange
            var pregnant = new Pregnant { Id = 1, DueDate = DateTime.Now.AddMonths(1), LitterSize = 3 };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => pregnant.RemoveAppointment(999));
        }

        [Test]
        public void GetAppointmentById_ShouldReturnCorrectAppointment()
        {
            // Arrange
            var pregnant = new Pregnant { Id = 1, DueDate = DateTime.Now.AddMonths(1), LitterSize = 3 };
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

            pregnant.AddAppointment(appointment1);
            pregnant.AddAppointment(appointment2);

            // Act
            var fetchedAppointment = pregnant.GetAppointmentById(200);

            // Assert
            Assert.That(appointment2 == fetchedAppointment);
        }

        [Test]
        public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
        {
            // Arrange
            var pregnant = new Pregnant { Id = 1, DueDate = DateTime.Now.AddMonths(1), LitterSize = 3 };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            pregnant.AddAppointment(appointment);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => pregnant.GetAppointmentById(999));
        }

        // ----------------------------------------------------------
        // Additional Tests for DueDate & LitterSize Validation
        // ----------------------------------------------------------

        [Test]
        public void DueDate_ShouldThrowInvalidDateException_IfBeforeBirth()
        {
            // Assert
            Assert.Throws<InvalidDateException>(() =>
            {
                // Arrange & Act
                var pregnant = new Pregnant(
                    "Bella",
                    Sex.Female,
                    10.0,
                    new DateTime(2022, 1, 1),
                    new List<Color> { Color.Brown },
                    dueDate: new DateTime(2021, 12, 31), // before birth
                    litterSize: 3
                );
            });
        }

        [Test]
        public void DueDate_ShouldAcceptValidValue()
        {
            // Arrange
            var birthDate = new DateTime(2022, 1, 1);
            var dueDate = birthDate.AddMonths(2); // 2 months after birth

            var pregnant = new Pregnant(
                "Bella",
                Sex.Female,
                10.0,
                birthDate,
                new List<Color> { Color.Brown },
                dueDate,
                litterSize: 4
            );

            // Assert
            Assert.That(pregnant.DueDate, Is.EqualTo(dueDate), "DueDate should match the valid value provided.");
        }

        [Test]
        public void LitterSize_ShouldThrowNegativeValueException_IfZeroOrLess()
        {
            // Assert
            Assert.Throws<NegativeValueException>(() =>
            {
                // Arrange & Act
                var pregnant = new Pregnant(
                    "Molly",
                    Sex.Female,
                    8.0,
                    new DateTime(2022, 1, 1),
                    new List<Color> { Color.Black },
                    dueDate: new DateTime(2022, 8, 1),
                    litterSize: 0 // invalid litter size
                );
            });
        }

        [Test]
        public void LitterSize_ShouldAcceptValidValue()
        {
            // Arrange
            var pregnant = new Pregnant(
                "Molly",
                Sex.Female,
                8.0,
                new DateTime(2022, 1, 1),
                new List<Color> { Color.Black },
                dueDate: new DateTime(2022, 8, 1),
                litterSize: 5
            );

            // Assert
            Assert.That(pregnant.LitterSize, Is.EqualTo(5), "LitterSize should store the valid integer provided.");
        }

}


using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class FishTests
{
    private string _testPathPet, _testPathFish;

    [SetUp]
    public void Setup()
    {
        _testPathFish = "../../../Data/Fish.json";
        _testPathPet = "../../../Data/Pet.json";
        File.Delete(_testPathFish);
        File.Delete(_testPathPet);
    }
        
    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testPathFish))
        {
            File.Delete(_testPathFish);
        }
        if (File.Exists(_testPathPet))
        {
            File.Delete(_testPathPet);
        }
    }

    [Test]
    public void AddToExtent_ShouldAddFishCorrectly()
    {
        // Arrange
        var fish1 = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 3, 15), [Color.Golden], WaterType.Freshwater, 22.5);
        var fish2 = new Fish("Momo", Sex.Male, 0.45, new DateTime(2018, 6, 12), [Color.Black, Color.White], WaterType.Saltwater, 15.0);

        // Act
        var extent = Fish.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Goldie"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=0.2"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2021-03-15"));
        Assert.IsTrue(extent[0].Contains("Colors=(Golden)"));
        Assert.IsTrue(extent[0].Contains("WaterType=Freshwater"));
        Assert.IsTrue(extent[0].Contains("WaterTemperature=22.5"));
        Assert.IsTrue(extent[1].Contains("Id=2"));
        Assert.IsTrue(extent[1].Contains("Name=Momo"));
        Assert.IsTrue(extent[1].Contains("Sex=Male"));
        Assert.IsTrue(extent[1].Contains("Weight=0.45"));
        Assert.IsTrue(extent[1].Contains("WaterType=Saltwater"));
        Assert.IsTrue(extent[1].Contains("WaterTemperature=15"));
        
    }
    
    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 3, 15), [Color.Golden], WaterType.Freshwater, 22.5);

        // Act
        var json = File.ReadAllText(_testPathFish);

        // Assert
        Assert.IsTrue(json.Contains("\"Name\": \"Goldie\""));
        Assert.IsTrue(json.Contains("\"Sex\": 1"));
        Assert.IsTrue(json.Contains("\"Weight\": 0.2"));
        Assert.IsTrue(json.Contains("\"WaterType\": 0"));
        Assert.IsTrue(json.Contains("\"WaterTemperature\": 22.5"));
    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPathFish, "[{\"WaterType\":0,\"WaterTemperature\":22.5,\"Id\":1,\"Name\":\"Goldie\",\"Sex\":1,\"Weight\":0.2,\"DateOfBirth\":\"2021-03-15T00:00:00\",\"Colors\":[5],\"Age\":2}]");

        // Act
        var extent = Fish.GetExtentAsString();

        // Assert
        Assert.IsTrue(extent[0].Contains("Id=1"));
        Assert.IsTrue(extent[0].Contains("Name=Goldie"));
        Assert.IsTrue(extent[0].Contains("Sex=Female"));
        Assert.IsTrue(extent[0].Contains("Weight=0.2"));
        Assert.IsTrue(extent[0].Contains("DateOfBirth=2021-03-15"));
        Assert.IsTrue(extent[0].Contains("Colors=(Red)"));
        Assert.IsTrue(extent[0].Contains("WaterType=Freshwater"));
        Assert.IsTrue(extent[0].Contains("WaterTemperature=22.5"));
    }

    [Test]
    public void Age_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, 22.5);

        // Act
        int age = fish.Age;

        // Assert
        Assert.That(age, Is.EqualTo(DateTime.Now.Year - 2021));
    }

    [Test]
    public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var fish = new Fish("", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, -0.2, new DateTime(2021, 1, 1), [Color.Golden], WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, DateTime.Now.AddDays(1), [Color.Golden], WaterType.Freshwater, 22.5);
        });
    }

    [Test]
    public void WaterTemperature_ShouldThrowANegativeValueException_ForNegativeWaterTemperature()
    {
        // Act & Assert
        Assert.Throws<NegativeValueException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden],  WaterType.Freshwater, -5.0);
        });
    }
    
    [Test]
    public void Color_ShouldThrowAnEmptyListException()
    {
        // Act & Assert
        Assert.Throws<EmptyListException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [], WaterType.Freshwater, -5.0);
        });
    }
        
    [Test]
    public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
    {
        // Act & Assert
        Assert.Throws<DuplicatesException>(() => 
        {
            // Arrange
            var fish = new Fish("Goldie", Sex.Female, 0.2, new DateTime(2021, 1, 1), [Color.Golden, Color.Golden], WaterType.Freshwater, -5.0);
        });
    }
    
    [Test]
        public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var fish = new Fish(
                    "Guppy",
                    Sex.Female,
                    0.05,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    WaterType.Freshwater,
                    26.5
                );

                // Act
                fish.AddSpecie(null!);  // Should throw NullReferenceException
            });
        }

        [Test]
        public void AddSpecie_ShouldAddSpecieCorrectly()
        {
            // Arrange
            var fish = new Fish(
                "Guppy",
                Sex.Female,
                0.05,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                WaterType.Freshwater,
                26.5
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            fish.AddSpecie(cat);

            // Assert
            Assert.That(fish.GetSpecie()!.Equals(cat));
            Assert.That(cat.GetPets().Contains(fish));
        }

        [Test]
        public void RemoveSpecie_ShouldRemoveSpecieCorrectly()
        {
            // Arrange
            var fish = new Fish(
                "Nemo",
                Sex.Male,
                0.3,
                DateTime.Now.AddDays(-20),
                new List<Color> { Color.Yellow, Color.White },
                WaterType.Saltwater,
                24.0
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            fish.AddSpecie(cat);
            fish.RemoveSpecie();

            // Assert
            Assert.IsNull(fish.GetSpecie(), "Specie should be null after removal.");
        }

        [Test]
        public void RemoveSpecie_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var fish = new Fish(
                    "Nemo",
                    Sex.Male,
                    0.3,
                    DateTime.Now.AddDays(-20),
                    new List<Color> { Color.Yellow, Color.White },
                    WaterType.Saltwater,
                    24.0
                );

                // Attempting to remove a specie when none is assigned
                fish.RemoveSpecie();
            });
        }

        [Test]
        public void ModifyClient_ShouldAddClientCorrectly()
        {
            // Arrange & Act
            var fish = new Fish(
                "Goldie",
                Sex.Female,
                0.2,
                new DateTime(2022, 1, 1),
                new List<Color> { Color.Gray },
                WaterType.Freshwater,
                25.0
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { fish }
            );

            // Assert
            Assert.That(fish.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(fish));
        }

        [Test]
        public void Fish_ShouldAddClientCorrectly_AssignToDummyClient()
        {
            // Arrange & Act
            var fish = new Fish(
                "Bubbles",
                Sex.Male,
                0.1,
                DateTime.Now.AddMonths(-6),
                new List<Color> { Color.Blue },
                WaterType.Freshwater,
                23.5
            );

            // If your base Pet constructor assigns a dummy client, just assert non-null
            Assert.That(fish.GetClient() != null);
        }

        [Test]
        public void ModifyClient_ShouldModifyClientCorrectly()
        {
            // Arrange
            var fish1 = new Fish(
                "Bubble1",
                Sex.Male,
                0.2,
                new DateTime(2023, 1, 1),
                new List<Color> { Color.Blue },
                WaterType.Freshwater,
                23.0
            );

            var fish2 = new Fish(
                "Bubble2",
                Sex.Male,
                0.2,
                new DateTime(2023, 1, 1),
                new List<Color> { Color.Blue },
                WaterType.Freshwater,
                24.0
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { fish1 }
            );

            // Act
            fish2.ModifyClient(client);

            // Assert
            Assert.That(fish2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(fish2));
            Assert.That(fish1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(fish1));
        }

        [Test]
        public void ModifyClient_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var fish = new Fish(
                    "Bubbles",
                    Sex.Male,
                    0.1,
                    DateTime.Now.AddMonths(-6),
                    new List<Color> { Color.Blue },
                    WaterType.Freshwater,
                    23.5
                );

                fish.ModifyClient(null!);
            });
        }

        [Test]
        public void Fish_ShouldThrowANullReferenceException_ForNullClient()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var fish = new Fish(
                    "Marlin",
                    Sex.Male,
                    0.25,
                    new DateTime(2022, 8, 1),
                    new List<Color> { Color.Blue },
                    client: null!,   // explicitly passing null
                    waterType: WaterType.Saltwater,
                    waterTemperature: 25.0
                );
            });
        }

        [Test]
        public void Fish_ShouldAddClientCorrectly_WithConstructor()
        {
            // Arrange
            var fish1 = new Fish(
                "Pearl",
                Sex.Female,
                0.3,
                new DateTime(2021, 10, 1),
                new List<Color> { Color.Gray },
                WaterType.Saltwater,
                26.0
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { fish1 }
            );

            //Act
            var fish2 = new Fish(
                "Dory",
                Sex.Female,
                0.2,
                new DateTime(2021, 10, 1),
                new List<Color> { Color.Blue, Color.Yellow },
                client,
                waterType: WaterType.Saltwater,
                waterTemperature: 24.5
            );

            //Assert
            Assert.That(fish2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(fish2));
            Assert.That(fish1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(fish1));
        }

        [Test]
        public void RemovePet_ShouldRemoveFishCorrectly()
        {
            // Arrange
            var fish1 = new Fish(
                "Nemo",
                Sex.Male,
                0.2,
                new DateTime(2022, 1, 1),
                new List<Color> { Color.Blue, Color.White },
                WaterType.Saltwater,
                24.0
            );

            var fish2 = new Fish(
                "Dory",
                Sex.Female,
                0.25,
                new DateTime(2022, 2, 1),
                new List<Color> { Color.Blue, Color.Yellow },
                WaterType.Saltwater,
                25.0
            );

            var cat = new Specie("Cat", "Felis catus");
            cat.AddPet(fish1);
            cat.AddPet(fish2);

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { fish1, fish2 }
            );

            //Act
            fish1.RemovePet();  // inherited from Pet

            //Assert
            Assert.That(!Fish.GetCurrentExtent().Contains(fish1), "Removed fish1 should not be in Fish’s extent anymore.");
            Assert.That(!client.GetPets().Contains(fish1), "Client should no longer reference fish1 after removal.");
            Assert.That(fish1.GetSpecie() == null, "fish1’s assigned specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(fish1), "Specie should no longer reference fish1.");
            Assert.That(fish1.GetClient() != client, "fish1’s client reference should be cleared after removal.");
        }

        [Test]
        public void RemovePet_ShouldThrowANotFoundException()
        {
            // Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange & Act
                var fish1 = new Fish(
                    "Piołun",
                    Sex.Male,
                    0.3,
                    new DateTime(2019, 5, 12),
                    new List<Color> { Color.Black, Color.White },
                    WaterType.Freshwater,
                    22.0
                );

                // First removal is valid
                fish1.RemovePet();

                // Second removal should throw NotFoundException
                fish1.RemovePet();
            });
        }

        [Test]
        public void AddAppointment_ShouldAssociateAppointmentWithFish()
        {
            // Arrange
            var fish = new Fish
            {
                Id = 1,
                WaterType = WaterType.Freshwater,
                WaterTemperature = 23.0
            };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            // Act
            fish.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(fish.HasAppointments(), "Fish should have at least one appointment after AddAppointment.");
            Assert.That(appointment == fish.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(fish == appointment.GetPet(), "Appointment should reference the Fish after assignment.");
        }

        [Test]
        public void AddAppointment_ShouldAllowMultipleAppointmentsForSameFish()
        {
            // Arrange
            var fish = new Fish
            {
                Id = 1,
                WaterType = WaterType.Brackish,
                WaterTemperature = 22.5
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
            fish.AddAppointment(appointment1);
            fish.AddAppointment(appointment2);

            // Assert
            Assert.That(fish.GetAppointments().Count == 2, "Fish should have exactly two appointments.");
            Assert.Contains(appointment1, fish.GetAppointments(), "Fish should contain the first appointment.");
            Assert.Contains(appointment2, fish.GetAppointments(), "Fish should contain the second appointment.");
        }

        [Test]
        public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
        {
            // Arrange
            var fish = new Fish { Id = 1, WaterType = WaterType.Freshwater, WaterTemperature = 25.0 };

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

            fish.AddAppointment(appointment1);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => fish.AddAppointment(appointment2));
        }

        [Test]
        public void RemoveAppointment_ShouldRemoveExistingAppointment()
        {
            // Arrange
            var fish = new Fish { Id = 1, WaterType = WaterType.Freshwater, WaterTemperature = 25.0 };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            fish.AddAppointment(appointment);

            // Act
            fish.RemoveAppointment(appointment.Id);

            // Assert
            Assert.IsFalse(fish.HasAppointments(), "Fish should have no appointments after removal.");
            Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference Fish after removal.");
        }

        [Test]
        public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
        {
            // Arrange
            var fish = new Fish { Id = 1, WaterType = WaterType.Brackish, WaterTemperature = 24.0 };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => fish.RemoveAppointment(999));
        }

        [Test]
        public void GetAppointmentById_ShouldReturnCorrectAppointment()
        {
            // Arrange
            var fish = new Fish { Id = 1, WaterType = WaterType.Freshwater, WaterTemperature = 25.0 };
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

            fish.AddAppointment(appointment1);
            fish.AddAppointment(appointment2);

            // Act
            var fetchedAppointment = fish.GetAppointmentById(200);

            // Assert
            Assert.That(appointment2 == fetchedAppointment);
        }

        [Test]
        public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
        {
            // Arrange
            var fish = new Fish { Id = 1, WaterType = WaterType.Freshwater, WaterTemperature = 25.0 };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            fish.AddAppointment(appointment);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => fish.GetAppointmentById(999));
        }
    
}

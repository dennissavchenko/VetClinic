using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class BirdTests
    {
        private string _testPathPet, _testPathBird;

        [SetUp]
        public void Setup()
        {
            _testPathBird = "../../../Data/Bird.json";
            _testPathPet = "../../../Data/Pet.json";
            File.Delete(_testPathBird);
            File.Delete(_testPathPet);
        }
        
        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_testPathBird))
            {
                File.Delete(_testPathBird);
            }
            if (File.Exists(_testPathPet))
            {
                File.Delete(_testPathPet);
            }
        }

        [Test]
        public void AddToExtent_ShouldAddBirdCorrectly()
        {
            // Arrange
            var bird1 = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2018, 5, 1), [Color.Brown], 10, true);
            var bird2 = new Bird("Momo", Sex.Male, 0.45, new DateTime(2018, 6, 12), [Color.Black, Color.White], 50, false);

            // Act
            var extent = Bird.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Name=Tweety"));
            Assert.IsTrue(extent[0].Contains("Sex=Female"));
            Assert.IsTrue(extent[0].Contains("Weight=0.5"));
            Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
            Assert.IsTrue(extent[0].Contains("Colors=(Brown)"));
            Assert.IsTrue(extent[0].Contains("WingsSpan=10"));
            Assert.IsTrue(extent[0].Contains("CanFly=True"));
            Assert.IsTrue(extent[1].Contains("Id=2"));
            Assert.IsTrue(extent[1].Contains("Name=Momo"));
            Assert.IsTrue(extent[1].Contains("Sex=Male"));
            Assert.IsTrue(extent[1].Contains("Weight=0.45"));
            Assert.IsTrue(extent[1].Contains("WingsSpan=50"));
            Assert.IsTrue(extent[1].Contains("CanFly=False"));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var bird = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2018, 5, 1), [Color.Brown], 10, true);
            // Act
            var json = File.ReadAllText(_testPathBird);

            // Assert
            Assert.IsTrue(json.Contains("\"Name\": \"Tweety\""));
            Assert.IsTrue(json.Contains("\"Sex\": 1"));
            Assert.IsTrue(json.Contains("\"Weight\": 0.5"));
            Assert.IsTrue(json.Contains("\"WingsSpan\": 10"));
            Assert.IsTrue(json.Contains("\"CanFly\": true"));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPathBird, "[{\"WingsSpan\":10,\"CanFly\":true,\"Id\":11,\"Name\":\"Tweety\",\"Sex\":1,\"Weight\":0.5,\"DateOfBirth\":\"2020-04-10T00:00:00\",\"Colors\":[8],\"Age\":4}]");

            // Act
            var extent = Bird.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Name=Tweety"));
            Assert.IsTrue(extent[0].Contains("Sex=Female"));
            Assert.IsTrue(extent[0].Contains("Weight=0.5"));
            Assert.IsTrue(extent[0].Contains("DateOfBirth=2020-04-10"));
            Assert.IsTrue(extent[0].Contains("Colors=(Yellow)"));
            Assert.IsTrue(extent[0].Contains("WingsSpan=10"));
            Assert.IsTrue(extent[0].Contains("CanFly=True"));
        }

        [Test]
        public void Age_ShouldBeCalculatedCorrectly()
        {
            // Arrange
            var pet = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow], 10, true);

            // Act
            int age = pet.Age;

            // Assert
            Assert.That(age,  Is.EqualTo(DateTime.Now.Year - 2020));
        }

        [Test]
        public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
        {
            // Act & Assert
            Assert.Throws<EmptyStringException>(() =>
            {
                // Arrange
                var bird = new Bird("", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow], 10, true);
            });
        }
        
        [Test]
        public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
        {
            // Act & Assert
            Assert.Throws<NegativeValueException>(() => 
            {
                // Arrange
                var bird = new Bird("Tweety", Sex.Female, -0.5, new DateTime(2020, 1, 1), [Color.Yellow], 10, true);
            });
        }
        
        [Test]
        public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
        {
            // Act & Assert
            Assert.Throws<InvalidDateException>(() => 
            {
                // Arrange
                var bird = new Bird("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.Yellow],  10, true);
            });
        }
        
        [Test]
        public void WingsSpan_ShouldThrowANegativeValueException_ForNegativeWingsSpan()
        {
            // Act & Assert
            Assert.Throws<NegativeValueException>(() => 
            {
                // Arrange
                var bird = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow], -10, true);
            });
        }
        
        [Test]
        public void Color_ShouldThrowAnEmptyListException()
        {
            // Act & Assert
            Assert.Throws<EmptyListException>(() => 
            {
                // Arrange
                var bird = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [], 10, true);
            });
        }
        
        [Test]
        public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
        {
            // Act & Assert
            Assert.Throws<DuplicatesException>(() => 
            {
                // Arrange
                var bird = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow, Color.Yellow], 10, true);
            });
        }
        
        [Test]
        public void AddSpecie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var bird = new Bird(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    wingsSpan: 10.0,
                    canFly: true
                );

                // Act
                bird.AddSpecie(null!);  // Should throw NullReferenceException
            });
        }

        [Test]
        public void AddSpecie_ShouldAddSpecieCorrectly()
        {
            // Arrange
            var bird = new Bird(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                wingsSpan: 10.0,
                canFly: true
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            bird.AddSpecie(cat);

            // Assert
            Assert.That(bird.GetSpecie()!.Equals(cat));
            Assert.That(cat.GetPets().Contains(bird));
        }

        [Test]
        public void RemoveSpecie_ShouldRemoveSpecieCorrectly()
        {
            // Arrange
            var bird = new Bird(
                "Tweety",
                Sex.Female,
                0.5,
                DateTime.Now.AddDays(-10),
                new List<Color> { Color.Black },
                wingsSpan: 10.0,
                canFly: true
            );
            var cat = new Specie("Cat", "Felis catus");

            // Act
            bird.AddSpecie(cat);
            bird.RemoveSpecie();

            // Assert
            Assert.IsNull(bird.GetSpecie(), "Specie should be null after removal.");
        }

        [Test]
        public void RemoveSpecie_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var bird = new Bird(
                    "Tweety",
                    Sex.Female,
                    0.5,
                    DateTime.Now.AddDays(-10),
                    new List<Color> { Color.Black },
                    wingsSpan: 10.0,
                    canFly: true
                );

                // Attempting to remove a specie when none is assigned
                bird.RemoveSpecie();
            });
        }

        [Test]
        public void ModifyClient_ShouldAddClientCorrectly()
        {
            // Arrange & Act
            var bird = new Bird(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                wingsSpan: 20.0,
                canFly: true
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { bird }  // Bird inherits Pet
            );

            // Assert
            Assert.That(bird.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(bird));
        }

        [Test]
        public void Bird_ShouldAddClientCorrectly_AssignToDummyClient()
        {
            // Arrange & Act
            // Using the constructor that does NOT take a client:
            var bird = new Bird(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                wingsSpan: 20.0,
                canFly: true
            );

            // By default, your Bird/Pet constructor might assign a dummy client 
            // or a non-null client object internally.

            // Assert
            Assert.That(bird.GetClient() != null);
        }

        [Test]
        public void ModifyClient_ShouldModifyClientCorrectly()
        {
            // Arrange
            var bird1 = new Bird(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                wingsSpan: 20.0,
                canFly: true
            );

            var bird2 = new Bird(
                "Buddy1",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                wingsSpan: 22.0,
                canFly: false
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { bird1 }
            );

            // Act
            bird2.ModifyClient(client);

            // Assert
            Assert.That(bird2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(bird2));
            Assert.That(bird1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(bird1));
        }

        [Test]
        public void ModifyClient_ShouldThrowANullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var bird = new Bird(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    wingsSpan: 20.0,
                    canFly: true
                );

                bird.ModifyClient(null!);
            });
        }

        [Test]
        public void Bird_ShouldThrowANullReferenceException_ForNullClient()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange & Act
                var bird = new Bird(
                    "Buddy",
                    Sex.Male,
                    15.5,
                    new DateTime(2018, 5, 1),
                    new List<Color> { Color.Brown },
                    client: null!,    // explicitly passing null
                    wingsSpan: 20.0,
                    canFly: true
                );
            });
        }

        [Test]
        public void Bird_ShouldAddClientCorrectly_WithConstructor()
        {
            // Arrange
            var bird1 = new Bird(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                wingsSpan: 20.0,
                canFly: true
            );

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { bird1 }
            );

            //Act
            var bird2 = new Bird(
                "Buddy1",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                client,
                wingsSpan: 25.0,
                canFly: false
            );

            //Assert
            Assert.That(bird2.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(bird2));
            Assert.That(bird1.GetClient().Equals(client));
            Assert.That(client.GetPets().Contains(bird1));
        }

        [Test]
        public void RemovePet_ShouldRemoveBirdCorrectly()
        {
            // Arrange
            var bird1 = new Bird(
                "Buddy",
                Sex.Male,
                15.5,
                new DateTime(2018, 5, 1),
                new List<Color> { Color.Brown },
                wingsSpan: 20.0,
                canFly: true
            );
            var bird2 = new Bird(
                "Momo",
                Sex.Male,
                10.0,
                new DateTime(2018, 6, 12),
                new List<Color> { Color.Black, Color.White },
                wingsSpan: 15.0,
                canFly: true
            );

            var cat = new Specie("Cat", "Felis catus");

            // Since Bird inherits Pet, AddPet calls are valid
            cat.AddPet(bird1);
            cat.AddPet(bird2);

            var client = new Client(
                "Marek",
                "Kowalski",
                "828222222",
                "kw@gmail.com",
                new List<Pet> { bird1, bird2 }
            );

            //Act
            bird1.RemovePet();  // Inherited from Pet

            //Assert
            Assert.That(!Bird.GetCurrentExtent().Contains(bird1), "Removed bird1 should not be in Bird’s extent anymore.");
            Assert.That(!client.GetPets().Contains(bird1), "Client should no longer reference bird1 after removal.");
            Assert.That(bird1.GetSpecie() == null, "Bird1’s assigned specie should be null after removal.");
            Assert.That(!cat.GetPets().Contains(bird1), "Specie should no longer reference bird1.");
            Assert.That(bird1.GetClient() != client, "Bird1’s client reference should be cleared after removal.");
        }

        [Test]
        public void RemovePet_ShouldThrowANotFoundException()
        {
            // Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange & Act
                var bird1 = new Bird(
                    "Piołun",
                    Sex.Male,
                    5.2,
                    new DateTime(2019, 5, 12),
                    new List<Color> { Color.Black, Color.White },
                    wingsSpan: 8.0,
                    canFly: true
                );

                // First removal is valid
                bird1.RemovePet();

                // Second removal should throw NotFoundException (already removed from extent)
                bird1.RemovePet();
            });
        }

        [Test]
        public void AddAppointment_ShouldAssociateAppointmentWithBird()
        {
            // Arrange
            var bird = new Bird
            {
                Id = 1,
                WingsSpan = 5.0,
                CanFly = true
            };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            // Act
            bird.AddAppointment(appointment);

            // Assert
            Assert.IsTrue(bird.HasAppointments(), "Bird should have at least one appointment after AddAppointment.");
            Assert.That(appointment == bird.GetAppointmentById(100), "The added appointment should be retrievable by its ID.");
            Assert.That(bird == appointment.GetPet(), "Appointment should reference the Bird after assignment.");
        }

        [Test]
        public void AddAppointment_ShouldAllowMultipleAppointmentsForSameBird()
        {
            // Arrange
            var bird = new Bird
            {
                Id = 1,
                WingsSpan = 5.0,
                CanFly = true
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
            bird.AddAppointment(appointment1);
            bird.AddAppointment(appointment2);

            // Assert
            Assert.That(bird.GetAppointments().Count == 2, "Bird should have exactly two appointments.");
            Assert.Contains(appointment1, bird.GetAppointments(), "Bird should contain the first appointment.");
            Assert.Contains(appointment2, bird.GetAppointments(), "Bird should contain the second appointment.");
        }

        [Test]
        public void AddAppointment_ShouldThrowDuplicatesExceptionForSameAppointmentId()
        {
            // Arrange
            var bird = new Bird { Id = 1, WingsSpan = 5.0, CanFly = false };

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

            bird.AddAppointment(appointment1);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => bird.AddAppointment(appointment2));
        }

        [Test]
        public void RemoveAppointment_ShouldRemoveExistingAppointment()
        {
            // Arrange
            var bird = new Bird { Id = 1, WingsSpan = 5.0, CanFly = true };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now.AddDays(1),
                State = AppointmentState.Scheduled,
                Price = 150
            };

            bird.AddAppointment(appointment);

            // Act
            bird.RemoveAppointment(appointment.Id);

            // Assert
            Assert.IsFalse(bird.HasAppointments(), "Bird should have no appointments after removal.");
            Assert.IsNull(appointment.GetPet(), "Appointment should no longer reference Bird after removal.");
        }

        [Test]
        public void RemoveAppointment_ShouldThrowNotFoundExceptionForNonExistentId()
        {
            // Arrange
            var bird = new Bird { Id = 1, WingsSpan = 5.0, CanFly = false };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => bird.RemoveAppointment(999));
        }

        [Test]
        public void GetAppointmentById_ShouldReturnCorrectAppointment()
        {
            // Arrange
            var bird = new Bird { Id = 1, WingsSpan = 5.0, CanFly = true };
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

            bird.AddAppointment(appointment1);
            bird.AddAppointment(appointment2);

            // Act
            var fetchedAppointment = bird.GetAppointmentById(200);

            // Assert
            Assert.That(appointment2 == fetchedAppointment);
        }

        [Test]
        public void GetAppointmentById_ShouldThrowNotFoundExceptionForNonExistentAppointment()
        {
            // Arrange
            var bird = new Bird { Id = 1, WingsSpan = 5.0, CanFly = true };
            var appointment = new Appointment
            {
                Id = 100,
                DateTime = DateTime.Now,
                State = AppointmentState.Scheduled,
                Price = 100
            };
            bird.AddAppointment(appointment);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => bird.GetAppointmentById(999));
        }
        
    }


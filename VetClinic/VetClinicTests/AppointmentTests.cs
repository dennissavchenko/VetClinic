using VetClinic;

namespace VetClinicTests;

public class AppointmentTests
    {
        private string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = "../../../Data/Appointment.json";
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
        public void AddToExtent_ShouldAddAppointmentCorrectly()
        {
            // Arrange
            var appointment1 = new Appointment(new DateTime(2018, 5, 1, 16, 30, 00), AppointmentState.Scheduled, 150);
            var appointment2 = new Appointment(new DateTime(2018, 5, 2, 16, 00, 00), AppointmentState.Canceled, 200);

            // Act
            var extent = Appointment.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("DateTime=2018-05-01T16:30:00"));
            Assert.That(extent[0].Contains("State=Scheduled"));
            Assert.That(extent[0].Contains("Price=150"));
            Assert.That(extent[1].Contains("Id=2"));
            Assert.That(extent[1].Contains("DateTime=2018-05-02T16:00:00"));
            Assert.That(extent[1].Contains("State=Canceled"));
            Assert.That(extent[1].Contains("Price=200"));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), AppointmentState.Canceled, 150);

            // Act
            var json = File.ReadAllText(_testPath);

            // Assert
            Assert.IsTrue(json.Contains("\"DateTime\": \"2018-05-01T14:30:00"));
            Assert.IsTrue(json.Contains("\"State\": 3"));
            Assert.IsTrue(json.Contains("\"Price\": 150"));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPath, "[{ \"Id\": 1, \"DateTime\": \"2018-05-01T14:30:00\", \"State\": 3, \"Price\": 150 }]");

            // Act
            var extent = Appointment.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("DateTime=2018-05-01T14:30:00"));
            Assert.That(extent[0].Contains("State=Canceled"));
            Assert.That(extent[0].Contains("Price=150"));
        }

        [Test]
        public void Price_ShouldThrowArgumentOutOfRangeException_ForZeroOrNegativePrice()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // Arrange
                var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), AppointmentState.Scheduled, -50);
            });
        }
        
        [Test]
        public void AssignPet_ShouldAssignPetToAnAppointmentCorrectly()
        {
            // Arrange
            var pet1 = new Pet { Id = 1 };
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // Act
            appointment.AssignPet(pet1);
            
            // Assert
            Assert.That(pet1.HasAppointments);
            Assert.That(pet1.GetAppointments().Contains(appointment));
            Assert.That(appointment.GetPet() == pet1);
        }
        
        [Test]
        public void AssignPet_ShouldThrowInvalidOperationExceptionIfAppointmentAlreadyHasDifferentPet()
        {
            // Arrange
            var pet1 = new Pet { Id = 1 };
            var pet2 = new Pet { Id = 2 };
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // pet1 has the appointment
            appointment.AssignPet(pet1);

            // Act & Assert: Trying to add the same appointment to pet2
            Assert.Throws<InvalidOperationException>(() => appointment.AssignPet(pet2));
        }
        
        [Test]
        public void AssignPet_ShouldThrowNullReferenceExceptionIfPetIsNull()
        {
            // Arrange
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => appointment.AssignPet(null!));
        }

        [Test]
        public void RemovePet_ShouldUnassignPetFromAppointment()
        {
            // Arrange
            var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
            var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), AppointmentState.Canceled, 150);
            appointment.AssignPet(pet1);

            // Act
            appointment.RemovePet();

            // Assert
            Assert.IsFalse(pet1.HasAppointments(), "Pet should have no appointments after appointment.RemovePet().");
            Assert.IsNull(appointment.GetPet(), "Appointment.GetPet() should be null after removal.");
        }
        
        [Test]
        public void RemovePet_ShouldThrowInvalidOperationException_AppointmentHasNoPet()
        {
            // Arrange
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => appointment.RemovePet());
        }
        
    }
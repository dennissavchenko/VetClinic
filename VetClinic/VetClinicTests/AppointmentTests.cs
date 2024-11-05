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
            var appointmentDate = new DateTime(2018, 5, 1);
            var appointment = new Appointment(appointmentDate, 150);

            // Act
            var extent = Appointment.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].DateTime, Is.EqualTo(appointmentDate));
            Assert.That(extent[0].Price, Is.EqualTo(150));
        }

        [Test]
        public void AddToExtent_ShouldAssignIdCorrectly()
        {
            // Arrange
            var appointment1 = new Appointment(new DateTime(2018, 5, 1, 10, 0, 0), 100);
            var appointment2 = new Appointment(new DateTime(2018, 5, 2, 15, 0, 0), 200);

            // Act
            var extent = Appointment.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(2));
            Assert.That(extent[0].Id, Is.EqualTo(1));
            Assert.That(extent[1].Id, Is.EqualTo(2));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), 150);

            // Act
            var json = File.ReadAllText(_testPath);

            // Assert
            Assert.IsTrue(json.Contains("\"DateTime\": \"2018-05-01T14:30:00"));
            Assert.IsTrue(json.Contains("\"Price\": 150"));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPath, "[{ \"Id\": 1, \"DateTime\": \"2018-05-01T14:30:00\", \"Price\": 150 }]");

            // Act
            var extent = Appointment.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].DateTime, Is.EqualTo(new DateTime(2018, 5, 1, 14, 30, 0)));
            Assert.That(extent[0].Price, Is.EqualTo(150));
        }

        [Test]
        public void Price_ShouldThrowArgumentOutOfRangeException_ForZeroOrNegativePrice()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // Arrange
                var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), -50);
            });
        }
    }
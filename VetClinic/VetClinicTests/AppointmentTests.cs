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
            var appointment1 = new Appointment(new DateTime(2018, 5, 1, 16, 30, 00), 150);
            var appointment2 = new Appointment(new DateTime(2018, 5, 2, 16, 00, 00), 200);

            // Act
            var extent = Appointment.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("DateTime=2018-05-01T16:30:00"));
            Assert.That(extent[0].Contains("Price=150"));
            Assert.That(extent[1].Contains("Id=2"));
            Assert.That(extent[1].Contains("DateTime=2018-05-02T16:00:00"));
            Assert.That(extent[1].Contains("Price=200"));
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
            var extent = Appointment.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("DateTime=2018-05-01T14:30:00"));
            Assert.That(extent[0].Contains("Price=150"));
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
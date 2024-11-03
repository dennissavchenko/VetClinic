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
            var bird = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2018, 5, 1), [Color.Brown], 10, true);

            // Act
            var extent = Bird.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].Name, Is.EqualTo("Tweety"));
            Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
            Assert.That(extent[0].Weight, Is.EqualTo(0.5));
            Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2018, 5, 1)));
            Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.Brown }));
            Assert.That(extent[0].WingsSpan, Is.EqualTo(10));
            Assert.That(extent[0].CanFly, Is.EqualTo(true));
        }

        [Test]
        public void AddToExtent_ShouldAssignIdCorrectly()
        {
            // Arrange
            var bird1 = new Bird("Luna", Sex.Female, 0.1, new DateTime(2020, 6, 10), [Color.Gray], 15, true);
            var bird2 = new Bird("Simba", Sex.Male, 0.2, new DateTime(2019, 8, 15), [Color.Yellow], 12, true);

            // Act
            var extent = Bird.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(2));
            Assert.That(extent[0].Id, Is.EqualTo(1));
            Assert.That(extent[1].Id, Is.EqualTo(2));
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
            var extent = Bird.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].Name, Is.EqualTo("Tweety"));
            Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
            Assert.That(extent[0].Weight, Is.EqualTo(0.5));
            Assert.That(extent[0].DateOfBirth, Is.EqualTo(new DateTime(2020, 4, 10)));
            Assert.That(extent[0].Colors, Is.EqualTo(new List<Color> { Color.Yellow }));
            Assert.That(extent[0].WingsSpan, Is.EqualTo(10));
            Assert.That(extent[0].CanFly, Is.EqualTo(true));
        }

        [Test]
        public void Age_ShouldBeCalculatedCorrectly()
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow]);

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
                var bird = new Bird("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.Yellow], 10, true);
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
        
    }

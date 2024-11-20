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
        
    }

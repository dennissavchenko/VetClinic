using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class PetTests
    {
        private string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = "../../../Data/Pet.json";
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
        public void AddToExtent_ShouldAddPetCorrectly()
        {
            // Arrange
            var pet = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);

            // Act
            var extent = Pet.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].Name, Is.EqualTo("Buddy"));
            Assert.That(extent[0].Sex, Is.EqualTo(Sex.Male));
            Assert.That(extent[0].Weight, Is.EqualTo(15.5));
        }

        [Test]
        public void AddToExtent_ShouldAssignIdCorrectly()
        {
            // Arrange
            var pet1 = new Pet("Luna", Sex.Female, 4.5, new DateTime(2020, 6, 10), [Color.Gray]);
            var pet2 = new Pet("Simba", Sex.Male, 6.2, new DateTime(2019, 8, 15), [Color.Yellow]);

            // Act
            var extent = Pet.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(2));
            Assert.That(extent[0].Id, Is.EqualTo(1));
            Assert.That(extent[1].Id, Is.EqualTo(2));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var pet = new Pet("Rex", Sex.Male, 22.0, new DateTime(2017, 3, 20), [Color.Black, Color.White ]);

            // Act
            var json = File.ReadAllText(_testPath);

            // Assert
            Assert.IsTrue(json.Contains("\"Name\": \"Rex\""));
            Assert.IsTrue(json.Contains("\"Sex\": 0"));
            Assert.IsTrue(json.Contains("\"Weight\": 22"));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            var pet = new Pet("Bella", Sex.Female, 10.0, new DateTime(2019, 12, 15), [Color.Golden]);
            typeof(Pet).GetField("_extent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, new List<Pet>());

            // Act
            var extent = Pet.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].Name, Is.EqualTo("Bella"));
            Assert.That(extent[0].Sex, Is.EqualTo(Sex.Female));
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
                var pet = new Pet("", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow]);
            });
        }
        
        [Test]
        public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
        {
            // Act & Assert
            Assert.Throws<NegativeValueException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, -0.5, new DateTime(2020, 1, 1), [Color.Yellow]);
            });
        }
        
        [Test]
        public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
        {
            // Act & Assert
            Assert.Throws<InvalidDateException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.Yellow]);
            });
        }
        
    }

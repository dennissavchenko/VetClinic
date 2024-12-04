using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class PetTests
    {
        private string _testPath;
        private Specie _dog;

        [SetUp]
        public void Setup()
        {
            _testPath = "../../../Data/Pet.json";
            _dog = new Specie("Dog", "Canis lupus familiaris");
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
            Specie cat = new Specie("Cat", "Felis catus");
            // Arrange
            var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown], _dog);
            var pat2 = new Pet("Momo", Sex.Male, 10, new DateTime(2018, 6, 12), [Color.Black, Color.White], cat);

            // Act
            var extent = Pet.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Name=Buddy"));
            Assert.IsTrue(extent[0].Contains("Sex=Male"));
            Assert.IsTrue(extent[0].Contains("Weight=15.5"));
            Assert.IsTrue(extent[0].Contains("DateOfBirth=2018-05-01"));
            Assert.IsTrue(extent[0].Contains("Colors=(Brown)"));
            Assert.IsTrue(extent[1].Contains("Id=2"));
            Assert.IsTrue(extent[1].Contains("Name=Momo"));
            Assert.IsTrue(extent[1].Contains("Sex=Male"));
            Assert.IsTrue(extent[1].Contains("Weight=10"));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var pet = new Pet("Rex", Sex.Male, 22.0, new DateTime(2017, 3, 20), [Color.Black, Color.White], _dog);

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
            File.WriteAllText(_testPath, "[{\"Id\":1,\"Name\":\"Bella\",\"Sex\":1,\"Weight\":5.5,\"DateOfBirth\":\"2020-05-01T00:00:00\",\"Colors\":[9]}]");

            // Act
            var extent = Pet.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Name=Bella"));
            Assert.IsTrue(extent[0].Contains("Sex=Female"));
            Assert.IsTrue(extent[0].Contains("Weight=5.5"));
        }

        [Test]
        public void Age_ShouldBeCalculatedCorrectly()
        {
            // Arrange
            var pet = new Pet("Tweety", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow], _dog);

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
                var pet = new Pet("", Sex.Female, 0.5, new DateTime(2020, 1, 1), [Color.Yellow], _dog);
            });
        }
        
        [Test]
        public void Weight_ShouldThrowANegativeValueException_ForNegativeWeight()
        {
            // Act & Assert
            Assert.Throws<NegativeValueException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, -0.5, new DateTime(2020, 1, 1), [Color.Yellow], _dog);
            });
        }
        
        [Test]
        public void DateOfBirth_ShouldThrowAnInvalidDateException_ForFutureDates()
        {
            // Act & Assert
            Assert.Throws<InvalidDateException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(1), [Color.Yellow], _dog);
            });
        }
        
        [Test]
        public void Color_ShouldThrowAnEmptyListException()
        {
            // Act & Assert
            Assert.Throws<EmptyListException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [], _dog);
            });
        }
        
        [Test]
        public void Color_ShouldThrowADuplicateException_DuplicatesInListDetected()
        {
            // Act & Assert
            Assert.Throws<DuplicatesException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black, Color.Black], _dog);
            });
        }
        
        [Test]
        public void Specie_ShouldThrowANullReferenceException_SpecieIsNull()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => 
            {
                // Arrange
                var pet = new Pet("Tweety", Sex.Female, 0.5, DateTime.Now.AddDays(-10), [Color.Black], null);
            });
        }
        
    }

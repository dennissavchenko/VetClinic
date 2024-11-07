using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class SpecieTests
    {
        private string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = "../../../Data/Specie.json";
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
        public void AddToExtent_ShouldAddSpecieCorrectly()
        {
            // Arrange
            Specie dog = new Specie("Dog", "Canis lupus familiaris");
            Specie cat = new Specie("Cat", "Felis catus");

            // Act
            var extent = Specie.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("Name=Dog"));
            Assert.That(extent[0].Contains("Description=Canis lupus familiaris"));
            Assert.That(extent[1].Contains("Id=2"));
            Assert.That(extent[1].Contains("Name=Cat"));
            Assert.That(extent[1].Contains("Description=Felis catus"));
            
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            Specie dog = new Specie("Dog", "Canis lupus familiaris");

            // Act
            var json = File.ReadAllText(_testPath);

            // Assert
            Assert.IsTrue(json.Contains("\"Name\": \"Dog\""));
            Assert.IsTrue(json.Contains("\"Description\": \"Canis lupus familiaris\""));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPath, "[{\"Id\": 1, \"Name\":\"Dog\", \"Description\": \"Canis lupus familiaris\"}]");

            // Act
            var extent = Specie.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("Name=Dog"));
            Assert.That(extent[0].Contains("Description=Canis lupus familiaris"));
        }

        [Test]
        public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
        {
            // Act & Assert
            Assert.Throws<EmptyStringException>(() =>
            {
                // Arrange
                Specie dog = new Specie("", "Canis lupus familiaris");
            });
        }
        
        [Test]
        public void Description_ShouldThrowAnEmptyStringException_ForEmptyDescriptionString()
        {
            // Act & Assert
            Assert.Throws<EmptyStringException>(() =>
            {
                // Arrange
                Specie dog = new Specie("Dog", "");
            });
        }
        
    }

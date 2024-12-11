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
        
        [Test]
        public void AddPet_ValidPet_AddsSuccessfully()
        {
            // Arrange
            var specie = new Specie("Dog", "Canine species");
            var pet = new Pet("Buddy", Sex.Male, 15.0, new DateTime(2020, 1, 1), new List<Color> { Color.Brown });
            specie.AddPet(pet);

            // Assert
            Assert.That(specie.GetPets()[0].Equals(pet));
            Assert.That(pet.GetSpecie()!.Equals(specie));
        }

        [Test]
        public void AddPet_DuplicatePet_ThrowsException()
        {
            // Arrange
            var specie = new Specie("Cat", "Feline species");
            var pet = new Pet("Kitty", Sex.Female, 5.0, new DateTime(2021, 5, 15), [Color.White]);
            specie.AddPet(pet);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => specie.AddPet(pet));
        }
        
        public void AddPet_ShouldThrowANullReferenceException()
        {
            // Arrange
            var specie = new Specie("Cat", "Feline species");
            var pet = new Pet("Kitty", Sex.Female, 5.0, new DateTime(2021, 5, 15), [Color.White]);
            specie.AddPet(pet);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => specie.AddPet(null));
        }

        [Test]
        public void RemovePet_ValidPet_RemovesSuccessfully()
        {
            // Arrange
            var specie = new Specie("Bird", "Avian species");
            var pet = new Pet("Tweety", Sex.Female, 0.5, new DateTime(2022, 3, 10), new List<Color> { Color.Yellow });
            specie.AddPet(pet);

            // Act
            specie.RemovePet(pet);

            // Assert
            Assert.That(specie.GetPets().Count.Equals(0));
            Assert.That(pet.GetSpecie() == null);
        }

        [Test]
        public void RemovePet_NonExistentPet_ThrowsException()
        {
            // Arrange
            var specie = new Specie("Fish", "Aquatic species");
            var specie1 = new Specie("Fish1", "Aquatic species");
            var pet = new Pet("Nemo", Sex.Male, 1.0, new DateTime(2023, 4, 12), new List<Color> { Color.Blue });
            specie.AddPet(pet);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => specie1.RemovePet(pet));
        }
        
        [Test]
        public void RemovePet_ShouldThrowANullReferenceException()
        {
            // Arrange
            var specie = new Specie("Fish", "Aquatic species");
            var specie1 = new Specie("Fish1", "Aquatic species");
            var pet = new Pet("Nemo", Sex.Male, 1.0, new DateTime(2023, 4, 12), new List<Color> { Color.Blue });
            specie.AddPet(pet);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => specie1.RemovePet(null));
        }
        
    }

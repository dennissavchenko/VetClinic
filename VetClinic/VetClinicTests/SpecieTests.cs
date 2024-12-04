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
            var pet = new Pet("Buddy", Sex.Male, 15.0, new DateTime(2020, 1, 1), new List<Color> { Color.Brown }, specie);

            // Assert
            Assert.That(specie.GetPets()[0].Name.Equals("Buddy"));
        }

        [Test]
        public void AddPet_DuplicatePet_ThrowsException()
        {
            // Arrange
            var specie = new Specie("Cat", "Feline species");
            var pet = new Pet("Kitty", Sex.Female, 5.0, new DateTime(2021, 5, 15), new List<Color> { Color.White }, specie);

            // Act & Assert
            Assert.Throws<DuplicatesException>(() => specie.AddPet(pet));
        }

        [Test]
        public void RemovePet_ValidPet_RemovesSuccessfully()
        {
            // Arrange
            var specie = new Specie("Bird", "Avian species");
            var pet = new Pet("Tweety", Sex.Female, 0.5, new DateTime(2022, 3, 10), new List<Color> { Color.Yellow }, specie);

            // Act
            specie.RemovePet(pet);

            // Assert
            Assert.That(specie.GetPets().Count.Equals(0));
        }

        [Test]
        public void RemovePet_NonExistentPet_ThrowsException()
        {
            // Arrange
            var specie = new Specie("Fish", "Aquatic species");
            var specie1 = new Specie("Fish1", "Aquatic species");
            var pet = new Pet("Nemo", Sex.Male, 1.0, new DateTime(2023, 4, 12), new List<Color> { Color.Blue }, specie);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => specie1.RemovePet(pet));
        }

        [Test]
        public void ModifyPet_ValidPet_ModifiesSuccessfully()
        {
            // Arrange
            var specie = new Specie("Reptile", "Reptilian species");
            var pet = new Pet("Lizzy", Sex.Female, 2.0, new DateTime(2019, 11, 11), new List<Color> { Color.Green }, specie);

            // Act
            pet.Name = "Lizard";
            specie.ModifyPet(pet);

            // Assert
            Assert.That(specie.GetPets()[0].Name.Equals("Lizard"));
        }

        [Test]
        public void ModifyPet_NonExistentPet_ThrowsException()
        {
            // Arrange
            var specie1 = new Specie("Amphibian", "Amphibious species");
            var specie2 = new Specie("Amphibian1", "Amphibious species");
            var pet = new Pet("Froggy", Sex.Male, 0.2, new DateTime(2020, 6, 21), new List<Color> { Color.Green }, specie1);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => specie2.ModifyPet(pet));
        }
        
    }

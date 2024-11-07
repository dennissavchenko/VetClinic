using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Exceptions;
using VetClinic;

namespace VetClinicTests
{
    public class MedicationTests
    {       private string _testPath;

            [SetUp]
            public void Setup()
            {
                _testPath = "../../../Data/Medication.json";
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
            public void AddToExtent_ShouldAddMedicationCorrectly()
            {
                // Arrange
                var medication1 = new Medication("Paracetamol",Form.Pill);
                var medication2 = new Medication("Alfalfa", Form.Syrup);

            // Act
            var extent = Medication.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Name=Paracetamol"));
            Assert.IsTrue(extent[0].Contains("Form=Pill"));
            Assert.IsTrue(extent[1].Contains("Id=2"));
            Assert.IsTrue(extent[1].Contains("Name=Alfalfa"));
            Assert.IsTrue(extent[1].Contains("Form=Syrup"));
            
        }

         

            [Test]
            public void SaveExtent_ShouldSerializeToJsonCorrectly()
            {
                // Arrange
                var medication = new Medication("abc",Form.Injection);

                // Act
                var json = File.ReadAllText(_testPath);

                // Assert
                Assert.IsTrue(json.Contains("\"Name\": \"abc\""));
                Assert.IsTrue(json.Contains("\"Form\": 1"));
            }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPath, "[{\"Id\":1,\"Name\":\"Gel\",\"Form\":2}]");

            // Act
            var extent = Medication.GetExtentAsString();
            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Name=Gel"));
            Assert.IsTrue(extent[0].Contains("Form=Cream"));
        }

            [Test]
            public void Name_ShouldThrowAnEmptyStringException_ForEmptyNameString()
            {
                // Act & Assert
                Assert.Throws<EmptyStringException>(() =>
                {
                    // Arrange
                    var medication = new Medication("",Form.Pill);
                });
            }
    }
}
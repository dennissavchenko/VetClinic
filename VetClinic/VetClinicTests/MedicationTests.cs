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
                var medication = new Medication("Paracetamol",Form.Pill);

                // Act
                var extent = Medication.GetExtent();

                // Assert
                Assert.That(extent.Count, Is.EqualTo(1));
                Assert.That(extent[0].Name, Is.EqualTo("Paracetamol"));
                Assert.That(extent[0].Form, Is.EqualTo(Form.Pill));
            }

            [Test]
            public void AddToExtent_ShouldAssignIdCorrectly()
            {
                // Arrange
                var medication1 = new Medication("apap",Form.Pill);
                var medication2 = new Medication("ibroprufen",Form.Pill);

                // Act
                var extent = Medication.GetExtent();

                // Assert
                Assert.That(extent.Count, Is.EqualTo(2));
                Assert.That(extent[0].Id, Is.EqualTo(1));
                Assert.That(extent[1].Id, Is.EqualTo(2));
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
                File.WriteAllText(_testPath, "[{\"Id\":1,\"Name\":\"gel\",\"Form\":2}]");

                // Act
                var extent = Medication.GetExtent();

                // Assert
                Assert.That(extent.Count, Is.EqualTo(1));
                Assert.That(extent[0].Name, Is.EqualTo("gel"));
                Assert.That(extent[0].Form, Is.EqualTo(Form.Cream));
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Exceptions;
using VetClinic;

namespace VetClinicTests
{
    public class DoseTests
    {
        private string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = "../../../Data/Dose.json";
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
        public void AddToExtent_ShouldAddDoseCorrectly()
        {
            // Arrange
            var Dose = new Dose("Take once per day",20);

            // Act
            var extent = Dose.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].Description, Is.EqualTo("Take once per day"));
            Assert.That(extent[0].Amount, Is.EqualTo(20));
        }

        [Test]
        public void AddToExtent_ShouldAssignIdCorrectly()
        {
            // Arrange
            var dose1 = new Dose("Take every 8 hrs",23);
            var dose2 = new Dose("Use every two days", 40);

            // Act
            var extent = Dose.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(2));
            Assert.That(extent[0].Id, Is.EqualTo(1));
            Assert.That(extent[1].Id, Is.EqualTo(2));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var dose = new Dose("Every day for two months", 60);

            // Act
            var json = File.ReadAllText(_testPath);

            // Assert
            Assert.IsTrue(json.Contains("\"Description\": \"Every day for two months\""));
            Assert.IsTrue(json.Contains("\"Amount\": 60"));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPath, "[{\"Id\":1,\"Description\":\"Take two per day\",\"Amount\":30}]");

            // Act
            var extent = Dose.GetExtent();

            // Assert
            Assert.That(extent.Count, Is.EqualTo(1));
            Assert.That(extent[0].Description, Is.EqualTo("Take two per day"));
            Assert.That(extent[0].Amount, Is.EqualTo(30));
        }

        [Test]
        public void Description_ShouldThrowAnEmptyStringException_ForEmptyDescriptionString()
        {
            // Act & Assert
            Assert.Throws<EmptyStringException>(() =>
            {
                // Arrange
                var dose = new Dose("", 30);
            });
        }



        [Test]
        public void Amount_ShouldThrowANegativeValueException_ForNegativeAmount()
        {
            // Act & Assert
            Assert.Throws<NegativeValueException>(() =>
            {
                // Arrange
                var dose = new Dose("xyu", -20);
            });
        }


    }
}

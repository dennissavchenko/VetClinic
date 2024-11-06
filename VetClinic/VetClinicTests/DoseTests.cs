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
            var Dose1= new Dose("Take once per day",20);
            var Dose2 = new Dose("Take twice per day", 25);


            // Act
            var extent = Dose.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Description=Take once per day"));
            Assert.IsTrue(extent[0].Contains("Amount=20"));
         
            Assert.IsTrue(extent[1].Contains("Id=2"));
            Assert.IsTrue(extent[1].Contains("Description=Take twice per day"));
            Assert.IsTrue(extent[1].Contains("Amount=25"));
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
            var extent = Dose.GetExtentAsString();

            // Assert
            Assert.IsTrue(extent[0].Contains("Id=1"));
            Assert.IsTrue(extent[0].Contains("Description=Take two per day"));
            Assert.IsTrue(extent[0].Contains("Amount=30"));
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

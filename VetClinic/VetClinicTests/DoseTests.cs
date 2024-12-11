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
            var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
            var medication1 = new Medication("abc", Form.Pill);
            var prescription2 = new Prescription(new DateTime(2000, 3, 24), new DateTime(2019, 3, 22));
            var medication2 = new Medication("vnvnvnvnv", Form.Pill);
            var dose1 = new Dose("Take once per day", 20, medication1, prescription1);
            var dose2 = new Dose("Take twice per day", 25, medication2, prescription2);


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
            var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
            var medication1 = new Medication("abc", Form.Pill);
            var dose = new Dose("Every day for two months", 60, medication1, prescription1);

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
                var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
                var medication1 = new Medication("abc", Form.Pill);
                var dose = new Dose("", 30, medication1, prescription1);
            });
        }



        [Test]
        public void Amount_ShouldThrowANegativeValueException_ForNegativeAmount()
        {
            // Act & Assert
            Assert.Throws<NegativeValueException>(() =>
            {
                // Arrange
                var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
                var medication1 = new Medication("abc", Form.Pill);
                var dose = new Dose("xyu", -20, medication1, prescription1);
            });
        }
        
        [Test]
        public void AddMedication_ShouldAddMedicationCorrectly()
        {
            // Arrange
            var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
            var medication1 = new Medication("abc", Form.Pill);
            var dose = new Dose("Take once per day", 20, medication1, prescription1);

            // Act
            var medication = dose.GetMedication();
            var prescription = dose.GetPrescription();

            // Assert
            Assert.That(medication.Equals(medication1));
            Assert.That(prescription.Equals(prescription1));
            Assert.That(medication1.GetDoses().Contains(dose));
            Assert.That(prescription1.GetDoses().Contains(dose));
        }

        [Test]
        public void AddPrescription_ShouldModifyPrescriptionCorrectly()
        {
            // Arrange
            var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
            var prescription2 = new Prescription(new DateTime(2000, 3, 24), new DateTime(2019, 3, 22));
            var medication1 = new Medication("abc", Form.Pill);
            var dose = new Dose("Take once per day", 20, medication1, prescription1);

            // Act
            dose.AddPrescription(prescription2);

            // Assert
            Assert.That(dose.GetPrescription().Equals(prescription2));
            Assert.That(!prescription1.GetDoses().Contains(dose));
            Assert.That(prescription2.GetDoses().Contains(dose));
        }
        
        [Test]
        public void AddMedication_ShouldModifyMedicationCorrectly()
        {
            // Arrange
            var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
            var medication1 = new Medication("abc", Form.Pill);
            var medication2 = new Medication("cdg", Form.Cream);
            var dose = new Dose("Take once per day", 20, medication1, prescription1);

            // Act
            dose.AddMedication(medication2);

            // Assert
            Assert.That(dose.GetMedication().Equals(medication2));
            Assert.That(!medication1.GetDoses().Contains(dose));
            Assert.That(medication2.GetDoses().Contains(dose));
        }
        
        [Test]
        public void RemoveMedication_ShouldRemoveDoseFromExtent_ShouldRemoveDoseFromItsPrescriptionAndMedication()
        {
            // Arrange
            var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
            var medication1 = new Medication("abc", Form.Pill);
            var dose = new Dose("Take once per day", 20, medication1, prescription1);

            // Act
            dose.RemoveDose();

            // Assert
            Assert.That(!Dose.GetDoses().Contains(dose));
            Assert.That(!medication1.GetDoses().Contains(dose));
            Assert.That(!prescription1.GetDoses().Contains(dose));
        }
        
        [Test]
        public void RemoveDose_ShouldThrowANotFoundException_WhenTryingToDeleteAlreadyRemovedDose()
        {
            // Act & Assert
            Assert.Throws<NotFoundException>(() =>
            {
                // Arrange
                var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
                var medication1 = new Medication("abc", Form.Pill);
                var dose = new Dose("Take once per day", 20, medication1, prescription1);
                dose.RemoveDose();
                dose.RemoveDose();
            });
        }
        
        [Test]
        public void Dose_ShouldThrowANullReferenceException_ForMedication()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
                var dose = new Dose("Take once per day", 20, null, prescription1);
            });
        }
        
        [Test]
        public void Dose_ShouldThrowANullReferenceException_ForPrescription()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var medication1 = new Medication("abc", Form.Pill);
                var dose = new Dose("Take once per day", 20, medication1, null);
            });
        }
        
        [Test]
        public void AddMedication_ShouldThrowANullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
                var medication1 = new Medication("abc", Form.Pill);
                var dose = new Dose("Take once per day", 20, medication1, prescription1);
                dose.AddMedication(null);
            });
        }
        
        [Test]
        public void AddPrescription_ShouldThrowANullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                // Arrange
                var prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
                var medication1 = new Medication("abc", Form.Pill);
                var dose = new Dose("Take once per day", 20, medication1, prescription1);
                dose.AddPrescription(null);
            });
        }

    }
}

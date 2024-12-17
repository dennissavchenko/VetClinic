using VetClinic;
using VetClinic.Exceptions;
namespace VetClinicTests;

public class MedicationTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Medication.json";
        if (File.Exists(_testPath))
        {
            File.Delete(_testPath);
        }
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
        var medication1 = new Medication("Paracetamol", Form.Pill);
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
        var medication = new Medication("abc", Form.Injection);

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
            var medication = new Medication("", Form.Pill);
        });
    }
    
    [Test]
    public void AddPrescription_ShouldAddPrescriptionCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);

        // Act
        medication.AddPrescription(prescription, "Take after meals", 500);

        // Assert
        Assert.That(medication.GetDoses().Any(x => x.GetPrescription().Equals(prescription)));
        Assert.That(prescription.GetDoses().Any(x => x.GetMedication().Equals(medication)));
    }

    [Test]
    public void AddPrescription_ShouldThrowDuplicatesException_WhenPrescriptionAlreadyExists()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);
        medication.AddPrescription(prescription, "Take after meals", 500);

        // Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            medication.AddPrescription(prescription, "Take before meals", 250);
        });
    }

    [Test]
    public void AddPrescription_ShouldThrowNullReferenceException_WhenPrescriptionIsNull()
    {
        // Arrange
        var medication = new Medication("Paracetamol", Form.Pill);

        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            medication.AddPrescription(null!, "Take after meals", 500);
        });
    }
    
    [Test]
    public void AddPrescription_ShouldThrowEmptyStringException_ForDescription()
    {
        // Arrange
        var medication = new Medication("Paracetamol", Form.Pill);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));

        // Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            medication.AddPrescription(prescription, null!, 500);
        });
    }

    [Test]
    public void AddPrescription_ShouldThrowDuplicatesException_WhenDoseAlreadyExists()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);
        var dose = new Dose("Take after meals", 500, medication, prescription);

        // Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            medication.AddPrescription(dose);
        });
    }

    [Test]
    public void AddPrescription_ShouldThrowNullReferenceException_WhenDoseIsNull()
    {
        // Arrange
        var medication = new Medication("Paracetamol", Form.Pill);

        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            medication.AddPrescription(null!);
        });
    }

    [Test]
    public void RemovePrescription_ShouldRemovePrescriptionCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);
        medication.AddPrescription(prescription, "Take after meals", 500);

        // Act
        medication.RemovePrescription(prescription);

        // Assert
        Assert.That(!medication.GetDoses().Any(x => x.GetPrescription().Equals(prescription)));
        Assert.That(!prescription.GetDoses().Any(x => x.GetMedication().Equals(medication)));
    }

    [Test]
    public void RemovePrescription_ShouldThrowNotFoundException_WhenPrescriptionDoesNotExist()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);

        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
            medication.RemovePrescription(prescription);
        });
    }

    [Test]
    public void RemovePrescription_ShouldThrowNullReferenceException_WhenPrescriptionIsNull()
    {
        // Arrange
        var medication = new Medication("Paracetamol", Form.Pill);

        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            medication.RemovePrescription(null!);
        });
    }
    
    [Test]
    public void RemoveMedication_ShouldRemoveMedicationCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);
        medication.AddPrescription(prescription, "Take after meals", 500);

        // Act
        medication.RemoveMedication();

        // Assert
        Assert.That(!Medication.GetCurrentExtent().Contains(medication));
        Assert.That(!medication.GetDoses().Any(x => x.GetPrescription().Equals(prescription)));
        Assert.That(!prescription.GetDoses().Any(x => x.GetMedication().Equals(medication)));
    }
    
    [Test]
    public void RemoveMedication_ShouldThrowANotFoundExceptionException()
    {
        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
            var medication = new Medication("Paracetamol", Form.Pill);
            medication.RemoveMedication();
            medication.RemoveMedication();
        });
    }
    
    [Test]
    public void AddComponent_ValidMedication_ShouldSucceed()
    {
        // Arrange
        var medA = new Medication("MedA", Form.Pill);
        var medB = new Medication("MedB", Form.Injection);

        // Act
        medA.AddComponent(medB);

        // Assert
        Assert.That(medA.GetComponents().Contains(medB));
        Assert.That(medB.GetComponentOf().Contains(medA));
    }

    [Test]
    public void AddComponent_DuplicateMedication_ShouldThrowDuplicatesException()
    {
        // Arrange
        var medA = new Medication("MedA", Form.Pill);
        var medB = new Medication("MedB", Form.Injection);
        medA.AddComponent(medB);

        // Act & Assert
        Assert.Throws<DuplicatesException>(() => medA.AddComponent(medB));
    }
    
    [Test]
    public void AddComponent_SelfAsComponent_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var medA = new Medication("MedA", Form.Pill);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => medA.AddComponent(medA));
    }

    [Test]
    public void AddComponent_CyclicDependency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var medA = new Medication("MedA", Form.Pill);
        var medB = new Medication("MedB", Form.Injection);
        medA.AddComponent(medB);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => medB.AddComponent(medA));
    }
    
    [Test]
    public void RemoveComponent_ValidComponent_ShouldSucceed()
    {
        // Arrange
        var medA = new Medication("MedA", Form.Pill);
        var medB = new Medication("MedB", Form.Injection);
        medA.AddComponent(medB);

        // Act
        medA.RemoveComponent(medB);

        // Assert
        Assert.That(!medA.GetComponents().Contains(medB));
        Assert.That(!medB.GetComponentOf().Contains(medA));
    }
        
    [Test]
    public void RemoveComponent_NotAComponent_ShouldThrowNotFoundException()
    {
        // Arrange
        var medA = new Medication("MedA", Form.Pill);
        var medB = new Medication("MedB", Form.Injection);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => medA.RemoveComponent(medB));
    }
    
}

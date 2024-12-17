using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class PrescriptionTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Prescription.json";
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
    public void AddToExtent_ShouldAddPrescriptionCorrectly()
    {
        // Arrange
        var prescription1 = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var prescription2 = new Prescription(DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(3));

        // Act
        var extent = Prescription.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains($"StartDate={DateTime.Today:yyyy-MM-dd}"));
        Assert.That(extent[0].Contains($"EndDate={DateTime.Today.AddMonths(1):yyyy-MM-dd}"));
        Assert.That(extent[1].Contains("Id=2"));
        Assert.That(extent[1].Contains($"StartDate={DateTime.Today.AddMonths(-1):yyyy-MM-dd}"));
        Assert.That(extent[1].Contains($"EndDate={DateTime.Today.AddDays(3):yyyy-MM-dd}"));

    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains($"\"StartDate\": \"{DateTime.Today:yyyy-MM-dd}"));
        Assert.IsTrue(json.Contains($"\"EndDate\": \"{DateTime.Today.AddMonths(1):yyyy-MM-dd}"));

    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{\"Id\": 1, \"StartDate\":\"2020-05-01T00:00:00\",\"EndDate\":\"2020-06-01T00:00:00\"}]");

        // Act
        var extent = Prescription.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("StartDate=2020-05-01"));
        Assert.That(extent[0].Contains("EndDate=2020-06-01"));
    }
    
    [Test]
    public void StartDate_EndDate_ShouldThrowAnInvalidDateException_StartDateBiggerThanEndDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(-2));
        });
    }
    
    [Test]
    public void AddMedication_ShouldAddMedicationCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("abc", Form.Pill);

        // Act
        prescription.AddMedication(medication, "Take twice per day", 25);

        // Assert
        Assert.That(prescription.GetDoses().Any(x => x.GetMedication().Equals(medication)));
        Assert.That(medication.GetDoses().Any(x => x.GetPrescription().Equals(prescription)));
        Assert.That(prescription.GetDoses().Any(x => x.Amount.Equals(25) && x.Description.Equals("Take twice per day")));
    }
    
    [Test]
    public void AddMedication_ShouldThrowADuplicateException_CreatingDose()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("abc", Form.Pill);

        // Act
        prescription.AddMedication(medication, "Take twice per day", 25);

        // Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            prescription.AddMedication(medication, "Take twice per day", 10);
        });
    }
    
    [Test]
    public void AddMedication_ShouldThrowANullReferenceException_ForMedication()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange
            var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(2));
            prescription.AddMedication(null!, "Take twice per day", 25);
        });
    }
    
    [Test]
    public void AddMedication_ShouldThrowAnEmptyStringException_ForDescription()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var medication = new Medication("abc", Form.Pill);
            var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(2));
            prescription.AddMedication(medication, null!, 25);
        });
    }
    
    [Test]
    public void AddMedication_ShouldThrowADuplicateException_Dose()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("abc", Form.Pill);
        var dose = new Dose("Take twice per day", 25, medication, prescription);
        
        // Assert
        Assert.Throws<DuplicatesException>(() =>
        {
            prescription.AddMedication(dose);
        });
    }
    
    [Test]
    public void AddMedication_ShouldThrowANullReferenceException_ForDose()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            // Arrange
            var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(2));
            prescription.AddMedication(null!);
        });
    }
    
    [Test]
    public void RemoveMedication_ShouldRemoveMedicationCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("abc", Form.Pill);
        prescription.AddMedication(medication, "Take twice per day", 25);

        // Act
        prescription.RemoveMedication(medication);

        // Assert
        Assert.That(!prescription.GetDoses().Any(x => x.GetMedication().Equals(medication)));
        Assert.That(!medication.GetDoses().Any(x => x.GetPrescription().Equals(prescription)));
    }
    
    [Test]
    public void RemoveMedication_ShouldThrowANotFoundException()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("abc", Form.Pill);
        
        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
            prescription.RemoveMedication(medication);
        });
    }
    
    [Test]
    public void RemoveMedication_ShouldThrowANullReferenceException()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        
        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            prescription.RemoveMedication(null!);
        });
    }
    
    [Test]
    public void RemovePrescription_ShouldRemoveMedicationCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var medication = new Medication("Paracetamol", Form.Pill);
        medication.AddPrescription(prescription, "Take after meals", 500);

        // Act
        prescription.RemovePrescription();

        // Assert
        Assert.That(!Prescription.GetCurrentExtent().Contains(prescription));
        Assert.That(!medication.GetDoses().Any(x => x.GetPrescription().Equals(prescription)));
        Assert.That(!prescription.GetDoses().Any(x => x.GetMedication().Equals(medication)));
    }
    
    [Test]
    public void RemovePrescription_ShouldThrowANotFoundExceptionException()
    {
        // Assert
        Assert.Throws<NotFoundException>(() =>
        {
           var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
           prescription.RemovePrescription();
           prescription.RemovePrescription();
        });
    }
    
    [Test]
    public void AddVeterinarian_ShouldSetVeterinarianAndVerifyReverseConnection()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        var veterinarian = new Veterinarian("Marek", "Kowalski", "888999111", "marek.kowalski@gmail.com", Specialization.Dentistry, ExperienceLevel.Advanced);

        // Act
        prescription.AddVeterinarian(veterinarian);

        // Assert
        Assert.That(prescription.GetVeterinarian() == veterinarian);
        Assert.That(veterinarian.GetPrescriptions().Contains(prescription));
    }

    [Test]
    public void RemoveVeterinarian_ShouldClearAssociationAndVerifyReverseConnection()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        var veterinarian = new Veterinarian("Marta", "Nowicka", "112233445", "marta.nowicka@gmail.com", Specialization.Dermatology, ExperienceLevel.Intermediate);
        prescription.AddVeterinarian(veterinarian);

        // Act
        prescription.RemoveVeterinarian();

        // Assert
        Assert.IsNull(prescription.GetVeterinarian());
        Assert.IsFalse(veterinarian.GetPrescriptions().Contains(prescription));
    }

    [Test]
    public void AddVeterinarian_ShouldThrowMethodMisuseException_WhenVeterinarianAlreadySet()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        var veterinarian1 = new Veterinarian("Marek", "Kowalski", "888999111", "marek.kowalski@gmail.com", Specialization.Dentistry, ExperienceLevel.Advanced);
        var veterinarian2 = new Veterinarian("Piotr", "Nowak", "545333211", "piotr.nowak@gmail.com", Specialization.Surgery, ExperienceLevel.Senior);
        prescription.AddVeterinarian(veterinarian1);

        // Act & Assert
        Assert.Throws<MethodMisuseException>(() => prescription.AddVeterinarian(veterinarian2));
    }

    [Test]
    public void RemoveVeterinarian_ShouldThrowNotFoundException_WhenNoVeterinarianSet()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));

        // Act & Assert
        Assert.Throws<NotFoundException>(() => prescription.RemoveVeterinarian());
    }

    [Test]
    public void StartDate_ShouldThrowInvalidDateException_WhenStartDateIsAfterEndDate()
    {
        Assert.Throws<InvalidDateException>(() => new Prescription(DateTime.Today.AddDays(5), DateTime.Today));
    }

    [Test]
    public void AddMedication_ShouldThrowNullReferenceException_WhenMedicationIsNull()
    {
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        Assert.Throws<NullReferenceException>(() => prescription.AddMedication(null!, "Take daily", 100));
    }

    [Test]
    public void RemoveMedication_ShouldThrowNotFoundException_WhenMedicationNotAssociated()
    {
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        var medication = new Medication("Antibiotic", Form.Pill);

        Assert.Throws<NotFoundException>(() => prescription.RemoveMedication(medication));
    }

}

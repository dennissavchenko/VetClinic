using System.Reflection;
using VetClinic;
using VetClinic.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));

        // Act
        var extent = Prescription.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].StartDate, Is.EqualTo(DateTime.Today));
        Assert.That(extent[0].EndDate, Is.EqualTo(DateTime.Today.AddMonths(1)));

    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var prescription1 = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));
        var prescription2 = new Prescription(DateTime.Today, DateTime.Today.AddMonths(2));

        // Act
        var extent = Prescription.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.That(extent[0].Id, Is.EqualTo(1));
        Assert.That(extent[1].Id, Is.EqualTo(2));
    }

    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddMonths(1));

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains($"\"StartDate\": \"{DateTime.Today:yyyy-MM-ddTHH:mm:ss}"));
        Assert.IsTrue(json.Contains($"\"EndDate\": \"{DateTime.Today.AddMonths(1):yyyy-MM-ddTHH:mm:ss}"));

    }

    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{\"StartDate\":\"2020-05-01T00:00:00\",\"EndDate\":\"2020-06-01T00:00:00\"}]");

        // Act
        var extent = Prescription.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].StartDate, Is.EqualTo(new DateTime(2020, 05, 01)));
        Assert.That(extent[0].EndDate, Is.EqualTo(new DateTime(2020, 06, 01)));
    }
    
    [Test]
    public void StartDate_EndDate_ShouldThrowAnInvalidDateException_StartDateBiggerThanEndDate()
    {
        // Act & Assert
        Assert.Throws<InvalidDateException>(() =>
        {
            // Arrange
            var prescription1 = new Prescription(DateTime.Today, DateTime.Today.AddMonths(-2));
        });
    }
    
}

//Missing unitTesting for class Prescription/Dose/Medication.
//Missing additional error handling for startdate/endDate in prescription
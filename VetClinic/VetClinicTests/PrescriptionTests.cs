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
        var prescription = new Prescription(new DateTime(2018, 5, 1), new DateTime(2018, 6, 1));

        // Act
        var extent = Prescription.GetExtent();

        // Assert
        Assert.That(extent.Count, Is.EqualTo(1));
        Assert.That(extent[0].StartDate, Is.EqualTo(new DateTime(2018, 5, 1)));
        Assert.That(extent[0].EndDate, Is.EqualTo(new DateTime(2018, 6, 1)));

    }

    [Test]
    public void AddToExtent_ShouldAssignIdCorrectly()
    {
        // Arrange
        var prescription1 = new Prescription(new DateTime(2020, 6, 10), new DateTime(2020, 8, 10));
        var prescription2 = new Prescription(new DateTime(2019, 8, 15), new DateTime(2020, 6, 10));

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
        var prescription = new Prescription(new DateTime(2017, 3, 20), new DateTime(2017, 6, 20));

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"StarDate\": \"2017-03-20T00:00:00\""));
        Assert.IsTrue(json.Contains("\"EndDate\": \"2017-06-20T00:00:00\""));

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
    public void StartDate_ShouldThrowAnEmptyStringException_ForEmptyStartDate()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var prescription = new Prescription(new DateTime(), new DateTime(2020, 1, 1));
        });
    }

    [Test]
    public void EndDate_ShouldThrowAEmptyStringException_ForEmptyEndDate()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var prescription = new Prescription(new DateTime(2020, 1, 1), new DateTime());
        });
    }
}
//Missing unitTesting for class Prescription/Dose/Medication.
//Missing additional error handling for startdate/endDate in prescription
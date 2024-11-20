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

}

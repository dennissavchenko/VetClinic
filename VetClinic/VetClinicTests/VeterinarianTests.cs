using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class VeterinarianTests
{
    private string _testPath;

    [SetUp]
    public void Setup()
    {
        _testPath = "../../../Data/Veterinarian.json";
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
    public void AddToExtent_ShouldAddVeterinarianCorrectly()
    {
        // Arrange
        var veterinarian1 = new Veterinarian("Piotr", "Nowak", "545333211", "pnowak@gmail.com", Specialization.Dentistry, ExperienceLevel.Intermediate);
        var veterinarian2 = new Veterinarian("Marek", "Kowalski", "432567912", "kowalski@gmail.com", Specialization.Ophthalmology, ExperienceLevel.Junior);

        // Act
        var extent = Veterinarian.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("FirstName=Piotr"));
        Assert.That(extent[0].Contains("LastName=Nowak"));
        Assert.That(extent[0].Contains("PhoneNumber=545333211"));
        Assert.That(extent[0].Contains("Email=pnowak@gmail.com"));
        Assert.That(extent[0].Contains("Specialization=Dentistry"));
        Assert.That(extent[0].Contains("ExperienceLevel=Intermediate"));

        Assert.That(extent[1].Contains("Id=2"));
        Assert.That(extent[1].Contains("FirstName=Marek"));
        Assert.That(extent[1].Contains("LastName=Kowalski"));
        Assert.That(extent[1].Contains("PhoneNumber=432567912"));
        Assert.That(extent[1].Contains("Email=kowalski@gmail.com"));
        Assert.That(extent[1].Contains("Specialization=Ophthalmology"));
        Assert.That(extent[1].Contains("ExperienceLevel=Junior"));

    }
    
    [Test]
    public void SaveExtent_ShouldSerializeToJsonCorrectly()
    {
        // Arrange
        var veterinarian = new Veterinarian("Marta", "Nowicka", "112233445", "mnowicka@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);

        // Act
        var json = File.ReadAllText(_testPath);

        // Assert
        Assert.IsTrue(json.Contains("\"FirstName\": \"Marta\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Nowicka\""));
        Assert.IsTrue(json.Contains("\"PhoneNumber\": \"112233445\""));
        Assert.IsTrue(json.Contains("\"Email\": \"mnowicka@gmail.com\""));
        Assert.IsTrue(json.Contains("\"Specialization\": 1"));
        Assert.IsTrue(json.Contains("\"ExperienceLevel\": 2"));
    }
    
    [Test]
    public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
    {
        // Arrange
        File.WriteAllText(_testPath, "[{ \"Id\": 1, \"FirstName\": \"Adam\", \"LastName\": \"Szulc\", \"PhoneNumber\": \"091234567\", \"Email\": \"adam.szulc@example.com\", \"Specialization\": 3, \"ExperienceLevel\": 4}]");

        // Act
        var extent = Veterinarian.GetExtentAsString();

        // Assert
        Assert.That(extent[0].Contains("Id=1"));
        Assert.That(extent[0].Contains("FirstName=Adam"));
        Assert.That(extent[0].Contains("LastName=Szulc"));
        Assert.That(extent[0].Contains("PhoneNumber=091234567"));
        Assert.That(extent[0].Contains("Email=adam.szulc@example.com"));
        Assert.That(extent[0].Contains("Specialization=Ophthalmology"));
        Assert.That(extent[0].Contains("ExperienceLevel=Senior"));
    }

    [Test]
    public void FirstName_ShouldThrowAnEmptyStringException_ForEmptyFirstNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("", "Kowalska", "555666777", "aKowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }
    [Test]
    public void LastName_ShouldThrowAnEmptyStringException_ForEmptyLastNameString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "", "555666777", "aKowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnEmptyStringException_ForEmptyPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "", "aKowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }

    [Test]
    public void PhoneNumber_ShouldThrowAnInvalidDataException_ForInvalidPhoneNumberString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "55566", "anna@example.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }
    
    [Test]
    public void Email_ShouldThrowAnEmptyStringException_ForEmptyEmailString()
    {
        // Act & Assert
        Assert.Throws<EmptyStringException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }

    [Test]
    public void Email_ShouldThrowAnInvalidDataException_ForInvalidEmailString()
    {
        // Act & Assert
        Assert.Throws<InvalidFormatException>(() =>
        {
            // Arrange
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "akowalgmail.com", Specialization.Radiology, ExperienceLevel.Intermediate);
        });
    }
    
    [Test]
    public void AddAppointment_ShouldAddAppointmentAndVerifyReverseConnection()
    {
        // Arrange
        var veterinarian = new Veterinarian("Piotr", "Nowak", "545333211", "piotr.nowak@gmail.com", Specialization.Surgery, ExperienceLevel.Senior);
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);

        // Act
        veterinarian.AddAppointment(appointment);

        // Assert
        Assert.That(veterinarian.GetAppointments().Contains(appointment));
        Assert.That(appointment.GetVeterinarian() == veterinarian);
    }

    [Test]
    public void RemoveAppointment_ShouldRemoveAppointmentAndVerifyReverseConnection()
    {
        // Arrange
        var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "anna.kowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Junior);
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 300);
        veterinarian.AddAppointment(appointment);

        // Act
        veterinarian.RemoveAppointment(appointment);

        // Assert
        Assert.IsFalse(veterinarian.GetAppointments().Contains(appointment));
        Assert.IsNull(appointment.GetVeterinarian());
    }

    [Test]
    public void AddPrescription_ShouldAddPrescriptionAndVerifyReverseConnection()
    {
        // Arrange
        var veterinarian = new Veterinarian("Marek", "Kowalski", "888999111", "marek.kowalski@gmail.com", Specialization.Dentistry, ExperienceLevel.Advanced);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));

        // Act
        veterinarian.AddPrescription(prescription);

        // Assert
        Assert.That(veterinarian.GetPrescriptions().Contains(prescription));
        Assert.That(prescription.GetVeterinarian() == veterinarian);
    }

    [Test]
    public void RemovePrescription_ShouldRemovePrescriptionAndVerifyReverseConnection()
    {
        // Arrange
        var veterinarian = new Veterinarian("Marta", "Nowicka", "112233445", "marta.nowicka@gmail.com", Specialization.Dermatology, ExperienceLevel.Intermediate);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(15));
        veterinarian.AddPrescription(prescription);

        // Act
        veterinarian.RemovePrescription(prescription);

        // Assert
        Assert.IsFalse(veterinarian.GetPrescriptions().Contains(prescription));
        Assert.IsNull(prescription.GetVeterinarian());
    }

    [Test]
    public void AddAppointment_ShouldThrowDuplicateException_WhenAddingDuplicateAppointment()
    {
        // Arrange
        var veterinarian = new Veterinarian("Piotr", "Nowak", "545333211", "piotr.nowak@gmail.com", Specialization.Surgery, ExperienceLevel.Senior);
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);
        veterinarian.AddAppointment(appointment);

        // Act & Assert
        Assert.Throws<DuplicatesException>(() => veterinarian.AddAppointment(appointment));
    }

    [Test]
    public void RemoveAppointment_ShouldThrowNotFoundException_WhenAppointmentNotFound()
    {
        // Arrange
        var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "anna.kowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Junior);
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 300);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => veterinarian.RemoveAppointment(appointment));
    }

    [Test]
    public void RemovePrescription_ShouldThrowNotFoundException_WhenPrescriptionNotFound()
    {
        // Arrange
        var veterinarian = new Veterinarian("Marek", "Kowalski", "888999111", "marek.kowalski@gmail.com", Specialization.Dentistry, ExperienceLevel.Advanced);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));

        // Act & Assert
        Assert.Throws<NotFoundException>(() => veterinarian.RemovePrescription(prescription));
    }

    [Test]
    public void FirstName_ShouldThrowEmptyStringException_WhenFirstNameIsNullOrWhitespace()
    {
        Assert.Throws<EmptyStringException>(() => new Veterinarian("", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior));
        Assert.Throws<EmptyStringException>(() => new Veterinarian(null!, "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior));
    }

    [Test]
    public void LastName_ShouldThrowEmptyStringException_WhenLastNameIsNullOrWhitespace()
    {
        Assert.Throws<EmptyStringException>(() => new Veterinarian("John", "", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior));
        Assert.Throws<EmptyStringException>(() => new Veterinarian("John", null!, "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior));
    }

    [Test]
    public void PhoneNumber_ShouldThrowInvalidFormatException_WhenPhoneNumberIsNot9Digits()
    {
        Assert.Throws<InvalidFormatException>(() => new Veterinarian("John", "Smith", "12345", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior));
    }

    [Test]
    public void PhoneNumber_ShouldThrowEmptyStringException_WhenPhoneNumberIsNullOrWhitespace()
    {
        Assert.Throws<EmptyStringException>(() => new Veterinarian("John", "Smith", "", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior));
    }

    [Test]
    public void Email_ShouldThrowInvalidFormatException_WhenEmailIsInvalid()
    {
        Assert.Throws<InvalidFormatException>(() => new Veterinarian("John", "Smith", "123456789", "invalidemail", Specialization.Surgery, ExperienceLevel.Junior));
    }

    [Test]
    public void Email_ShouldThrowEmptyStringException_WhenEmailIsNullOrWhitespace()
    {
        Assert.Throws<EmptyStringException>(() => new Veterinarian("John", "Smith", "123456789", "", Specialization.Surgery, ExperienceLevel.Junior));
    }

    [Test]
    public void AddAppointment_ShouldThrowNullReferenceException_WhenAppointmentIsNull()
    {
        var vet = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
        Assert.Throws<NullReferenceException>(() => vet.AddAppointment(null!));
    }

    [Test]
    public void AddAppointment_ShouldThrowDuplicatesException_WhenAppointmentAlreadyAdded()
    {
        var vet = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
        vet.AddAppointment(appointment);

        Assert.Throws<DuplicatesException>(() => vet.AddAppointment(appointment));
    }

    [Test]
    public void RemoveAppointment_ShouldThrowNotFoundException_WhenAppointmentNotAssociated()
    {
        var vet = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);

        Assert.Throws<NotFoundException>(() => vet.RemoveAppointment(appointment));
    }

    [Test]
    public void AddPrescription_ShouldThrowNullReferenceException_WhenPrescriptionIsNull()
    {
        var vet = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
        Assert.Throws<NullReferenceException>(() => vet.AddPrescription(null!));
    }

    [Test]
    public void AddPrescription_ShouldThrowDuplicatesException_WhenPrescriptionAlreadyAdded()
    {
        var vet = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        vet.AddPrescription(prescription);

        Assert.Throws<DuplicatesException>(() => vet.AddPrescription(prescription));
    }

    [Test]
    public void RemovePrescription_ShouldThrowNotFoundException_WhenPrescriptionNotAssociated()
    {
        var vet = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));

        Assert.Throws<NotFoundException>(() => vet.RemovePrescription(prescription));
    }

    
}
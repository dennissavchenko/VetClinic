using VetClinic;
using VetClinic.Exceptions;

namespace VetClinicTests;

public class AppointmentTests
    {
        private string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = "../../../Data/Appointment.json";
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
        public void AddToExtent_ShouldAddAppointmentCorrectly()
        {
            // Arrange
            var appointment1 = new Appointment(new DateTime(2018, 5, 1, 16, 30, 00), AppointmentState.Scheduled, 150);
            var appointment2 = new Appointment(new DateTime(2018, 5, 2, 16, 00, 00), AppointmentState.Canceled, 200);

            // Act
            var extent = Appointment.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("DateTime=2018-05-01T16:30:00"));
            Assert.That(extent[0].Contains("State=Scheduled"));
            Assert.That(extent[0].Contains("Price=150"));
            Assert.That(extent[1].Contains("Id=2"));
            Assert.That(extent[1].Contains("DateTime=2018-05-02T16:00:00"));
            Assert.That(extent[1].Contains("State=Canceled"));
            Assert.That(extent[1].Contains("Price=200"));
        }

        [Test]
        public void SaveExtent_ShouldSerializeToJsonCorrectly()
        {
            // Arrange
            var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), AppointmentState.Canceled, 150);

            // Act
            var json = File.ReadAllText(_testPath);

            // Assert
            Assert.IsTrue(json.Contains("\"DateTime\": \"2018-05-01T14:30:00"));
            Assert.IsTrue(json.Contains("\"State\": 3"));
            Assert.IsTrue(json.Contains("\"Price\": 150"));
        }

        [Test]
        public void LoadExtent_ShouldDeserializeFromJsonCorrectly()
        {
            // Arrange
            File.WriteAllText(_testPath, "[{ \"Id\": 1, \"DateTime\": \"2018-05-01T14:30:00\", \"State\": 3, \"Price\": 150 }]");

            // Act
            var extent = Appointment.GetExtentAsString();

            // Assert
            Assert.That(extent[0].Contains("Id=1"));
            Assert.That(extent[0].Contains("DateTime=2018-05-01T14:30:00"));
            Assert.That(extent[0].Contains("State=Canceled"));
            Assert.That(extent[0].Contains("Price=150"));
        }

        [Test]
        public void Price_ShouldThrowArgumentOutOfRangeException_ForZeroOrNegativePrice()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // Arrange
                var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), AppointmentState.Scheduled, -50);
            });
        }
        
        [Test]
        public void AssignPet_ShouldAssignPetToAnAppointmentCorrectly()
        {
            // Arrange
            var pet1 = new Pet { Id = 1 };
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // Act
            appointment.AssignPet(pet1);
            
            // Assert
            Assert.That(pet1.HasAppointments);
            Assert.That(pet1.GetAppointments().Contains(appointment));
            Assert.That(appointment.GetPet() == pet1);
        }
        
        [Test]
        public void AssignPet_ShouldThrowInvalidOperationExceptionIfAppointmentAlreadyHasDifferentPet()
        {
            // Arrange
            var pet1 = new Pet { Id = 1 };
            var pet2 = new Pet { Id = 2 };
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // pet1 has the appointment
            appointment.AssignPet(pet1);

            // Act & Assert: Trying to add the same appointment to pet2
            Assert.Throws<InvalidOperationException>(() => appointment.AssignPet(pet2));
        }
        
        [Test]
        public void AssignPet_ShouldThrowNullReferenceExceptionIfPetIsNull()
        {
            // Arrange
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => appointment.AssignPet(null!));
        }

        [Test]
        public void RemovePet_ShouldUnassignPetFromAppointment()
        {
            // Arrange
            var pet1 = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), [Color.Brown]);
            var appointment = new Appointment(new DateTime(2018, 5, 1, 14, 30, 0), AppointmentState.Canceled, 150);
            appointment.AssignPet(pet1);

            // Act
            appointment.RemovePet();

            // Assert
            Assert.IsFalse(pet1.HasAppointments(), "Pet should have no appointments after appointment.RemovePet().");
            Assert.IsNull(appointment.GetPet(), "Appointment.GetPet() should be null after removal.");
        }
        
        [Test]
        public void RemovePet_ShouldThrowInvalidOperationException_AppointmentHasNoPet()
        {
            // Arrange
            var appointment = new Appointment { Id = 100, DateTime = DateTime.Now, State = AppointmentState.Scheduled, Price = 100 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => appointment.RemovePet());
        }
        
        [Test]
        public void AddVeterinarian_ShouldSetVeterinarianAndVerifyReverseConnection()
        {
            // Arrange
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);
            var veterinarian = new Veterinarian("Piotr", "Nowak", "545333211", "piotr.nowak@gmail.com", Specialization.Surgery, ExperienceLevel.Senior);

            // Act
            appointment.AddVeterinarian(veterinarian);

            // Assert
            Assert.That(appointment.GetVeterinarian() == veterinarian);
            Assert.That(veterinarian.GetAppointments().Contains(appointment));
        }

        [Test]
        public void RemoveVeterinarian_ShouldClearAssociationAndVerifyReverseConnection()
        {
            // Arrange
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);
            var veterinarian = new Veterinarian("Anna", "Kowalska", "555666777", "anna.kowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Junior);
            appointment.AddVeterinarian(veterinarian);

            // Act
            appointment.RemoveVeterinarian();

            // Assert
            Assert.IsNull(appointment.GetVeterinarian());
            Assert.IsFalse(veterinarian.GetAppointments().Contains(appointment));
        }

        [Test]
        public void AddVeterinarian_ShouldThrowMethodMisuseException_WhenVeterinarianAlreadySet()
        {
            // Arrange
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);
            var veterinarian1 = new Veterinarian("Piotr", "Nowak", "545333211", "piotr.nowak@gmail.com", Specialization.Surgery, ExperienceLevel.Senior);
            var veterinarian2 = new Veterinarian("Anna", "Kowalska", "555666777", "anna.kowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Junior);
            appointment.AddVeterinarian(veterinarian1);

            // Act & Assert
            Assert.Throws<MethodMisuseException>(() => appointment.AddVeterinarian(veterinarian2));
        }

        [Test]
        public void RemoveVeterinarian_ShouldThrowNotFoundException_WhenNoVeterinarianSet()
        {
            // Arrange
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => appointment.RemoveVeterinarian()); 
        }

        [Test]
        public void Price_ShouldThrowArgumentOutOfRangeException_WhenPriceIsZeroOrNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Appointment(DateTime.Now, AppointmentState.Scheduled, -100));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Appointment(DateTime.Now, AppointmentState.Scheduled, 0));
        }

        [Test]
        public void AssignPet_ShouldThrowNullReferenceException_WhenPetIsNull()
        { 
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
            Assert.Throws<NullReferenceException>(() => appointment.AssignPet(null!));
        }

        [Test]
        public void AssignPet_ShouldThrowInvalidOperationException_WhenReassignedToDifferentPet()
        {
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
            var pet1 = new Pet { Id = 1 };
            var pet2 = new Pet { Id = 2 };

            appointment.AssignPet(pet1);
            Assert.Throws<InvalidOperationException>(() => appointment.AssignPet(pet2));
        }

        [Test]
        public void RemovePet_ShouldThrowInvalidOperationException_WhenNoPetAssigned()
        {
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
            Assert.Throws<InvalidOperationException>(() => appointment.RemovePet());
        }

        [Test]
        public void AddVeterinarian_ShouldThrowNullReferenceException_WhenVeterinarianIsNull()
        {
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
            Assert.Throws<NullReferenceException>(() => appointment.AddVeterinarian(null!));
        }

        [Test]
        public void AddVeterinarian_ShouldThrowMethodMisuseException_WhenVeterinarianAlreadyAssigned()
        {
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
            var vet1 = new Veterinarian("John", "Smith", "123456789", "email@example.com", Specialization.Surgery, ExperienceLevel.Junior);
            var vet2 = new Veterinarian("Anna", "Kowalska", "987654321", "anna@example.com", Specialization.Radiology, ExperienceLevel.Intermediate);

            appointment.AddVeterinarian(vet1);
            Assert.Throws<MethodMisuseException>(() => appointment.AddVeterinarian(vet2));
        }

        [Test]
        public void RemoveVeterinarian_ShouldThrowNotFoundException_WhenNoVeterinarianAssigned()
        {
            var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 150);
            Assert.Throws<NotFoundException>(() => appointment.RemoveVeterinarian());
        }

    [Test]
    public void AddPayment_ValidPayment_AddsSuccessfully()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var payment = new Payment(50, PaymentType.Cash, DateTime.Now, appointment);

        
        // Assert
        Assert.That(appointment.GetPayments(), Contains.Item(payment));
        Assert.That(payment.GetAppointment(), Is.EqualTo(appointment));
    }

    [Test]
    public void AddPayment_DuplicatePayment_ThrowsException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var payment = new Payment(50, PaymentType.Cash, DateTime.Now, appointment);

        // Act & Assert
        Assert.Throws<DuplicatesException>(() => appointment.AddPayment(payment));
    }
    
    [Test]
    public void AddPayment_NullReference_ThrowsException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);

        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            var payment = new Payment(50, PaymentType.Cash, DateTime.Now, null!);
        });
    }
    
    [Test]
    public void RemovePayment_ValidPayment_RemovesSuccessfully()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var payment = new Payment(50, PaymentType.Cash, DateTime.Now, appointment);

        // Act
        appointment.RemovePayment(payment);

        // Assert
        Assert.That(appointment.GetPayments(), Does.Not.Contain(payment));
        Assert.That(!Payment.GetCurrentExtent().Contains(payment)); 
    }
    
    [Test]
    public void RemovePayment_NonExistentPayment_ThrowsException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var appointment1 = new Appointment(DateTime.Now, AppointmentState.Scheduled, 200);
        var payment = new Payment(50, PaymentType.Cash, DateTime.Now, appointment);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => appointment1.RemovePayment(payment));
    }
    
    [Test]
    public void RemovePayment_ShouldThrowANullReferenceException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var payment = new Payment(50, PaymentType.Cash, DateTime.Now, appointment);

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => appointment.RemovePayment(null!));
    }
    
    [Test]
    public void AddPrescription_ValidPrescription_AddsSuccessfully()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        
        appointment.AddPrescription(prescription);

        // Assert
        Assert.That(appointment.GetPrescription()!.Equals(prescription));
        Assert.That(prescription.GetAppointment()!.Equals(appointment));
    }
    
    [Test]
    public void AddPrescription_MethodMisuseException_ThrowsException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        var prescription1 = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        
        appointment.AddPrescription(prescription);

        // Assert
        Assert.Throws<MethodMisuseException>(() => appointment.AddPrescription(prescription1));
    }
    
    [Test]
    public void AddPrescription_NullReferenceException_ThrowsException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);

        // Assert
        Assert.Throws<NullReferenceException>(() => appointment.AddPrescription(null!));
    }
    
    [Test]
    public void RemovePrescription_RemovesSuccessfully()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        
        appointment.AddPrescription(prescription);
        Assert.That(appointment.GetPrescription()!.Equals(prescription));
        appointment.RemovePrescription();

        // Assert
        Assert.That(appointment.GetPrescription() == null);
        Assert.That(prescription.GetAppointment() == null);
    }
    
    [Test]
    public void AddPrescription_NotFoundException_ThrowsException()
    {
        // Arrange
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => appointment.RemovePrescription());
    }

    [Test]
    public void RemoveAppointment_RemovesSuccessfully_WithAssociations()
    {
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        var pet = new Pet("Buddy", Sex.Male, 15.5, new DateTime(2018, 5, 1), new List<Color> { Color.Brown });
        var vet = new Veterinarian("Anna", "Kowalska", "555666777", "anna.kowalska@gmail.com", Specialization.Radiology, ExperienceLevel.Junior);
        var prescription = new Prescription(DateTime.Today, DateTime.Today.AddDays(10));
        var payment = new Payment(50, PaymentType.Cash, DateTime.Now, appointment);

        appointment.AssignPet(pet);
        Assert.That(pet.GetAppointments().Contains(appointment));
        Assert.That(appointment.GetPet() == pet);

        appointment.AddVeterinarian(vet);
        Assert.That(vet.GetAppointments().Contains(appointment));
        Assert.That(appointment.GetVeterinarian() == vet);

        appointment.AddPrescription(prescription);
        Assert.That(appointment.GetPrescription() == prescription);
        Assert.That(prescription.GetAppointment() == appointment);
        
        // Act
        appointment.RemoveAppointment();
        
        // Assert
        Assert.IsFalse(pet.GetAppointments().Contains(appointment));
        Assert.IsNull(appointment.GetPet());
        Assert.IsFalse(vet.GetAppointments().Contains(appointment));
        Assert.IsNull(appointment.GetVeterinarian());
        Assert.IsNull(appointment.GetPrescription());
        Assert.IsNull(prescription.GetAppointment());
        Assert.IsFalse(appointment.GetPayments().Contains(payment));
        Assert.That(!Appointment.GetCurrentExtent().Contains(appointment));
        
    }
    
    [Test]
    public void RemoveAppointment_ShouldThrowNotFoundException()
    {
        var appointment = new Appointment(DateTime.Now, AppointmentState.Scheduled, 100);
        appointment.RemoveAppointment();

        Assert.Throws<NotFoundException>(() => appointment.RemoveAppointment());
    }
    
}

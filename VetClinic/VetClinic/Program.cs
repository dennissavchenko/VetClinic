using VetClinic;

if (Directory.Exists("../../../Data"))
{
    DirectoryInfo data = new DirectoryInfo("../../../Data");

    foreach (FileInfo file in data.GetFiles())
    {
        file.Delete();
    }
}

Specie dog = new Specie("Dog", "Canis lupus familiaris");
Specie cat = new Specie("Cat", "Felis catus");
Specie eagle = new Specie("Eagle", "Aquila chrysaetos");
Specie clownfish = new Specie("Clownfish", "Amphiprioninae");


Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
Fish fish1 = new Fish("Goldie", Sex.Female, 0.3, new DateTime(2021, 6, 15), [Color.Golden],WaterType.Freshwater, 24.0);
Mammal mammal1 = new Mammal("Burek", Sex.Male, 12.5, new DateTime(2018, 3, 22), [Color.Brown], true);
Bird bird1 = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 4, 10), [Color.Yellow], 0.25, true);
Client client1 = new Client("Marek","Kowalski","123456789","marek@gmail.com", [pet1]);
Veterinarian veterinarian = new Veterinarian("Jack", "Black", "555555555", "jack@g.com", Specialization.Surgery, ExperienceLevel.Senior);
Appointment appointment1 = new Appointment(DateTime.Now, AppointmentState.Scheduled, 1234);
Payment payment1 = new Payment(100, PaymentType.Cash, DateTime.Now, appointment1);
Prescription prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
Medication medication1 = new Medication("abc", Form.Pill);
Prescription prescription2 = new Prescription(new DateTime(2000, 3, 24), new DateTime(2019, 3, 22));
Medication medication2 = new Medication("vnvnvnvnv", Form.Pill);
//Dose dose1 = new Dose("xyz kkk kkk", 2.5, medication1, prescription1);
Injured injured1 = new Injured("Lola", Sex.Female, 3.5, new DateTime(2020, 2, 14), [Color.Gray], client1, InjuryType.Fracture, new DateTime(2021, 3, 15));
Healthy healthy1 = new Healthy("Momo", Sex.Male, 2.5, new DateTime(2015, 4, 13), [Color.Black, Color.White],
    ActivityLevel.Low, null);
Pregnant pregnant1 = new Pregnant("Mimi", Sex.Female, 1.5, new DateTime(2019, 6, 15), [Color.Cream], DateTime.Today.AddMonths(1), 3);

// pet1.AddSpecie(dog);
// dog.GetPets().ForEach(pet => Console.WriteLine(pet.Name));
// Console.WriteLine(pet1.GetSpecie()?.Name);
//
// pet1.RemoveSpecie();
//
// dog.AddPet(pet1);
// dog.GetPets().ForEach(pet => Console.WriteLine(pet.Name));
// Console.WriteLine(pet1.GetSpecie()?.Name);
//
// pet1.AddSpecie(cat);
// cat.GetPets().ForEach(pet => Console.WriteLine(pet.Name));
// Console.WriteLine(pet1.GetSpecie()?.Name);

// prescription1.AddMedication(medication1, "ffff", 3);
// //medication2.AddPrescription(prescription1, "gjgg", 30);
// Dose d = new Dose("fjfjff", 60, medication2, prescription1);
//
// medication1.GetDoses().ForEach(dose => Console.WriteLine(dose.Description + " med1"));
// medication2.GetDoses().ForEach(dose => Console.WriteLine(dose.Description + " med2"));
// prescription1.GetDoses().ForEach(dose => Console.WriteLine(dose.Description + " pres1"));
//
// medication1.RemovePrescription(prescription1);
// //prescription1.RemoveMedication(medication2);
//
// medication1.GetDoses().ForEach(dose => Console.WriteLine(dose.Description + " med1"));
// medication2.GetDoses().ForEach(dose => Console.WriteLine(dose.Description + " med2"));
// prescription1.GetDoses().ForEach(dose => Console.WriteLine(dose.Description + " pres1"));

// client1.GetPets().ForEach(x => Console.WriteLine(x.Name));
// Console.WriteLine(pet1.GetClient());
// Console.WriteLine(bird1.GetClient());
// Console.WriteLine(injured1.GetClient());
// Console.WriteLine(fish1.GetClient());
//
// client1.RemoveClient();
//
// client1.GetPets().ForEach(x => Console.WriteLine(x.Name));
// // Console.WriteLine(pet1.GetClient());
// // Console.WriteLine(bird1.GetClient());
// // Console.WriteLine(injured1.GetClient());
// // Console.WriteLine(fish1.GetClient());
// Client.GetCurrentExtent().ForEach(x => Console.WriteLine(x));
// Pet.GetCurrentExtent().ForEach(x => Console.WriteLine(x));

fish1.GetClient().GetPets().ForEach(x => Console.WriteLine(x));

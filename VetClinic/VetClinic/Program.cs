using VetClinic;
using Type = VetClinic.Type;

Specie dog = new Specie("Dog", "Canis lupus familiaris");
Specie cat = new Specie("Cat", "Felis catus");
Specie eagle = new Specie("Eagle", "Aquila chrysaetos");
Specie clownfish = new Specie("Clownfish", "Amphiprioninae");


Pet pet1 = new Pet("Piołun", Sex.Male, 5.2, new DateTime(2019, 5, 12), [Color.Black, Color.White]);
Fish fish1 = new Fish("Goldie", Sex.Female, 0.3, new DateTime(2021, 6, 15), [Color.Golden], WaterType.Freshwater, 24.0);
Mammal mammal1 = new Mammal("Burek", Sex.Male, 12.5, new DateTime(2018, 3, 22), [Color.Brown], true);
Bird bird1 = new Bird("Tweety", Sex.Female, 0.5, new DateTime(2020, 4, 10), [Color.Yellow], 0.25, true);
Client client1 = new Client("Marek","Kowalski","123456789","marek@gmail.com");
Veterinarian veterinarian = new Veterinarian("Jack", "Black", "555555555", "jack@g.com", "surgeon", "high");
Payment payment1 = new Payment(100, Type.Cash, DateTime.Now);
Appointment appointment1 = new Appointment(DateTime.Now, 1234);
Prescription prescription1 = new Prescription(new DateTime(2018, 3, 24), new DateTime(2019, 3, 22));
Medication medication1 = new Medication("abc", Form.Pill);
Dose dose1 = new Dose("xyz", 2.5);
Injured injured1 = new Injured("Lola", Sex.Female, 3.5, new DateTime(2020, 2, 14), [Color.Gray], InjuryType.Fracture, new DateTime(2021, 3, 15));
Healthy healthy1 = new Healthy("Momo", Sex.Male, 2.5, new DateTime(2015, 4, 13), [Color.Black, Color.White],
    ActivityLevel.Low, null);
Pregnant pregnant1 = new Pregnant("Mimi", Sex.Female, 1.5, new DateTime(2019, 6, 15), [Color.Cream], DateTime.Today.AddMonths(1), 3);

Pet.PrintExtent();
Fish.PrintExtent();
Mammal.PrintExtent();
Bird.PrintExtent();
Specie.PrintExtent();
Client.PrintExtent();
Veterinarian.PrintExtent();
Payment.PrintExtent();
Appointment.PrintExtent();
Prescription.PrintExtent();
Medication.PrintExtent(); 
Dose.PrintExtent();
Injured.PrintExtent();
Healthy.PrintExtent();
Pregnant.PrintExtent();


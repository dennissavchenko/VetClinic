
/*

    namespace VetClinic
    {
        public class Dose : StoredObject<Dose>, IIdentifiable  //Identificable underlined because im assigning an Id, I dont know if Im supposed to add it or no ://
        {
            public Prescription Prescription { get; set; }
            public Medication Medication { get; set; }
            public string Description { get; set; }
            public double Amount { get; set; }

            public Dose() { }

            public Dose(Prescription prescription, Medication medication, string description, double amount)
            {
                Prescription = prescription;
                Medication = medication;
                Description = description;
                Amount = amount;
                AddToExtent(this);
            }

            public override string ToString()
            {
                return $"Prescription Id={Prescription.Id}, Medication Id={Medication.Id}, Description={Description}, Amount={Amount}";
            }
        }
    }
*/
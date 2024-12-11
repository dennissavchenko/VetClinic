using VetClinic.Exceptions;

namespace VetClinic
{
    public class Dose : StoredObject<Dose>, IIdentifiable
    {
        public int Id { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new EmptyStringException("Description can't be empty.");
                _description = value;
            }
        }

        private double _amount;
        public double Amount
        {
            get => _amount;
            set
            {
                if (value <= 0)
                    throw new NegativeValueException("Amount must be a positive value.");
                _amount = value;
            }
        }

        private Medication _medication;
        private Medication Medication
        {
            get => _medication;
            set
            {
                _medication = value;
                _medication.AddDose(this);
            }
        }

        private Prescription _prescription;
        private Prescription Prescription
        {
            get => _prescription;
            set
            {
                _prescription = value;
                _prescription.AddDose(this);
                
            }
        }
        
        private static List<Dose> _extent = new();

        public static List<Dose> GetDoses()
        {
            return new List<Dose>(_extent);
        }

        public Medication GetMedication()
        {
            return _medication;
        }
        
        public Prescription GetPrescription()
        {
            return _prescription;
        }

        public void AddMedication(Medication medication)
        {
            if (_medication.GetDoses().Contains(this)) _medication.ModifyDose(this, medication);
            _medication = medication;
            if (!_medication.GetDoses().Contains(this)) _medication.AddDose(this);
        }
        
        public void AddPrescription(Prescription prescription)
        {
            if (_prescription.GetDoses().Contains(this)) _prescription.ModifyDose(this, prescription);
            _prescription = prescription;
            if (!_prescription.GetDoses().Contains(this)) _prescription.AddDose(this);
        }
        
        public Dose() {}

        public Dose(string description, double amount, Medication medication, Prescription prescription)
        {
            Description = description;
            Amount = amount;
            Medication = medication;
            Prescription = prescription;
            AddToExtent(this);
            _extent.Add(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, Description={Description}, Amount={Amount}";
        }

        public void RemoveDose()
        {
            if (!_extent.Contains(this)) throw new NotFoundException("Dose not found in the list.");
            _extent.Remove(this);
            _medication.RemoveDose(this);
            _prescription.RemoveDose(this);
        }
        
    }
}

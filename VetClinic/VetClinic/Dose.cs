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

        private Prescription _prescription;

        public Medication GetMedication()
        {
            return _medication;
        }
        
        public Prescription GetPrescription()
        {
            return _prescription;
        }
        
        public Dose() {}

        public Dose(string description, double amount, Medication medication, Prescription prescription)
        {
            Description = description;
            Amount = amount;
            _medication = medication;
            _prescription = prescription;
            if (!_medication.GetDoses().Contains(this)) 
                _medication.AddPrescription(this); // If this Dose is not already tracked by the Medication, add it to the Medication's list of Doses.
            AddToExtent(this);
            _extent.Add(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, Description={Description}, Amount={Amount}";
        }

        /// <summary>
        /// Removes this Dose from the system, ensuring the Dose is also disassociated from its Medication and Prescription.
        /// </summary>
        public void RemoveDose()
        {
            // If this Dose is not found in the global extent, throw an exception.
            if (!_extent.Contains(this))
                throw new NotFoundException("Dose not found in the list.");

            // Remove the Dose from the global extent of Doses.
            _extent.Remove(this);

            // Instruct the Prescription to remove the linked Medication from its records,
            // which also removes this Dose object from the Prescription side.
            _prescription.RemoveMedication(_medication);
        }
        
    }
}

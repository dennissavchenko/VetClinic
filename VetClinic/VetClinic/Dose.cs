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
        public Medication Medication
        {
            get => _medication;
            set
            {
                if (_medication == value) return;

                _medication?.RemoveDose(this); // Remove from the current medication
                _medication = value;
                _medication?.AddDose(this); // Add to the new medication
            }
        }

        private Prescription _prescription;
        public Prescription Prescription
        {
            get => _prescription;
            set
            {
                if (_prescription == value) return;

                _prescription?.RemoveDose(this); // Remove from the current prescription
                _prescription = value;
                _prescription?.AddDose(this); // Add to the new prescription
            }
        }



        public Dose() { }

        public Dose(string description, double amount)
        {
            Description = description;
            Amount = amount;
            AddToExtent(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, Description={Description}, Amount={Amount}";
        }
    }
}

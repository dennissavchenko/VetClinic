using System.Runtime.CompilerServices;
using VetClinic.Exceptions;

namespace VetClinic
{
    public class Prescription : StoredObject<Prescription>, IIdentifiable
    {
        public int Id { get; set; }
        private DateTime _startDate;
        private DateTime _endDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            { 
                if (_endDate != default && value > _endDate)
                    throw new InvalidDateException("Start date must be before end date.");
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_startDate != default && value < _startDate)
                    throw new InvalidDateException("End date must be after start date.");
                _endDate = value;
            }
        }

        private List<Dose> _doses = new();
        public List<Dose> GetDoses()
        {
            return new List<Dose>(_doses);
        }
        
        /// <summary>
        /// Associates a Medication with this Prescription by creating or retrieving a Dose.
        /// Prevents duplicates by checking if the Medication is already associated.
        /// </summary>
        public void AddMedication(Medication medication, string doseDescription, double doseAmount)
        {
            // If there's already a Dose in _doses referencing the same Medication, throw a DuplicatesException.
            if (_doses.Any(x => x.GetMedication().Equals(medication)))
                throw new DuplicatesException("Medication already exists in the list.");

            // Check if a Dose linking this Medication and this Prescription exists on the Medication side.
            var dose = medication.GetDoses().Find(x => x.GetPrescription().Equals(this));

            // If no such Dose exists, create a new one with the given doseDescription and doseAmount.
            if (dose == null) 
                dose = new Dose(doseDescription, doseAmount, medication, this);

            // If the Medication’s list of Doses doesn’t contain this new or retrieved Dose,
            // inform the Medication to add it, maintaining bidirectional consistency.
            if (!medication.GetDoses().Contains(dose))
                medication.AddPrescription(this, doseDescription, doseAmount);
        }

        /// <summary>
        /// Overload that associates an existing Dose object with this Prescription.
        /// Ensures bidirectional consistency by updating both this Prescription and the Medication side.
        /// </summary>
        public void AddMedication(Dose dose)
        {
            // Check if a Dose referencing the same Prescription or Medication already exists in _doses.
            // Throw a DuplicatesException if it does, preventing duplicate associations.
            if (_doses.Any(x => x.GetPrescription().Equals(dose.GetPrescription()) 
                                || x.GetMedication().Equals(dose.GetMedication())))
                throw new DuplicatesException("Prescription already exists in the list.");

            // Add the existing Dose object to this Prescription’s internal list.
            _doses.Add(dose);

            // If the Dose’s Medication doesn’t already track this Dose, tell the Medication to add it.
            // This ensures the relationship is fully bidirectional.
            if (!dose.GetMedication().GetDoses().Contains(dose))
                dose.GetMedication().AddPrescription(dose);
        }

        /// <summary>
        /// Removes the association between this Prescription and a given Medication.
        /// If no matching Dose is found, a NotFoundException is thrown.
        /// </summary>
        public void RemoveMedication(Medication medication)
        {
            // If the provided Medication is null, throw a NullReferenceException to prevent invalid usage.
            if (medication == null)
                throw new NullReferenceException("Medication cannot be null.");

            // Find the Dose object that links this Medication to this Prescription.
            var dose = _doses.Find(x => x.GetMedication().Equals(medication));

            // If no such Dose is found, throw a NotFoundException indicating the association doesn't exist.
            if (dose == null)
                throw new NotFoundException("This prescription in not associated with the medication.");

            // Remove the Dose from this Prescription’s list.
            _doses.Remove(dose);

            // If the Medication still references the same Dose, remove this Prescription from the Medication side
            // to maintain bidirectional consistency.
            if (medication.GetDoses().Contains(dose))
                medication.RemovePrescription(this);
        }

        /// <summary>
        /// Removes this Prescription entirely from the system, including all associated Doses.
        /// Ensures each Dose is properly removed before the Prescription is deleted.
        /// </summary>
        public void RemovePrescription()
        {
            // If the Prescription is not found in the global extent, throw a NotFoundException.
            if (!_extent.Contains(this))
                throw new NotFoundException("Prescription not found in the list.");

            // Copy the list of Doses to avoid modifying _doses while iterating.
            var doses = new List<Dose>(_doses);

            // For each Dose linked to this Prescription, call RemoveDose(),
            // which breaks the association from the Dose's side and removes it from the global extent.
            foreach (var dose in doses)
            {
                dose.RemoveDose();
            }

            // Finally, remove this Prescription from the global extent, completing the removal process.
            _extent.Remove(this);
        }
        public Prescription() { }

        public Prescription(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            AddToExtent(this);
            _extent.Add(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, StartDate={StartDate:yyyy-MM-dd}, EndDate={EndDate:yyyy-MM-dd}";
        }
    }
}


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
        
        private static List<Prescription> _extent = new();
        public List<Dose> GetDoses()
        {
            return new List<Dose>(_doses);
        }
        
        public void AddDose(Dose dose)
        {
            if (_doses.Contains(dose)) throw new DuplicatesException("Dose already exists in the list.");
            _doses.Add(dose);
            if (dose.GetPrescription() != this) dose.AddPrescription(this);
        }
        
        public void ModifyDose(Dose dose, Prescription newPrescription)
        {
            if (dose == null || newPrescription == null) throw new NullReferenceException();
            if (!_doses.Contains(dose)) throw new NotFoundException("Dose not found in the list.");
            _doses.Remove(dose);
            if(!newPrescription.GetDoses().Contains(dose)) newPrescription.AddDose(dose);
        }

        public void RemoveDose(Dose dose)
        {
            if (dose == null) throw new NullReferenceException();
            if (!_doses.Contains(dose)) throw new NotFoundException("This prescription in not associated with the dose.");
            if (Dose.GetDoses().Contains(dose)) dose.RemoveDose();
            if (_doses.Contains(dose)) _doses.Remove(dose);
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
        
        public void RemovePrescription()
        {
            if (!_extent.Contains(this)) throw new NotFoundException("Prescription not found in the list.");
            foreach (var dose in _doses)
            {
                dose.RemoveDose();
            }
            _extent.Remove(this);
        }
        
    }
}


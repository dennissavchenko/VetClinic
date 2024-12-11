using VetClinic.Exceptions;

namespace VetClinic
{
    public enum Form { Pill, Injection, Cream, Powder, Syrup }

    public class Medication : StoredObject<Medication>, IIdentifiable
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new EmptyStringException("Name is mandatory and cannot be empty.");
                _name = value;
            }
        }
        public Form Form { get; set; }
        
        private List<Dose> _doses = new();
        
        private static List<Medication> _extent = new();
        
        public List<Dose> GetDoses()
        {
            return new List<Dose>(_doses);
        }
        
        public void AddDose(Dose dose)
        {
            if (_doses.Contains(dose)) throw new DuplicatesException("Dose already exists in the list.");
            _doses.Add(dose);
            if (dose.GetMedication() != this) dose.AddMedication(this); 
        }

        public void ModifyDose(Dose dose, Medication newMedication)
        {
            if (!_doses.Contains(dose)) throw new NotFoundException("Dose not found in the list.");
            _doses.Remove(dose);
            if(!newMedication.GetDoses().Contains(dose)) newMedication.AddDose(dose);
        }

        public void RemoveDose(Dose dose)
        {
            if (Dose.GetDoses().Contains(dose)) throw new ForbiddenRemovalException("Association can only be removed if the dose is deleted from the system!");
            if (!_doses.Contains(dose)) throw new NotFoundException("This medication in not associated with the dose.");
            _doses.Remove(dose);
        }
        
    public Medication() { }

        public Medication(string name, Form form)
        {
            Name = name;
            Form = form;
            AddToExtent(this);
            _extent.Add(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, Name={Name}, Form={Form}";
        }
        
    }
}

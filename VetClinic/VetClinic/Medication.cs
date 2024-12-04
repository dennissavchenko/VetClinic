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
        public IReadOnlyList<Dose> Doses => _doses;

        public void AddDose(Dose dose)
        {
            if (_doses.Contains(dose)) return;
            _doses.Add(dose);
            dose.Medication = this; 
        }

        public void RemoveDose(Dose dose)
        {
            if (!_doses.Contains(dose)) return;
            _doses.Remove(dose);
            dose.Medication = null; 
        }
    


    public Medication() { }

        public Medication(string name, Form form)
        {
            Name = name;
            Form = form;
            AddToExtent(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, Name={Name}, Form={Form}";
        }
    }
}

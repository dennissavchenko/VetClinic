using VetClinic.Exceptions;

namespace VetClinic
{
    public enum Form { Pill, Injection, Cream, Powder }

    public class Medication : StoredObject<Medication>, IIdentifiable
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new EmptyStringException("Name is mandatory and cannot be empty.");
                _name = value;
            }
        }
        public Form Form { get; set; }

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

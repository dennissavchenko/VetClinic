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
                    throw new ArgumentException("Name is mandatory and cannot be empty.");
                _name = value;
            }
        }

        private Form _form;
        public Form Form
        {
            get => _form;
            set
            {
                if (!Enum.IsDefined(typeof(Form), value))
                    throw new ArgumentException("Form is mandatory and must be a valid value.");
                _form = value;
            }
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

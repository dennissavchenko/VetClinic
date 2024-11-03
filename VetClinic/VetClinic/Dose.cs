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
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Description can't be empty.");
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
                    throw new ArgumentException("Amount must be a positive value.");
                _amount = value;
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
            return $"Description={Description}, Amount={Amount}";
        }
    }
}

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
                if (value == default)
                    throw new ArgumentException("StartDate is mandatory and cannot be empty.");
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value == default)
                    throw new ArgumentException("EndDate is mandatory and cannot be empty.");
                _endDate = value;
            }
        }

        public Prescription() { }

        public Prescription(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            AddToExtent(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, StartDate={StartDate.ToShortDateString()}, EndDate={EndDate.ToShortDateString()}";
        }
    }
}


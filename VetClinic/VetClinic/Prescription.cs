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
                if (value == default)
                    throw new EmptyStringException("StartDate is mandatory and cannot be empty.");
                _startDate = value;

                if (_endDate != default && _endDate < _startDate)
                    throw new InvalidDateException("EndDate must be later than StartDate.");
            }
        }
 public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value == default)
                    throw new EmptyStringException("EndDate is mandatory and cannot be empty.");
                if (value < _startDate)
                    throw new InvalidDateException("EndDate must be later than StartDate.");
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


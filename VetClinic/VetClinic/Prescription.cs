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


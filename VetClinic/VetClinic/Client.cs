namespace VetClinic
{
    public class Client : StoredObject<Client>, IIdentifiable
    {
        public int Id { get; set; }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("FirstName can't be empty");
                _firstName = value;
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("LastName can't be empty");
                _lastName = value;
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("PhoneNumber can't be empty.");
                _phoneNumber = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Email can't be empty.");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Email must be a valid format.");
                _email = value;
            }
        }

        public Client() { }

        public Client(string firstName, string lastName, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            AddToExtent(this);
        }

        public override string ToString()
        {
            return $"Id={Id}, FirstName={FirstName}, LastName={LastName}, PhoneNumber={PhoneNumber}, Email={Email}";
        }
    }
}

using VetClinic.Exceptions;

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
                if (string.IsNullOrWhiteSpace(value))
                    throw new EmptyStringException("FirstName can't be empty");
                _firstName = value;
            }
        }

        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new EmptyStringException("LastName can't be empty");
                _lastName = value;
            }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new EmptyStringException("PhoneNumber can't be empty.");
                if (value.Length != 9)
                    throw new InvalidFormatException("PhoneNumber must have 9 digits.");
                _phoneNumber = value;
            }
        }

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new EmptyStringException("Email can't be empty.");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new InvalidFormatException("Email must be a valid format.");
                _email = value;
            }
        }

        private List<Pet> _pets = new();

        public List<Pet> GetPets()
        {
            return new List<Pet>(_pets);
        }
        
        /// <summary>
        /// Associates the specified pet with this client, ensuring a bidirectional relationship.
        /// </summary>
        public void AddPet(Pet pet)
        {
            // Throw DuplicatesException if the pet is already in this client's list.
            if (_pets.Contains(pet)) throw new DuplicatesException("Pet already exists in the list.");

            _pets.Add(pet);

            // Update the pet's reference to this client if necessary.
            if (!pet.GetClient().Equals(this)) pet.ModifyClient(this);
        }

        /// <summary>
        /// Removes the specified pet from this client's list, enforcing that a client must have at least one pet.
        /// </summary>
        public void RemovePet(Pet pet)
        {
            // Throw NullReferenceException if the pet reference is null.
            if (pet == null) throw new NullReferenceException("Pet cannot be null.");

            // Throw EmptyListException if this is the last pet in the list (violates the 1..* constraint).
            // Last pet can be removed only if the client is a placeholder (dummy client).
            if (_pets.Count == 1 && _firstName != null) throw new EmptyListException("Client must have at least one pet.");

            // Throw NotFoundException if the pet is not in this client's list.
            if (!_pets.Contains(pet)) throw new NotFoundException("Pet not found.");

            _pets.Remove(pet);

            // If the pet references this client, reset it to a new client.
            if (pet.GetClient().Equals(this)) pet.ModifyClient(new Client());
        }

        /// <summary>
        /// Removes this client along with all associated pets from the system.
        /// </summary>
        public void RemoveClient()
        {
            // Throw NotFoundException if this client is not in the global extent.
            if (!_extent.Contains(this)) 
                throw new NotFoundException("Client not found in the list.");

            // Copy the list of pets to avoid concurrent modification.
            var pets = new List<Pet>(_pets);
            
            // Remove each pet from this client.
            foreach (var pet in pets)
            {
                try
                {
                    pet.RemovePet();
                }
                // If the pet list of client is empty, remove the last pet from the client's list.
                catch (EmptyListException)
                {
                    _pets.Remove(pet);
                    pet.RemovePet();
                }
            }
            _extent.Remove(this);
        }

        /// <summary>
        /// Constructs a Client with specified details and an initial list of Pets, enforcing no duplicates and at least one pet.
        /// Ensures that each pet in the list is associated with this client.
        /// </summary>
        public Client(string firstName, string lastName, string phoneNumber, string email, List<Pet> pets)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;

            // Throw DuplicatesException if the pet list contains duplicates.
            if (pets.Count != pets.Distinct().Count()) 
                throw new DuplicatesException("Pets list contains duplicates.");

            // Throw EmptyListException if the pet list is empty.
            if (pets.Count == 0) 
                throw new EmptyListException("Client must have at least one pet.");

            // Creates association between this client and each pet in the list.
            foreach (var pet in pets)
            {
                pet.ModifyClient(this);
            }

            _extent.Add(this);
            AddToExtent(this);
        }

        public Client() { }

        public override string ToString()
        {
            return $"Id={Id}, FirstName={FirstName}, LastName={LastName}, PhoneNumber={PhoneNumber}, Email={Email}";
        }
        
    }
}

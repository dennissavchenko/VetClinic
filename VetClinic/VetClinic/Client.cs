namespace VetClinic;

public class Client :StoredObject<Client>,IIdentifiable
{

    public int Id { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public Client() {}

    public Client(String firstname, String lastname, string phonenumber, string email) 
    {
        FirstName = firstname;
        LastName = lastname;
        PhoneNumber = phonenumber; 
        Email = email;
        AddToExtent(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, FirstName={FirstName}, LastName={LastName}, PhoneNumber={PhoneNumber}, Email={Email}";
    }



}


namespace VetClinic;
public class User : StoredObject<User>, IIdentifiable
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public User(string username, string email)
    {
        Username = username;
        Email = email;
    }
}
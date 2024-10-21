using VetClinic;

Person person1 = new Person("John", "Doe");
User user1 = new User("jane123", "jane123@gmail.com");

Person.AddToExtent(person1);
User.AddToExtent(user1);

foreach (var person in Person.GetExtent())
{
    Console.WriteLine(person.Id + " " + person.FirstName + " " + person.LastName);
}

foreach (var user in User.GetExtent())
{
    Console.WriteLine(user.Id + " " + user.Username + " " + user.Email);
}


namespace VetClinic.Exceptions;

public class ForbiddenRemovalException : Exception
{
    public ForbiddenRemovalException(string message) : base(message)
    {
        
    }
    
}
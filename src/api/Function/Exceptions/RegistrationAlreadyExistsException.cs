namespace api.Exceptions;

[Serializable]
public class RegistrationAlreadyExistsException : Exception
{
    public RegistrationAlreadyExistsException() { }
    public RegistrationAlreadyExistsException(string email) : base($"Entity with email {email} already exists") { }
    public RegistrationAlreadyExistsException(string email, Exception inner) : base($"Entity with email {email} already exists", inner) { }
}
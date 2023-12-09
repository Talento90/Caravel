namespace Caravel.Exceptions;

/// <summary>
/// CaravelException is the base application exception.
/// All exceptions should extend this class in order to provide consistency in the application.
/// </summary>
public class CaravelException : Exception
{
    public CaravelException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}
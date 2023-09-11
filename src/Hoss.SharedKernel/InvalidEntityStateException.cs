namespace Hoss.SharedKernel;

/// <summary>
/// </summary>
[Serializable]
public class InvalidEntityStateException : Exception
{
    /// <summary>
    /// </summary>
    public InvalidEntityStateException()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public InvalidEntityStateException(string? message)
        : base(message)
    {
    }
}
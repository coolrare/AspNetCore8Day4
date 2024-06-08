namespace AspNetCore8Day4.Exceptions;

[Serializable]
internal class GetDataException : Exception
{
    public GetDataException()
    {
    }

    public GetDataException(string? message) : base(message)
    {
    }

    public GetDataException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
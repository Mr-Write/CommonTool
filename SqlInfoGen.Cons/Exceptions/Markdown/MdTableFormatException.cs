namespace SqlInfoGen.Cons.Exceptions.Markdown;

public class MdTableFormatException:Exception
{
    public MdTableFormatException()
    {
    }

    public MdTableFormatException(string message) : base(message)
    {
    }

    public MdTableFormatException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
namespace SqlInfoGen.Cons.Exceptions.Markdown;

public class MdTableFormatException:Exception
{
    public MdTableFormatException()
    {
    }

    public MdTableFormatException(string message = "md 表格格式化错误") : base(message)
    {
    }

    public MdTableFormatException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
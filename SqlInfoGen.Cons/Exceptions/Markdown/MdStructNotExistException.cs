namespace SqlInfoGen.Cons.Exceptions.Markdown;

public class MdStructNotExistException(string message = "无法解析 Md 未定义（未支持）的结构") : Exception(message);
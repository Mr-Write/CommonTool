namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public abstract class MdH(string content) : MdStruct
{
    public string Content
    {
        set => content = value;
    }
    
    /// <summary>
    /// 前缀标识符
    /// </summary>
    protected abstract string PrefixIdentifier { get; }

    /// <summary>
    /// 渲染后的内容
    /// </summary>
    public string RenderContent => $"{PrefixIdentifier} {content}";

    /// <summary>
    /// 原始文本内容
    /// </summary>
    public string TextContent => content;
}
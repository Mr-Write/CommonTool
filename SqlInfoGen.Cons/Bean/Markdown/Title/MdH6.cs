using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public sealed class MdH6(string content) : MdH(content)
{
    public override MdStructType Type => MdStructType.H6;
    protected override string PrefixIdentifier => "######";
}
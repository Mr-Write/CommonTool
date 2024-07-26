using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public sealed class MdH1(string content) : MdH(content)
{
    public override MdStructType Type => MdStructType.H1;
    protected override string PrefixIdentifier => "#";
}
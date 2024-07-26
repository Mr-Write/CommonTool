using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public sealed class MdH4(string content) : MdH(content)
{
    public override MdStructType Type => MdStructType.H4;
    protected override string PrefixIdentifier => "####";
}
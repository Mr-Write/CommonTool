using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public sealed class MdH2(string content) : MdH(content)
{
    public override MdStructType Type => MdStructType.H2;
    protected override string PrefixIdentifier => "##";
}
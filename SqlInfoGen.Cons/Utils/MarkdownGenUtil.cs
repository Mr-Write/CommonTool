using System.Data;
using System.Text;
using SqlInfoGen.Cons.Bean;

namespace SqlInfoGen.Cons.Utils;

public static class MarkdownGenUtil
{
    private static readonly string SEP_ROW = " ------ |";
    private static readonly string SEP = "|";
    private static readonly string NULL = "null";

    public static string GenMarkdown(TableConfigBean bean, DataTable dataTable,
        Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        // 生成 markdown
        var markdown = new StringBuilder();

        // 生成一级标题
        markdown.AppendLine($"# {bean.Table}");

        // 生成数据的二级标题
        if (bean.NeedTableSchema)
        {
            markdown.AppendLine("## 数据");
        }

        // 生成 data 的 markdown 表格
        markdown.AppendLine(ConvertDataTable(bean, dataTable));

        // 生成表结构的表格
        if (bean.NeedTableSchema)
        {
            // 生成数据的二级标题
            markdown.AppendLine("## 数据");
            markdown.AppendLine(ConvertSchemaTable(bean, tableFieldInfos));
        }
        
        return markdown.ToString();
    }

    /// <summary>
    /// 生成数据的表格
    /// </summary>
    /// <param name="tableBean"></param>
    /// <param name="dataTable"></param>
    private static string ConvertDataTable(TableConfigBean tableBean, DataTable dataTable)
    {
        var markdown = new StringBuilder();

        // 生成表头
        markdown.Append(InitTableHead(tableBean.Fields!.Select(f=>$"{f.Name}{(string.IsNullOrWhiteSpace(f.Alias) ? "" : $"({f.Alias})")}").ToList()));
        // 生成分割符
        markdown.Append(InitSepRow(tableBean.Fields!.Count));
        // 生成表格内容
        foreach (DataRow dataRow in dataTable.Rows)
        {
            markdown.Append(InitTableRow(tableBean.Fields!.Select(f=>Convert.ToString(dataRow[f.Name]) ?? NULL).ToList()));
        }
        
        return markdown.ToString();
    }

    /// <summary>
    /// 生成表结构的表格
    /// </summary>
    /// <param name="tableBean"></param>
    /// <param name="tableFieldInfos"></param>
    private static string ConvertSchemaTable(TableConfigBean tableBean,
        Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        var markdown = new StringBuilder();

        // 生成表头
        markdown.Append(InitTableHead(["Field", "Type", "Comment"]));
        // 生成分割符
        markdown.Append(InitSepRow(3));
        // 生成表格内容
        foreach (var field in tableBean.Fields!)
        {
            var fieldInfo = tableFieldInfos[field.Name];
            markdown.Append(InitTableRow([fieldInfo.Field, fieldInfo.Type, fieldInfo.Comment]));
        }

        return markdown.ToString();
    }

    /// <summary>
    /// 初始化表头
    /// </summary>
    /// <remarks>
    /// <para>
    /// 后续可能对表头进行特殊处理，以区别普通行
    /// </para>
    /// </remarks>
    /// <param name="heads"></param>
    /// <returns></returns>
    private static string InitTableHead(List<string> heads)
    {
       return InitTableRow(heads);
    }
    
    /// <summary>
    /// 初始化普通行
    /// </summary>
    /// <param name="spans"></param>
    /// <returns></returns>
    private static string InitTableRow(List<string> spans)
    {
        var builder = new StringBuilder(SEP);
        foreach (var span in spans)
        {
            builder.Append(span)
                .Append(SEP);
        }

        builder.Append("\n");
        return builder.ToString();
    }

    /// <summary>
    /// 生成分割行
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private static string InitSepRow(int size)
    {
        var builder = new StringBuilder(SEP);
        for (var i = 0; i < size; i++)
        {
            builder.Append(SEP_ROW);
        }

        builder.Append("\n");
        return builder.ToString();
    }
}
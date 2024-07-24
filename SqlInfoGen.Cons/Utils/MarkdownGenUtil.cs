using System.Data;
using System.Text;
using SqlInfoGen.Cons.Bean;

namespace SqlInfoGen.Cons.Utils;

public static class MarkdownGenUtil
{
    private static readonly string SEP_ROW = " ------ |";
    private static readonly string SEP = "|";
    private static readonly string NULL = "null";

    public static string GenMarkdown(TableConfigBean bean, DataTable dataTable)
    {
        // 生成 markdown
        var markdown = new StringBuilder();
        
        // 生成标题
        markdown.AppendLine($"# {bean.Table}");
                
        // 生成 data 的 markdown 表格
        markdown.Append(ConvertDataTable(bean, dataTable));

        return markdown.ToString();
    }
    
    /// <summary>
    /// 生成数据的表格
    /// </summary>
    /// <param name="bean"></param>
    /// <param name="dataTable"></param>
    private static string ConvertDataTable(TableConfigBean bean, DataTable dataTable )
    {
        var markdown = new StringBuilder();
        
        // 生成表头
        markdown.Append(InitTableHead(bean.Fields!));
        // 生成分割符
        markdown.Append(InitSepRow(bean.Fields!.Count));
        // 生成表格内容
        foreach (DataRow dataRow in dataTable.Rows)
        {
            var row = new StringBuilder(SEP);
            foreach (var field in bean.Fields)
            {
                row.Append(Convert.ToString(dataRow[field.Name]) ?? NULL).Append(SEP);
            }

            row.Append("\n");
            markdown.Append(row);
        }

        return markdown.ToString();
    }
    
    private static void ConvertSchemaTable(TableConfigBean bean, Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        
    }

    /// <summary>
    /// 初始化表头
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    private static string InitTableHead(List<Field> fields)
    {
        var builder = new StringBuilder(SEP);
        foreach (var field in fields)
        {
            builder.Append($"{field.Name}{(string.IsNullOrWhiteSpace(field.Alias) ? "" : $"({field.Alias})")}")
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
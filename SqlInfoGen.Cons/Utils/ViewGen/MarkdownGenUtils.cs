using System.Data;
using System.Text;
using NUnit.Framework;
using SqlInfoGen.Cons.Bean.Config;
using SqlInfoGen.Cons.Bean.Markdown;
using SqlInfoGen.Cons.Bean.Markdown.Title;
using SqlInfoGen.Cons.Exceptions.Markdown;
using static SqlInfoGen.Cons.Enums.MdStructType;

namespace SqlInfoGen.Cons.Utils.ViewGen;

public static class MarkdownGenUtils
{
    private static readonly string SepRow = " ------ |";
    private const string Sep = "|";
    private const string Null = "null";

    public static string GenMarkdown(TableConfigBean tableBean, DataTable dataTable,
        Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        var list = new List<MdStruct>();
        
        // 生成一级标题
        list.Add(new MdH1(tableBean.Table));

        // 生成数据的二级标题
        if (tableBean.NeedTableSchema)
        {
            list.Add(new MdH2("数据"));
        }

        // 生成 data 的 markdown 表格
        list.Add(new MdTable()
        {
            Heads = tableBean.Fields!
                .Select(f => $"{f.Name}{(string.IsNullOrWhiteSpace(f.Alias) ? "" : $"({f.Alias})")}").ToList(),
            Rows = dataTable.Rows.Cast<DataRow>()
                .Select(r => tableBean.Fields!.Select(f => Convert.ToString(r[f.Name]) ?? Null).ToList()).ToList()
        });

        // 生成表结构的表格
        if (tableBean.NeedTableSchema)
        {
            // 生成数据的二级标题
            list.Add(new MdH2("结构"));
            list.Add(new MdTable()
            {
                Heads = ["Field", "Type", "Comment"],
                Rows = tableBean.Fields!.Select(f =>
                {
                    var fieldInfo = tableFieldInfos[f.Name];
                    return new List<string> { fieldInfo.Field, fieldInfo.Type, fieldInfo.Comment };
                }).ToList()
            });
        }

        return RenderMd(list);
    }

    private static string RenderMd(List<MdStruct> list)
    {
        var markdown = new StringBuilder();
        foreach (var mdStruct in list)
        {
            markdown.AppendLine(mdStruct.Type switch
            {
                H1 or H2 or H3 or H4 or H5 or H6 => (mdStruct as MdH)!.RenderContent,
                Table => ReaderTable((mdStruct as MdTable)!),
                _ => throw new MdStructNotExistException()
            });
        }

        return markdown.ToString();
    }

    /// <summary>
    /// 渲染 markdown table 表格
    /// </summary>
    /// <param name="table"></param>
    private static string ReaderTable(MdTable table)
    {
        // 检测 table 正确性
        if (table.Heads == null || table.Heads.Count == 0)
        {
            throw new MdTableFormatException("table 无表头格式");
        }

        if (table.Rows != null && table.Rows.Any(r => r.Count != table.Heads.Count))
        {
            throw new MdTableFormatException("table 表头与普通行列数不一致");
        }

        var tableBuilder = new StringBuilder();

        // 生成表头
        tableBuilder.Append(InitTableHead(table.Heads));
        // 生成表格内容
        if (table.Rows != null)
        {
            tableBuilder.Append(InitTableRows(table.Rows));
        }
        return tableBuilder.ToString();
    }

    /// <summary>
    /// 初始化表头
    /// </summary>
    /// <param name="headRow"></param>
    /// <returns></returns>
    private static string InitTableHead(List<string> headRow)
    {
        var headRowBuilder = new StringBuilder();
        headRowBuilder.Append(Sep);
        foreach (var span in headRow)
        {
            headRowBuilder.Append(span).Append(Sep);
        }
        headRowBuilder.Append("\n");
        for (var i = 0; i < headRow.Count; i++)
        {
            headRowBuilder.Append(SepRow);
        }

        headRowBuilder.Append("\n");
        return headRowBuilder.ToString();
    }

    /// <summary>
    /// 初始化普通行
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    private static string InitTableRows(List<List<string>> rows)
    {
        var rowsBuilder = new StringBuilder();
        foreach (var row in rows)
        {
            rowsBuilder.Append(Sep);
            foreach (var span in row)
            {
                rowsBuilder.Append(span).Append(Sep);
            }

            rowsBuilder.Append("\n");
        }

        return rowsBuilder.ToString();
    }
}
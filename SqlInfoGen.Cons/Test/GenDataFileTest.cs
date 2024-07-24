using System.Data;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Common;
using SqlInfoGen.Cons.Enums;
using SqlInfoGen.Cons.Helpers;
using SqlInfoGen.Cons.Utils;

namespace SqlInfoGen.Cons.Test;

public class GenDataFileTest : DbTest
{
    [Test]
    public async Task TestGenDataFile()
    {
        var dbBeans = ConfigUtil.GetDbConfigBeanList();
        foreach (var dbBean in dbBeans)
        {
            // 配置输出文件信息
            SetDbFileInfo(dbBean);
            // 检查是否遵循规范
            var dbErrorInfo = CheckOutputFileFollowRule(dbBean);
            if (dbErrorInfo is not null)
            {
                Console.WriteLine($"{dbBean.ReadFilePath} 中存在错误信息: {dbErrorInfo}");
                continue;
            }
            var tableBeans = JsonToObjUtil.GetTableConfigBeanList(dbBean.ReadFilePath);
            // 检查输出目录是否存在
            if (!Directory.Exists(dbBean.OutputDir))
            {
                Directory.CreateDirectory(dbBean.OutputDir);
            }
            // 输出流
            await using var writer = new StreamWriter(dbBean.OutputFilePath);
            foreach (var tableBean in tableBeans)
            {
                Console.WriteLine(JsonSerializer.Serialize(tableBean));

                // 获取表结构数据
                var tableFieldInfos = DbHelper.GetTableSchema(tableBean.Table);
                // 检查是否遵循规则
                var errorInfo = CheckTableFollowRule(tableBean, tableFieldInfos);
                if (errorInfo is not null)
                {
                    Console.WriteLine($"{dbBean.ReadFilePath} 中存在错误信息: {errorInfo}");
                    continue;
                }

                // 如果需要所有字段，则修改 bean 的 Fields
                if (tableBean.NeedAllFields)
                {
                    tableBean.Fields = tableFieldInfos.Values.OrderBy(f => f.Order).Select(f => new Field
                    {
                        Name = f.Field,
                        // 将配置的别名赋给 Field
                        Alias = tableBean.Fields?.FirstOrDefault(r => r.Name == f.Field)?.Alias
                    }).ToList();
                }

                // 生成 sql
                var sql = GenSql(tableBean);

                // 执行 sql
                var dataTable = DbHelper.GetDataTable(sql);

                // 生成该表的内容
                var tableCxt = GenFileContent(dbBean, tableBean, dataTable);
                await writer.WriteAsync(tableCxt);
            }
        }
    }

    private static string GenFileContent(DbConfigBean dbBean, TableConfigBean tableBean, DataTable dataTable)
    {
        switch (dbBean.OutputFileNameSuffixEnum)
        {
            case FileSuffixEnum.Md:
                return MarkdownGenUtil.GenMarkdown(tableBean, dataTable);
            default:
                return string.Empty;
        }
    }

    private static void SetDbFileInfo(DbConfigBean dbBean)
    {
        if (dbBean.OutputDir == ConfigCommon.DefaultName || string.IsNullOrWhiteSpace(dbBean.OutputDir))
        {
            dbBean.OutputDir = ConfigCommon.DefaultOutputDir;
        }

        int index = dbBean.ReadFileName.LastIndexOf(".", StringComparison.Ordinal);
        // 设置 db 名称为文件名
        dbBean.Db = index == -1 ? dbBean.ReadFileName : dbBean.ReadFileName.Substring(0, index);
        if (dbBean.OutputFileName == ConfigCommon.DefaultName || string.IsNullOrWhiteSpace(dbBean.OutputFileName))
        {
            dbBean.OutputFileName = $"{ConfigCommon.DefaultOutputFileNamePrefix}{dbBean.Db}";
        }

        if (dbBean.OutputFileNameSuffix == ConfigCommon.DefaultName ||
            string.IsNullOrWhiteSpace(dbBean.OutputFileNameSuffix))
        {
            dbBean.OutputFileNameSuffix = ConfigCommon.DefaultOutputFileNameSuffix;
        }

        dbBean.ReadFilePath = Path.Combine(Path.Combine(dbBean.ReadDirLevels.ToArray()), dbBean.ReadFileName);
        dbBean.OutputFilePath =
            $"{dbBean.OutputDir}{Path.DirectorySeparatorChar}{dbBean.OutputFileName}.{dbBean.OutputFileNameSuffix}";
    }

    private static string GenSql(TableConfigBean bean)
    {
        var builder = new StringBuilder();
        builder.Append("select ");
        // 配置查询的字段
        builder.Append(string.Join(",", bean.Fields!.Select(f=>f.Name)));
        // 配置查询的表
        builder.Append($" from {bean.Table} ");
        // 配置查询条件
        if (bean.SelectConditions is not null && bean.SelectConditions.Count > 0)
        {
            builder.Append(
                $" where {string.Join(" and ", bean.SelectConditions.Select(condition => $" ({condition}) "))}");
        }

        // 配置排序规则
        if (bean.OrderByConditions is not null && bean.OrderByConditions.Count > 0)
        {
            builder.Append($" order by {string.Join(", ", bean.OrderByConditions)}");
        }

        // 配置返回数量
        builder.Append($" limit {bean.Limit.Offset}, {bean.Limit.Count}");

        return builder.ToString();
    }

    /// <summary>
    /// 检查输出文件配置是否正确
    /// </summary>
    /// <returns></returns>
    private static string? CheckOutputFileFollowRule(DbConfigBean bean)
    {
        // 检查文件后缀是否提供支持
        var enumByDescription = EnumExtensions.ParseEnumByDescription<FileSuffixEnum>(bean.OutputFileNameSuffix);
        if (enumByDescription == null)
        {
            return $"OutputFileNameSuffix 为 [{bean.OutputFileNameSuffix}] 暂不支持生成";
        }

        bean.OutputFileNameSuffixEnum = enumByDescription.Value;

        return null;
    }

    /// <summary>
    /// 检查表配置是否正确
    /// </summary>
    /// <param name="bean"></param>
    /// <param name="tableFieldInfos"></param>
    /// <returns></returns>
    private static string? CheckTableFollowRule(TableConfigBean bean,
        Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        // 检查是否配置了 table
        if (string.IsNullOrWhiteSpace(bean.Table))
        {
            return "Table 不能为空";
        }

        // 如果没有配置了 NeedAllFields，则必须配置 Fields
        if (!bean.NeedAllFields && (bean.Fields == null || bean.Fields.Count == 0))
        {
            return "NeedAllFields 为 false 时，必须配置 Fields";
        }

        // 检查配置查询的字段是否都存在
        if (!bean.NeedAllFields)
        {
            var notExistsFields = new List<string>();
            foreach (var field in bean.Fields!)
            {
                if (!tableFieldInfos.ContainsKey(field.Name))
                {
                    notExistsFields.Add(field.Name);
                }
            }

            if (notExistsFields.Count > 0)
            {
                return $"{bean.Table} 中不存在的字段: {string.Join(",", notExistsFields)}。请仔细检查配置。";
            }
        }

        // 检查配置的 limit
        if (bean.Limit.Offset < 0 || bean.Limit.Count <= 0)
        {
            return "Limit 的 Offset 必须大于等于 0，Count 必须大于 0";
        }


        return null;
    }
}
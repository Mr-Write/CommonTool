using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Bean.Config;
using SqlInfoGen.Cons.Utils;

namespace SqlInfoGen.Cons.Helpers;

public static class DbHelper
{
    private static readonly string? ConnectionString;

    static DbHelper()
    {
        ConnectionString = ConfigUtils.GetMySqlConnectionString();
    }

    private static MySqlCommand PrepareCommand(MySqlConnection conn, string sql, CommandType cmdType,
        params MySqlParameter[]? parameters)
    {
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.CommandType = cmdType;
        if (parameters is { Length: > 0 })
        {
            cmd.Parameters.AddRange(parameters);
        }

        return cmd;
    }

    /**
     * 执行增删改的操作
     */
    public static int ExecuteNonQuery(string sql, CommandType cmdType =
        CommandType.Text, params MySqlParameter[]? parameters)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = PrepareCommand(conn, sql, cmdType, parameters);
        return cmd.ExecuteNonQuery();
    }

    public static object? ExecuteScalar(string sql, CommandType cmdType =
        CommandType.Text, params MySqlParameter[]? parameters)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = PrepareCommand(conn, sql, cmdType, parameters);
        return cmd.ExecuteScalar();
    }

    public static DataTable GetDataTable(string sql, CommandType cmdType =
        CommandType.Text, params MySqlParameter[]? parameters)
    {
        using MySqlDataAdapter adapter = new(sql, new MySqlConnection(ConnectionString));
        adapter.SelectCommand!.CommandType = cmdType;
        if (parameters is { Length: > 0 })
        {
            adapter.SelectCommand.Parameters.AddRange(parameters);
        }

        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);
        return dataTable;
    }

    public static Dictionary<string, TableFieldInfo> GetTableSchema(string tableName)
    {
        string sql = $"SHOW FULL COLUMNS FROM {tableName}";
        var dataTable = GetDataTable(sql);
        var dict = new Dictionary<string, TableFieldInfo>();
        int order = 0;
        foreach (DataRow row in dataTable.Rows)
        {
            var field = row["Field"].ToString()!;
            dict.Add(field, new()
            {
                Field = field,
                Type = row["Type"].ToString()!,
                Comment = row["Comment"].ToString(),
                Order = order++
            });
        }

        return dict;
    }
}
using System.Data;
using NUnit.Framework;

namespace SqlInfoGen.Cons.Extensions;

public static class SelectExtensions
{
    public static List<V> Select<V>(this DataTable dataTable, Func<DataRow, V> func){
        return dataTable.Rows.Cast<DataRow>().Select(func).ToList();
    }
}
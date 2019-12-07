using System;
using System.Collections.Generic;

namespace Mono.Data.Sqlite
{
    //public sealed class SqliteDataTable
    //{
    //    private readonly SqliteManager manager;

    //    private readonly SQLiteField[] fields;

    //    public SqliteTable(SqliteManager database, string tableName)
    //    {
    //        manager = database;
    //        Name = tableName;

    //    }

    //    public string Name { get; }

    //    public void SetTable()
    //    {
    //        // 
    //        var fieldList = new List<SQLiteField<>>();
    //        using (var info = manager.ExecuteReader($"PRAGMA table_info({Name})"))
    //        {
    //            new SQLiteField<int>
    //            var type = (SQLiteDataType)Enum.Parse(typeof(SQLiteDataType), info[2].ToString(), false);
    //            switch (type)
    //            {
    //                case SQLiteDataType.INTEGER:
    //                    //fieldList.Add(new SQLiteField<long>(
    //                    //    info[1].ToString(),
    //                    //    bool.Parse(info[3].ToString()),
    //                    //    int.Parse(info[4].ToString()),
    //                    //    bool.Parse(info[5].ToString())
    //                    //    ));
    //                    break;
    //                case SQLiteDataType.REAL:
    //                    break;
    //                case SQLiteDataType.TEXT:
    //                    break;
    //                case SQLiteDataType.BLOB:
    //                    break;
    //            }
    //            if ()
    //                T
    //                    info.Read();
    //        }
    //    }
    //}
}



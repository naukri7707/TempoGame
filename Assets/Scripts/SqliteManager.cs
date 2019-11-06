using System.Text;
using UnityEngine;

namespace Mono.Data.Sqlite
{
    public sealed class SqliteManager
    {
        /// <summary>
        /// 建構資料庫連結
        /// </summary>
        /// <param name="path">資料庫(.db)在 Application.persistentDataPath 下的相對路徑</param>
        public SqliteManager(string path)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
            var databasePath = $"data source={Application.persistentDataPath}/{path}";
#elif UNITY_ANDROID
            var databasePath = $"URL=file:{Application.persistentDataPath}/{conn}";
#endif
            connection = new SqliteConnection(databasePath);
            connection.Open();
        }

        /// <summary>
        /// 建構資料庫連結並指定資料表
        /// </summary>
        /// <param name="path">資料庫(.db)在 Application.persistentDataPath 下的相對路徑</param>
        /// <param name="table">目標資料表</param>
        public SqliteManager(string path, string table) : this(path)
        {
            Table = table;
        }

        /// <summary>
        /// 解構時關閉連線
        /// </summary>
        ~SqliteManager()
        {
            connection.Close();
        }

        /// <summary>
        /// 連結工具
        /// </summary>
        private readonly SqliteConnection connection;

        /// <summary>
        /// 當前資料表
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 執行非查詢指令
        /// </summary>
        /// <param name="sqlQuery">SQL指令</param>
        /// <returns>受影響的資料筆數</returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 執行查詢指令
        /// </summary>
        /// <param name="sqlQuery">SQL指令</param>
        /// <returns>查詢結果</returns>
        public SqliteDataReader ExecuteQuery(string sqlQuery)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sqlQuery;
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 生成資料表
        /// </summary>
        /// <param name="tableName">資料表名稱</param>
        /// <param name="fields">欄位細節</param>
        public void CreateTable(string tableName, params SQLiteField[] fields)
        {
            if (fields.Length == 0)
            {
                throw new SqliteException("No Columns!");
            }
            var query = new StringBuilder($"CREATE TABLE IF NOT EXISTS '{tableName}' (");
            foreach (var f in fields)
            {
                query.Append($"'{f.Name}' {f.Type},");
            }
            query.Length--;
            query.Append(");");
            ExecuteNonQuery(query.ToString());
        }

        #region -- Insert --

        /// <summary>
        /// 插入 依序寫入完整資料
        /// </summary>
        /// <param name="values">完整數值</param>
        /// <returns>受影響的資料筆數</returns>
        public int Insert(params object[] values)
        {
            var query = new StringBuilder($"INSERT INTO {Table} VALUES (");
            foreach (var val in values)
            {
                query.Append($"{FormatValueToSQL(val)}, ");
            }
            query.Length--;
            query.Append(");");
            Debug.Log(query);
            return ExecuteNonQuery(query.ToString());
        }

        /// <summary>
        /// 插入 單筆資料至對應欄位
        /// </summary>
        /// <param name="field">欄位</param>
        /// <param name="value">數值</param>
        /// <returns>受影響的資料筆數</returns>
        public int Insert(string field, object value)
        {
            return ExecuteNonQuery($"INSERT INTO {Table} ({field}) VALUES ({FormatValueToSQL(value)});");
        }

        /// <summary>
        /// 插入 多筆資料至欄位
        /// </summary>
        /// <param name="fields">欄位</param>
        /// <param name="values">數值</param>
        /// <returns>受影響的資料筆數</returns>
        public int Insert(string[] fields, params object[] values)
        {
            if (fields.Length != values.Length)
            {
                throw new SqliteException("field.Length != values.Length");
            }
            var query = new StringBuilder($"INSERT INTO {Table} (");
            foreach (var f in fields)
            {
                query.Append($"{f},");
            }
            query.Length--;
            query.Append(") VALUES (");
            foreach (var v in values)
            {
                query.Append($"{FormatValueToSQL(v)},");
            }
            query.Length--;
            query.Append(");");
            return ExecuteNonQuery(query.ToString());
        }

        /// <summary>
        /// 插入 多筆資料至對應欄位
        /// </summary>
        /// <param name="datas">資料</param>
        /// <returns>受影響的資料筆數</returns>
        public int Insert(params SQLiteDataPair[] datas)
        {
            var query = new StringBuilder($"INSERT INTO {Table} (");
            foreach (var d in datas)
            {
                query.Append($"{d.Field},");
            }
            query.Length--;
            query.Append(") VALUES (");
            foreach (var d in datas)
            {
                query.Append($"{FormatValueToSQL(d.Value)},");
            }
            query.Length--;
            query.Append(");");
            return ExecuteNonQuery(query.ToString());
        }

        #endregion

        #region -- Update --

        /// <summary>
        /// 更新 數值至符合條件的欄位
        /// </summary>
        /// <param name="condition">條件</param>
        /// <param name="field">欄位</param>
        /// <param name="value">數值</param>
        /// <returns></returns>
        public int Update(string condition, string field, object value)
        {
            return ExecuteNonQuery($"UPDATE {Table} SET {field} = {FormatValueToSQL(value)} {(condition == "" ? ";" : $"WHERE {condition};")}");
        }

        /// <summary>
        /// 更新 數值至符合條件的多個欄位
        /// </summary>
        /// <param name="condition">條件</param>
        /// <param name="fields">欄位</param>
        /// <param name="values">數值</param>
        /// <returns></returns>
        public int Update(string condition, string[] fields, params object[] values)
        {
            if (fields.Length != values.Length)
            {
                throw new SqliteException("field.Length != values.Length");
            }
            var query = new StringBuilder($"UPDATE {Table} SET");
            for (int i = 0; i < fields.Length; i++)
            {
                query.Append($" {fields[i]} = {FormatValueToSQL(values[i])},");
            }
            query.Length--;
            query.Append(condition == "" ? ";" : $" WHERE {condition};");
            return ExecuteNonQuery(query.ToString());
        }

        /// <summary>
        /// 插入 多筆資料至對應欄位
        /// </summary>
        /// <param name="datas">資料</param>
        /// <returns>受影響的資料筆數</returns>
        public int Update(string condition, params SQLiteDataPair[] datas)
        {
            var query = new StringBuilder($"UPDATE {Table} SET");
            foreach (var d in datas)
            {
                query.Append($" {d.Field} = {FormatValueToSQL(d.Value)},");
            }
            query.Length--;
            query.Append(condition == "" ? ";" : $" WHERE {condition};");
            return ExecuteNonQuery(query.ToString());
        }

        #endregion

        #region -- Delete --

        /// <summary>
        /// 刪除 符合條件的資料
        /// </summary>
        /// <param name="condition">條件</param>
        /// <returns></returns>
        public int Delete(string condition)
        {
            return ExecuteNonQuery($"DELETE FROM {Table} {(condition == "" ? ";" : $"WHERE {condition};")}");
        }

        #endregion

        #region -- Select --

        /// <summary>
        /// 查詢 指定欄位
        /// </summary>
        /// <param name="field">欄位</param>
        /// <returns>查詢結果</returns>
        public SqliteDataReader Select(string field)
        {
            return ExecuteQuery($"SELECT {field} FROM {Table}");
        }

        /// <summary>
        /// 查詢 多個指定欄位
        /// </summary>
        /// <param name="fields">欄位</param>
        /// <returns>查詢結果</returns>
        public SqliteDataReader Select(params string[] fields)
        {
            var query = new StringBuilder("SELECT ");
            foreach (var f in fields)
            {
                query.Append($"{f},");
            }
            query.Length--;
            query.Append($" FROM {Table}");
            return ExecuteQuery(query.ToString());
        }

        /// <summary>
        /// 查詢 所有欄位
        /// </summary>
        /// <returns>查詢結果</returns>
        public SqliteDataReader SelectAll()
        {
            return ExecuteQuery($"SELECT * FROM {Table}");
        }

        #endregion

        /// <summary>
        /// 將資料轉換成SQL字串
        /// </summary>
        /// <param name="obj">資料</param>
        /// <returns>SQL字串</returns>
        private string FormatValueToSQL(object obj)
        {
            if (obj is string)
                return $"'{obj}'";
            else if (obj is System.Enum)
                return ((int)obj).ToString();
            else
                return obj.ToString();
        }
    }

    /// <summary>
    /// SQLite 資料型態
    /// </summary>
    public enum SQLiteDataType
    {
        INTEGER,
        REAL,
        TEXT,
        BLOB
    }

    /// <summary>
    /// SQLite 欄位
    /// </summary>
    public struct SQLiteField
    {
        /// <summary>
        /// 產生一個帶有 name 和 type 的 SQLiteField
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="type">型態</param>
        public SQLiteField(string name, SQLiteDataType type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型態
        /// </summary>
        public SQLiteDataType Type { get; set; }
    }

    /// <summary>
    /// SQLite 資料對
    /// </summary>
    public struct SQLiteDataPair
    {
        /// <summary>
        /// 產生一個帶有 field 和 value 的 SQLiteDataPair
        /// </summary>
        /// <param name="field">欄位</param>
        /// <param name="value">資料</param>
        public SQLiteDataPair(string field, object value)
        {
            Field = field;
            Value = value;
        }

        /// <summary>
        /// 欄位
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 數值
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// SQLite 擴充函式
    /// </summary>
    public static class SqliteExtensionsMethod
    {
        /// <summary>
        /// 取得轉型後的資料
        /// </summary>
        /// <typeparam name="T">目標型態</typeparam>
        /// <param name="reader">擴充對象</param>
        /// <param name="index">欄位索引</param>
        /// <returns>轉型後的資料</returns>
        public static T Get<T>(this SqliteDataReader reader, int index) where T : System.IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return (T)System.Convert.ChangeType(reader.GetValue(index), typeof(int));
            }
            return (T)reader.GetValue(index);
        }

        /// <summary>
        /// 取得轉型後的資料
        /// </summary>
        /// <typeparam name="T">目標型態</typeparam>
        /// <param name="reader">擴充對象</param>
        /// <param name="name">欄位名稱</param>
        /// <returns>轉型後的資料</returns>
        public static T Get<T>(this SqliteDataReader reader, string name) where T : System.IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return (T)System.Convert.ChangeType(reader.GetValue(reader.GetOrdinal(name)), typeof(int));
            }
            return (T)reader.GetValue(reader.GetOrdinal(name));
        }
    }
}
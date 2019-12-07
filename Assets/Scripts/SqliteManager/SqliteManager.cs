using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;

namespace Mono.Data.Sqlite
{
    public sealed class SqliteManager : IDisposable
    {
        /// <summary>
        /// 連結工具
        /// </summary>
        private SqliteConnection connection;

        /// <summary>
        /// 建構資料庫連結
        /// </summary>
        /// <param name="path">資料庫完整路徑，每個參數中間會自動插入 "/"</param>
        public SqliteManager(params string[] path)
        {
            try
            {
                var fullPath = new StringBuilder();
                foreach (var p in path)
                {
                    fullPath.Append(p);
                    fullPath.Append("/");
                }
                fullPath.Remove(fullPath.Length - 1, 1);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
                var databasePath = $@"data source={fullPath}";
#elif UNITY_ANDROID
            var databasePath = $@"URL=file:{fullPath}";
#endif
                connection = new SqliteConnection(databasePath);
                connection.Open();
            }
            catch (Exception e)
            {
                Debug.Log("連結失敗 : " + e.Message);
            }
        }

        /// <summary>
        /// 當前資料表
        /// </summary>
        public string Table { get; set; }

        private SqliteDataReader LastDataReader { get; set; }

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
        public SqliteDataReader ExecuteReader(string sqlQuery)
        {
            if (LastDataReader != null && !LastDataReader.IsClosed)
            {
                LastDataReader.Close();
            }
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlQuery;
            var res = cmd.ExecuteReader();
            LastDataReader = res;
            return res;
        }

        /// <summary>
        /// 生成資料表 (如果沒有的話)
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
                query.Append($@"{f.Name} {f.Type} {f.Suffix},");
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
                query.Append($"{val.ToSQL()}, ");
            }
            query.Length -= 2;
            query.Append(");");
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
            return ExecuteNonQuery($"INSERT INTO {Table} ({field}) VALUES ({value.ToSQL()});");
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
                query.Append($"{v.ToSQL()},");
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
                query.Append($"{d.Value.ToSQL()},");
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
            return ExecuteNonQuery($"UPDATE {Table} SET {field} = {value.ToSQL()} {(condition == "" ? ";" : $"WHERE {condition};")}");
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
                query.Append($" {fields[i]} = {values[i].ToSQL()},");
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
                query.Append($" {d.Field} = {d.Value.ToSQL()},");
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
        /// <param name="condition">條件</param>
        /// <param name="field">欄位</param>
        /// <returns>查詢結果</returns>
        public SqliteDataReader Select(string condition, string field)
        {
            return ExecuteReader($"SELECT {field} FROM {Table}{(condition == "" ? ";" : $" WHERE {condition}; ")}");
        }

        /// <summary>
        /// 查詢 多個指定欄位
        /// </summary>
        /// <param name="condition">條件</param>
        /// <param name="fields">欄位</param>
        /// <returns>查詢結果</returns>
        public SqliteDataReader Select(string condition, params string[] fields)
        {
            var query = new StringBuilder("SELECT ");
            foreach (var f in fields)
            {
                query.Append($"{f},");
            }
            query.Length--;
            query.Append($" FROM {Table}{(condition == "" ? ";" : $" WHERE {condition};")}");
            return ExecuteReader(query.ToString());
        }

        /// <summary>
        /// 查詢 所有欄位
        /// </summary>
        /// <returns>查詢結果</returns>
        public SqliteDataReader SelectAll()
        {
            return ExecuteReader($"SELECT * FROM {Table}");
        }

        /// <summary>
        /// 查詢 所有欄位
        /// </summary>
        /// /// <param name="condition">條件</param>
        /// <returns>查詢結果</returns>
        public SqliteDataReader SelectAll(string condition)
        {
            return ExecuteReader($"SELECT * FROM {Table} WHERE {condition};");
        }

        #endregion

        public void Dispose()
        {
            if (LastDataReader != null && !LastDataReader.IsClosed)
            {
                LastDataReader.Close();
            }
            connection.Close();
        }
    }
}
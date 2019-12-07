using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Data.Sqlite
{
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
        public static T Get<T>(this SqliteDataReader reader, string name) where T : IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return (T)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(name)), typeof(int));
            }
            return (T)reader.GetValue(reader.GetOrdinal(name));
        }

        /// <summary>
        /// 將資料轉換成對應的SQL字串
        /// </summary>
        /// <param name="self">資料</param>
        /// <returns>SQL字串</returns>
        public static string ToSQL(this object self)
        {
            if (self is string)
            {
                StringBuilder sb = new StringBuilder(self.ToString());
                sb.Replace("'", "''");
                return $"'{sb}'";
            }
            else
            {
                return self.ToString();
            }
        }

        /// <summary>
        /// 將資料轉換成對應的SQL字串
        /// </summary>
        /// <typeparam name="T">型態</typeparam>
        /// <param name="self">資料</param>
        /// <returns>SQL字串</returns>
        public static string ToSQL<T>(this T self) where T : IConvertible
        {
            if (self is string)
            {
                StringBuilder sb = new StringBuilder(self.ToString());
                sb.Replace("'", "''");
                return $"'{sb}'";
            }
            else
            {
                return self.ToString();
            }
        }
    }
}

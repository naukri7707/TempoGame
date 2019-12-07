using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Data.Sqlite
{
    /// <summary>
    /// SQLite 關鍵字
    /// </summary>
    public static class SqlKeyWord
    {
        /// <summary>
        /// [約束] 主鍵
        /// </summary>
        public const string PRIMARY_KEY = "PRIMARY KEY";

        /// <summary>
        /// [約束] 不可為空
        /// </summary>
        public const string NOT_NULL = "NOT NULL";

        /// <summary>
        /// [約束] 不可重複
        /// </summary>
        public const string UNIQUE = "UNIQUE";
        
        /// <summary>
        /// 自動遞增
        /// </summary>
        public const string AUTOINCREMENT = "AUTOINCREMENT";

        /// <summary>
        /// [約束] 預設值
        /// </summary>
        public static string DEFAULT<T>(T defaultValue) where T : IConvertible
        {
            return $"DEFAULT {defaultValue.ToSQL()}";
        }

        /// <summary>
        /// [約束] 檢查
        /// </summary>
        /// <param name="expression">表達式 (注意 TEXT 要加 '')</param>
        /// <returns></returns>
        public static string CHECK(string expression)
        {
            return $"CHECK({expression})";
        }
    }
}

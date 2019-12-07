using Mono.Data.Sqlite;
using Naukri.OsuAnalysis;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public static class ZipExtractor
{
    /// <summary>
    /// 解壓縮歌曲包 (.osz)
    /// </summary>
    /// <param name="src">壓縮檔絕對路徑 (包含副檔名)</param>
    /// <param name="dst">目標資料夾絕對路徑</param>
    /// <returns>解壓目錄絕對路徑</returns>
    public static void ExtractZip(string src, string dst)
    {
        if (!Directory.Exists(dst))
        {
            ZipFile.ExtractToDirectory(src, dst);
        }
        else
        {
            Debug.Log($"Folder Exists : {dst}");
        }
    }
}
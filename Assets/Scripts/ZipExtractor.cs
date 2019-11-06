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
    /// <param name="path">歌曲包絕對路徑 (包含.osz副檔名)</param>
    public static void ExtractSong(string path)
    {
        string extractPath = Path.Combine(Application.streamingAssetsPath, "Songs", Path.GetFileNameWithoutExtension(path));
        if (!Directory.Exists(extractPath))
        {
            ZipFile.ExtractToDirectory(path, extractPath);
        }
        else
        {
            Debug.Log($"Folder Exists : {extractPath}");
        }
    }

    public static void AnalysisSong(string path)
    {

    }
}
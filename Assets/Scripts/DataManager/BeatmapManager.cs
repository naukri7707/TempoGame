using Mono.Data.Sqlite;
using Naukri.OsuAnalysis;
using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public static class BeatmapManager
{

    public static string SongFolderPath { get; } = $@"{Application.streamingAssetsPath}/Songs";

    public static string PackageFolderPath { get; } = $@"{Application.streamingAssetsPath}/Packages";

    /// <summary>
    /// 從Package資料夾解壓縮歌曲包 (.osz)
    /// </summary>
    /// <returns>新增歌曲數</returns>
    public static int ExtractOszFromPackageFolder()
    {
        var res = 0;
        var newOsz = new DirectoryInfo(PackageFolderPath).GetFiles("*.osz");
        if (newOsz.Length > 0)
        {
            MessageBox.ShowLazy($"發現了{newOsz.Length}個Osz檔案，正在背景解析");
            // 產生 Song 路徑
            if (Directory.Exists(SongFolderPath) == false)
            {
                Directory.CreateDirectory(SongFolderPath);
            }
            foreach (var fi in newOsz)
            {
                res += ExtractOsz(fi.FullName);
            }
        }
        return res;
    }

    /// <summary>
    /// 解壓縮歌曲包 (.osz)
    /// </summary>
    /// <param name="path">歌曲包絕對路徑 (包含.osz副檔名)</param>
    /// <returns>新增歌曲數</returns>
    private static int ExtractOsz(string path)
    {
        var res = 0;
        var packageName = Path.GetFileNameWithoutExtension(path);
        var packagePath = Path.Combine(SongFolderPath, packageName);
        if (!Directory.Exists(packagePath))
        {
            ZipFile.ExtractToDirectory(path, packagePath);

            var di = new DirectoryInfo(packagePath);
            foreach (var fi in di.GetFiles("*.osu"))
            {
                res += InsertBeatmap(packageName, fi.FullName);
            }
            // 如果沒有符合條件的 beatmap，刪除該 beatmapset
            if (res == 0)
            {
                Directory.Delete(packagePath, true);
            }
            // 刪除 .osz 檔
            System.IO.File.Delete(path);
        }
        else
        {
            MessageBox.ShowLazy($"圖譜\"{packageName}\"已存在，自動刪除Osz檔案");
            Debug.LogError($"Folder Exists : {packagePath}");
        }
        return res;
    }

    /// <summary>
    /// 新增歌曲資訊至 BeatmapList
    /// </summary>
    /// <param name="packageName">歌曲包名稱</param>
    /// <param name="osuFilePath">.osu檔路徑</param>
    public static int InsertBeatmap(string packageName, string osuFilePath)
    {
        var beatmap = new BeatmapSimple(osuFilePath);
        if (beatmap.General.Mode != 3 || beatmap.Difficulty.CircleSize != 4)
        {
            return 0;
        }
        using (var songList = new SqliteManager(Application.streamingAssetsPath, "GameData.db") { Table = "BeatmapList" })
        {
            using (var dr = songList.Select($"BeatmapID = {beatmap.Metadata.BeatmapID}", "BeatmapID"))
            {
                if (!dr.HasRows)
                {
                    //  MP3 轉 WAV
                    if (Path.GetExtension(beatmap.General.AudioFilename) == ".mp3")
                    {
                        var src = Path.Combine(Application.streamingAssetsPath, "Songs", packageName, beatmap.General.AudioFilename);
                        var dst = Path.ChangeExtension(src, ".wav");
                        beatmap.General.AudioFilename = Path.ChangeExtension(beatmap.General.AudioFilename, ".wav");
                        // 如果該素材尚未轉檔，轉換之
                        if (!File.Exists(dst))
                        {
                            NAudioPlayer.ConvertMp3ToWav(src);
                            // 刪除原始檔
                            File.Delete(src);
                        }
                    }
                    //
                    songList.Insert(
                        beatmap.Metadata.BeatmapID,
                        beatmap.Metadata.BeatmapSetID,
                        beatmap.Metadata.Title,
                        beatmap.Metadata.TitleUnicode,
                        beatmap.Metadata.Artist,
                        beatmap.Metadata.ArtistUnicode,
                        beatmap.Metadata.Creator,
                        beatmap.Metadata.Version,
                        packageName,
                        beatmap.Events.Filename,
                        beatmap.General.AudioFilename,
                        Path.GetFileName(osuFilePath)
                        );
                    return 1;
                }
            }
            return 0;
        }
    }

    /// <summary>
    /// 取得相對路徑
    /// </summary>
    /// <param name="fullPath">目標路徑</param>
    /// <param name="basePath">基礎路徑</param>
    /// <returns>相對路徑</returns>
    public static string GetRelativePath(string fullPath, string basePath)
    {
        // 統一格式
        fullPath.Replace("/", @"\");
        basePath.Replace("/", @"\");
        // 路徑最後加 '\'
        if (!basePath.EndsWith(@"\"))
        {
            basePath += @"\";
        }
        return fullPath.Replace(basePath, "");
    }
}


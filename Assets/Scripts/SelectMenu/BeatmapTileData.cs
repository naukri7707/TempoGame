using System.IO;

public struct BeatmapTileData
{
    public BeatmapTileData(
        int beatmapID,
        int beatmapSetID,
        string title,
        string titleUnicode,
        string artist,
        string artistUnicode,
        string creator,
        string version,
        string packageName,
        string backgroundFile,
        string musicFile,
        string osuFile)
    {
        BeatmapID = beatmapID;
        BeatmapSetID = beatmapSetID;
        Title = title;
        TitleUnicode = titleUnicode;
        Artist = artist;
        ArtistUnicode = artistUnicode;
        Creator = creator;
        Version = version;
        PackageName = packageName;
        BackgroundFile = backgroundFile;
        MusicFile = musicFile;
        OsuFile = osuFile;
    }

    public int BeatmapID { get; }

    public int BeatmapSetID { get; }

    public string Title { get; }

    public string TitleUnicode { get; }

    public string Artist { get; }

    public string ArtistUnicode { get; }

    public string Creator { get; }

    public string Version { get; }

    public string PackageName { get; }

    public string BackgroundFile { get; }

    public string MusicFile { get; }

    public string OsuFile { get; }

    public string BackgroundPath => Path.Combine(BeatmapManager.SongFolderPath, PackageName, BackgroundFile);

    public string MusicPath => Path.Combine(BeatmapManager.SongFolderPath, PackageName, MusicFile);

    public string OsuPath => Path.Combine(BeatmapManager.SongFolderPath, PackageName, OsuFile);

    // TOFO OsuPath
}

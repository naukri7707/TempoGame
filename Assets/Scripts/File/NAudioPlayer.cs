using UnityEngine;
using System.IO;
using System;
using NAudio;
using NAudio.Wave;
using System.Text;

public static class NAudioPlayer
{
    /// <summary>
    /// 將 mp3 轉換成 wav
    /// </summary>
    /// <param name="src">mp3檔路徑</param>
    public static void ConvertMp3ToWav(string src)
    {
        var dst = Path.ChangeExtension(src, ".wav");

        using (var reader = new Mp3FileReader(src))
        {
            WaveFileWriter.CreateWaveFile(dst, reader);
        }
    }
}
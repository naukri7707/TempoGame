using Naukri.Beatmap;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Naukri.ExtensionMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 將 Image 的 Sprite 設為指定路徑下的圖片
        /// </summary>
        /// <param name="self">物件本身</param>
        /// <param name="path">完整圖片路徑</param>
        public static void GetExternalSprite(this Image self, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) //自動雙清
            {
                fs.Seek(0, SeekOrigin.Begin);            //設定當前流的位置
                byte[] bytes = new byte[fs.Length];      //創建文件長度緩衝區
                fs.Read(bytes, 0, (int)fs.Length);       //讀取文件
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(bytes);
                self.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
            }
        }

        /// <summary>
        /// 取得外部音效
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path"></param>
        /// <param name="playOnLoad"></param>
        public static void GetExternalAudio(this AudioSource self, string path, bool playOnLoad = false)
        {
            var fileType = AudioType.ACC;
            switch (Path.GetExtension(path))
            {
                case ".acc":
                    fileType = AudioType.ACC;
                    break;
                case ".ogg":
                    fileType = AudioType.OGGVORBIS;
                    break;
                case ".wav":
                    fileType = AudioType.WAV;
                    break;
                default:
                    break;
            }
            using (UnityWebRequest web = UnityWebRequestMultimedia.GetAudioClip(path, fileType))
            {
                var externalClip = DownloadHandlerAudioClip.GetContent(web);
                externalClip.name = Path.GetFileNameWithoutExtension(path);
                self.clip = externalClip;
                if (playOnLoad)
                {
                    self.Play();
                }
            }
        }

        /// <summary>
        /// 異步取得外部音樂
        /// </summary>
        /// <param name="self">物件本身</param>
        /// <param name="path">外部素材路徑</param>
        /// <param name="playOnLoad">載入後撥放</param>
        /// <returns></returns>
        public static IEnumerator GetExternalAudioAsync(this AudioSource self, string path, bool playOnLoad = false)
        {
            if (!path.StartsWith("file://"))
            {
                path = "file://" + path;
            }
            var fileType = AudioType.UNKNOWN;
            switch (Path.GetExtension(path))
            {
                case ".acc":
                    fileType = AudioType.ACC;
                    break;
                case ".mp3":
                    fileType = AudioType.MPEG;
                    break;
                case ".ogg":
                    fileType = AudioType.OGGVORBIS;
                    break;
                case ".wav":
                    fileType = AudioType.WAV;
                    break;
                default:
                    yield return null;
                    break;
            }
            using (UnityWebRequest web = UnityWebRequestMultimedia.GetAudioClip(path, fileType))
            {
                yield return web.SendWebRequest();
                if (!web.isNetworkError && !web.isHttpError)
                {
                    var externalClip = DownloadHandlerAudioClip.GetContent(web);
                    externalClip.name = Path.GetFileNameWithoutExtension(path);
                    self.clip = externalClip;
                    if (playOnLoad)
                    {
                        self.Play();
                    }
                }
            }
        }
    }
}
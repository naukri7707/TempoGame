using System.IO;
using UnityEngine;
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
        public static void SetSprite(this Image self, string path)
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
    }
}
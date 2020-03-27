using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace YTH.Functions
{
    class MP3
    {
        private static MediaPlayer player = new MediaPlayer();
        //音频标记-音频对应的文件地址
        private static Dictionary<string, string> mp3Dic = null;

        public static void play(string key)
        {
            //if (mp3Dic == null)
            //{
            //    mp3Dic = Config.getMp3Dic();
            //    List<string> keys = new List<string>();
            //    List<string> vals = new List<string>();
            //    foreach (KeyValuePair<string, string> kv in mp3Dic)
            //    {
            //        keys.Add(kv.Key);
            //        vals.Add(kv.Value);
            //    }
            //    for (int i = 0; i < keys.Count; i++)
            //    {
            //        mp3Dic[keys[i]] = @"MP3\" + vals[i];
            //    }
            //}
            //if (mp3Dic.ContainsKey(key))
            //{
            //    player.Open(new Uri(mp3Dic[key], UriKind.Relative));
            //    player.Play();
            //}
        }
    }
}

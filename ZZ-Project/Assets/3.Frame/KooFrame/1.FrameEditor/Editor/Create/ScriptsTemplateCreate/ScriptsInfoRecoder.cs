//using System;
//using System.IO;

//namespace KooFrame
//{
//    public class ScriptsInfoRecoder : UnityEditor.AssetModificationProcessor
//    {
//        private static void OnWillCreateAsset(string path)
//        {
//            path = path.Replace(".meta", "");
//            if (path.EndsWith(".cs"))
//            {
//                string str = File.ReadAllText(path);
//                str = str.Replace("#AUTHORNAME#", Environment.UserName).Replace(
//                    "#CREATETIME#", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss dddd"));
//                File.WriteAllText(path, str);
//            }
//        }
//    }
//}
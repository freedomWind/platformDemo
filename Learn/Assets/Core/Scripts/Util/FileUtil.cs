using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NEngine;

/*
1，花圈
2，
*/

public static class FileUtil
{
    public static void SaveFile(string path, string str)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        string dir = path.Substring(0, path.LastIndexOf('/'));
        if(!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        FileStream fs = File.Create(path);
       // Debug.LogError("ppp ="+path);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(str);
        sw.Close();
        fs.Close();
    }
    public static string ReadFromFile(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            string str = sr.ReadToEnd();
            return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(str));
        }
    }
    /// <summary>
    /// unity 内部文件夹streamingAsset需要用www加载
    /// </summary>
    /// <param name="path"></param>
    public static void ReadFromInnerFile(string path,System.Action<byte[]> result)
    {
        NEngine.Assets.Downloader.DownLoaderFile(path, result);
    }
    public static void SaveFile(string path, byte[] bytes)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream fs = File.Create(path);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }
    public static void DeleteFolder(string path)
    {
    }
    public static void ClearDirectory(string source)
    {
        if (Directory.Exists(source))
        {
            string[] files = Directory.GetFiles(source);
            for (int i = 0; i < files.Length; i++)
                File.Delete(files[i]);
        }
    }
    public static void DirCopyTo(string origDir, string destDir)
    {
        if (!Directory.Exists(origDir))
            return;
        if (!Directory.Exists(destDir))
            Directory.CreateDirectory(destDir);
        string[] files = Directory.GetFiles(origDir,"*.*",SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            string newAddr = files[i].Replace(origDir+"\\", "");
            File.Copy(files[i], destDir+"/"+newAddr, true);
        }
    }
}

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
}

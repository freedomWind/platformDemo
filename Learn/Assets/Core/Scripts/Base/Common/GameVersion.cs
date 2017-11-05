using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APPVersion
{
    List<GameVersion> gamelist;
    public APPVersion()
    {
        gamelist = new List<GameVersion>();
    }
    public void Add(GameVersion verr)
    {
        gamelist.Add(verr);
    }
    public string[] GetUpdateResult(APPVersion old)
    {
        return null;
    }
    public static APPVersion FromStr(string str)
    {
        APPVersion ver = new APPVersion();
        string[] strs = str.Split('|');
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i] == "") continue;
            GameVersion g = GameVersion.FromStr(strs[i]);
            ver.gamelist.Add(g);
        }
        return ver;
    }
    public override string ToString()
    {
        string str = "";
        foreach (var v in gamelist)
        {
            str += v.ToString();
            str += "|";
        }
        return str;
    }
}

public struct GameVersion
{
    public string gName;
    public int vID;
    public string desc;
    public GameVersion(string gName,string desc,int id = 0)
    {
        this.gName = gName;
        this.desc = desc;
        this.vID = id;
    }
    public void Add(string desc)
    {
        this.desc = desc;
        vID++;
    }
    public bool IsNew(GameVersion ver)
    {
        return ver.gName == gName && ver.vID > vID;
    }
    public override string ToString()
    {
        //return base.ToString();
        return gName + "&" + desc + "&"+vID;
    }
    public static GameVersion FromStr(string str)
    {
        GameVersion ver = default(GameVersion);
        string[] strs = str.Split('&');
        if (strs.Length == 3)
        {
            int id = 0;
            int.TryParse(strs[2], out id);
            ver = new GameVersion(strs[0], strs[1], id);
        }
        return ver;
    }
}

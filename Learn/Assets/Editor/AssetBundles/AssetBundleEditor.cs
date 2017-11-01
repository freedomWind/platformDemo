using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using NEngine.Assets;

namespace NEngine
{ 
    static class AssetBundleEditor
    {
        static string pathPacking = Application.dataPath + "/ForAssetbundleMaking";
        static string pathConfig_Editor = Application.dataPath + "/Editor/Config";
        static string pathRes_Editor = Application.dataPath + "/Editor/Res";

        static Dictionary<string, AssetBundleManifest> _dic = new Dictionary<string, AssetBundleManifest>();
        public static AssetBundleManifest Manifest(string game)
        {
            AssetBundleManifest _main = null;
            _dic.TryGetValue(game, out _main);
            if (_main == null)
            {
                string path = pathRes_Editor + "/" + game+"_ab" + "/" + game+"_ab";
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                _main = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                _dic.Add(game, _main);
          //      ab.Unload(true);
            }
            return _main;
        }

        [MenuItem("Tools/BuildAssetBundle")]
        static void BuildSourceToAB()
        {
            string[] names = System.Enum.GetNames(typeof(NEngine.Game.GameEnum));
            for (int i = 0; i < names.Length; i++)
            {
                BuildSourceToAB(names[i]);
            }
        }
        static void BuildSourceToAB(string game)
        {
            ClearAssetBundlesName();
            AssetbundleConfig abconfig = new AssetbundleConfig();
            Pack(game,ref abconfig);
            string outputPath = pathRes_Editor + "/" + game + "_ab";  //生成的ab包路径
            DeleteDirectory(outputPath);
            Directory.CreateDirectory(outputPath);
            Debug.Log("BuildSourceToAB outputPath = "+outputPath);
            BuildPipeline.BuildAssetBundles(outputPath,BuildAssetBundleOptions.ChunkBasedCompression,EditorUserBuildSettings.activeBuildTarget);//会在根目录生成manifest

            //Debug.Log("打包完成");
            ////生成Config文件
            ClearConfigStreamAssets(game);
            CreatAssetversionInfo(game);
            CreateBundleInfo(game, abconfig);
            CreatePreloadInfo(game);
        }
        [MenuItem("Tools/InitAssetsConfig")]
        public static void CheckInit()
        {
            string[] names = System.Enum.GetNames(typeof(NEngine.Game.GameEnum));
            for (int i = 0; i < names.Length; i++)
            {
                CheckInit(names[i]);
            }
        }
        /// <summary>
        /// 初始化游戏资源目录
        /// ab包制作目录
        /// 游戏资源目录结构
        /// </summary>
        /// <param name="gamename"></param>
        public static void CheckInit(string gamename)
        {
            string path1 = pathPacking + "/ForPackWithDependices/"+gamename+"_Res";
            string path2 = pathPacking + "/ForPackWithNoDependices/"+gamename+"_Res";
           
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }        
        }
        /// <summary>
        /// 生成资源配置信息： 资源包信息， 资源与包的关系信息
        /// </summary>
        static void CreatAssetversionInfo(string game)
        {
            Dictionary<string, Hash128> abDic = new Dictionary<string, Hash128>();
            string manifestFolder = game + "_ab";
            string mainFolder = game + "_Res";
            mainFolder = mainFolder.ToLower();
            GetABInfo(game, pathRes_Editor +"/"+manifestFolder +"/"+mainFolder, ref abDic);
            AssetVersion rinfo = new AssetVersion();
            rinfo.manifestName = mainFolder +"/"+manifestFolder; //  将manifest文件放入对应游戏资源子目录下
            foreach (KeyValuePair<string, Hash128> kp in abDic)
                rinfo.bundlesInfo.Add(kp.Key, kp.Value);
            string path = pathConfig_Editor + "/" + game + "/assetVersion.config";
            FileUtil.SaveFile(path, rinfo.ToStr());
            CopyToStreamingAssets(game, path);
        }
        static void CreateBundleInfo(string game, AssetbundleConfig abinfo)
        {
            string path = pathConfig_Editor + "/" + game + "/bundle.config";
            string str = abinfo.ToString();
            FileUtil.SaveFile(path, abinfo.ToString());
            CopyToStreamingAssets(game, path);
        }
        static void CreatePreloadInfo(string game)
        {
            PreloadConfig preload = new PreloadConfig();
            string path = pathConfig_Editor + "/" + game + "/preload.config";
            FileUtil.SaveFile(path, preload.ToString());
            CopyToStreamingAssets(game, path);
        }

        static void GetABInfo(string game, string source, ref Dictionary<string, Hash128> dic)
        {
            DirectoryInfo floder = new DirectoryInfo(source);
            FileSystemInfo[] files = null;
            try
            {
                files = floder.GetFileSystemInfos();
            }
            catch
            {
                return;
            }
            if (files == null) return;
            string assetbundlename = "";
            Debug.Log("source:" + source);
            //fullname ../Res/No1/No1_Res
            string replacestr = pathRes_Editor.Replace('/', '\\');
            replacestr += "\\"+game+"_ab\\";
            foreach (FileSystemInfo f in files)
            {
                if (f is DirectoryInfo)
                {
                    GetABInfo(game, f.FullName, ref dic);
                }
                else
                {
                    if (!f.Name.EndsWith(".unity3d"))
                        continue;
                    assetbundlename = f.FullName.Replace(replacestr, "");
                    Debug.Log("assetbundlename:" + assetbundlename + " - replacestr:" + replacestr);

                    Hash128 hash = Manifest(game).GetAssetBundleHash(assetbundlename);
                    if (hash != null)
                        dic.Add(assetbundlename, hash);
                }
            }
        }

        static void ClearAssetBundlesName()
        {
            int length = AssetDatabase.GetAllAssetBundleNames().Length;
            string[] temName = new string[length];
            if (length == 0) return;
            for (int i = 0; i < length; i++)
            {
                temName[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            }
            for (int i = 0; i < temName.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(temName[i], true);
            }
        }

        static void Pack(string game,ref AssetbundleConfig abconfig)
        {
            Pack(pathPacking + "/ForPackWithDependices/"+game+"_Res",true,ref abconfig);  //处理依赖关系打包方式
            Pack(pathPacking + "/ForPackWithNoDependices/"+game+"_Res", false,ref abconfig);  //不处理依赖关系打包
        }

        static void Pack(string source, bool isDetailDepends, ref AssetbundleConfig abconfig)
        {
            Debug.Log("source: " + source);
            DirectoryInfo folder = null;
            FileSystemInfo[] files = null;
            try
            {
                folder = new DirectoryInfo(source);
                files = folder.GetFileSystemInfos();
            }
            catch { Debug.LogWarning("no files at the folder:"+source); return; }
            int length = files.Length;
            for (int i = 0; i < length; i++)
            {
                if (files[i] is DirectoryInfo)
                {
                    Pack(files[i].FullName, isDetailDepends, ref abconfig);
                }
                else
                {
                    if (!files[i].Name.EndsWith(".meta"))
                    {
                        if (files[i].Name.EndsWith(".cs"))// || files[i].Name.Equals("assetVersionInfo.txt"))
                            continue;
                        if (isDetailDepends)
                            fileWithDepends(files[i].FullName, ref abconfig);
                        else
                            file(files[i].FullName, ref abconfig);
                    }
                }
            }
        }
        /// <summary>
        /// 打包资源 （依赖关系）
        /// </summary>
        /// <param name="source">资源名称（No1_Res/）</param>
        /// <param name="abConfig"></param>
        static void fileWithDepends(string source,ref AssetbundleConfig abConfig)
        {
            string _source = Replace(source);
            if (_source == "") return;
            _source = _source.Substring(Application.dataPath.Length + 1);
            _source = "Assets/" + _source;
            string[] dps = AssetDatabase.GetDependencies(_source);
            foreach (var dp in dps)
            {
                if (dp.EndsWith(".cs"))
                    continue;
                AssetImporter assetImporter = AssetImporter.GetAtPath(dp);
                string fullname = dp.Replace("Assets/ForAssetbundleMaking/", "");
                fullname = fullname.Substring(fullname.IndexOf('/') + 1);  //资源的名字
                string bagname = fullname.Replace(Path.GetExtension(fullname), ".unity3d");
                assetImporter.assetBundleName = bagname;
                //Debug.Log("bagna:" + bagname);
                abConfig.AddAssetBundle(fullname, bagname);  //资源名和包名关系
            }
        }
        /// <summary>
        /// 打包 不处理依赖关系
        /// </summary>
        /// <param name="source"></param>
        static void file(string source,ref AssetbundleConfig abconfig)
        {
            string _source = Replace(source);
            if (_source == "") return;
            _source = _source.Substring(Application.dataPath.Length + 1);
            _source = "Assets/" + _source;
            AssetImporter assetImport = AssetImporter.GetAtPath(_source);

            string fullname = _source.Replace("Assets/ForAssetbundleMaking/", "");
            fullname = fullname.Substring(fullname.IndexOf('/') + 1);  //资源的名字
            string bagname = fullname.Replace(Path.GetExtension(fullname), ".unity3d");
            assetImport.assetBundleName = bagname;
            //Debug.Log("bagna:" + bagname);
            abconfig.AddAssetBundle(fullname, bagname);  //资源名和包名关系
        }

        static string Replace(string s)
        {
            return s.Replace("\\", "/");
        }
        static void CopyToStreamingAssets(string game,string origPath)
        {
            string filename = origPath.Substring(origPath.LastIndexOf('/')+1);
            Debug.Log("filename = " + filename);
            string aimPath = Application.streamingAssetsPath + "/CONFIG/" + game;
            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }
            Directory.CreateDirectory(aimPath);
            aimPath = aimPath + "/" + filename;
            File.Copy(origPath, aimPath);
        }
        static void ClearConfigStreamAssets(string game)
        {
            string dir = Application.streamingAssetsPath + "/CONFIG/" + game;
            ClearDirectory(dir);
        }
        static void ClearDirectory(string source)
        {
            if (Directory.Exists(source))
            {
                string[] files = Directory.GetFiles(source);
                for (int i = 0; i < files.Length; i++)
                    File.Delete(files[i]);
            }
        }
        static void DeleteDirectory(string source)
        {
            if (!Directory.Exists(source)) return;
            DirectoryInfo info = new DirectoryInfo(source);
            FileSystemInfo[] ff = info.GetFileSystemInfos();
            for (int i = 0; i < ff.Length; i++)
            {
                if (ff[i] is DirectoryInfo)
                {
                    DeleteDirectory(ff[i].FullName);
                }
                else
                {
                    File.Delete(ff[i].FullName);
                }
            }
            Directory.Delete(source);
        }
    }
}



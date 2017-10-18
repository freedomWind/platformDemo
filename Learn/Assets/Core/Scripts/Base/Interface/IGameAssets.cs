using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    public class GameAsset
    {
        public string assetVersion;
        public GameAsset()
        { }


    }

    public interface IGameAsset
    {
        string assetVersion { get; set; }

    }
}

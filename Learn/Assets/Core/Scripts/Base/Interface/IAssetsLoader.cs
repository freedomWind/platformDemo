using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    public interface IAssetsLoader 
    {
        bool checkUpdate();
        void doUpdate(System.Action callback);
    }
}

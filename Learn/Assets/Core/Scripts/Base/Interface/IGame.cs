using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Game
{
    public abstract class IGame 
    {
        public readonly string Name;   //名称
        public string Desc;            //描述
        
        public IGame(string name)
        {
            this.Name = name;
        }
    }
}

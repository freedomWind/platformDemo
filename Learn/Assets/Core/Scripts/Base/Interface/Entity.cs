using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NEngine.Event
{
    public delegate void EventDel(EventArg arg);
    public struct EventArg
    {
        public readonly System.Enum eID;
        public readonly object obj1;
        public readonly object obj2;

        public EventArg(System.Enum eID, object obj1 = null, object obj2 = null)
        {
            this.eID = eID;
            this.obj1 = obj1;
            this.obj2 = obj2;
        }
    }
    /// <summary>
    /// 消息实体 
    /// </summary>
    public abstract class Entity
    {
        Dictionary<Enum, EventDel> _delDic;

        public void Dispath(EventArg arg)
        {
            EventDel del = null;
            _delDic.TryGetValue(arg.eID, out del);
            if (del != null)
                del(arg);
            else
            {
                Debug.LogWarning("entity is didnot add listenner, enum = " + arg.eID);
            }
        }
        public Entity()
        {
            _delDic = new Dictionary<Enum, EventDel>();
        }
        public void AddListener(Enum e, EventDel del)
        {
            EventDel d = null;
            _delDic.TryGetValue(e, out d);
            if (d == null)
            {
                _delDic.Add(e, del);
            }
         //   else Delegate.Combine(d, del);
        }
        public void RemoveListener(Enum e)
        {
            _delDic.Remove(e);
        }
        public void Clear()
        {
            _delDic.Clear();
        }
    }
}
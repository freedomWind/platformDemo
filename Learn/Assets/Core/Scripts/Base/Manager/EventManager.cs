using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NEngine.Event
{
    public class EventManager
    {
        NEnity enity;
        public EventManager()
        {
            enity = new NEnity();
        }
        public void DisPatch(EventArg arg)
        {
            enity.Dispath(arg);
        }
        public void AddListener(Enum e, EventDel del)
        {
            enity.AddListener(e, del);
        }
        public void Remove(Type type)
        {
            foreach (var v in Enum.GetValues(type))
            {
                enity.RemoveListener((Enum)v);
            }
        }

    }
    public sealed class NEnity : Entity
    { }
}





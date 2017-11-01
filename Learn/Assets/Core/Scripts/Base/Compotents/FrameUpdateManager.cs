using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine
{
    public class FrameUpdateManager :MonoBehaviour
    {
        private List<IFrameUpdate> _list;
        private static FrameUpdateManager _ins;
        public static FrameUpdateManager Instance
        {
            get
            {
                if (_ins == null)
                {
                    _ins = App.Ins.AppMono.gameObject.AddComponent<FrameUpdateManager>();
                }
                return _ins;
            }
        }
        private FrameUpdateManager()
        {
            _list = new List<IFrameUpdate>();
        }
        public void AddFrame(IFrameUpdate frame)
        {
            _list.Add(frame);
        }
        public void RemoveFrame(IFrameUpdate frame)
        {
            _list.Remove(frame);
        }
        void Update()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].UpdateDo(Time.deltaTime);
            }
        }
    }
}
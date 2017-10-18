using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine
{
    public interface IFrameUpdate
    {
        void UpdateDo(float time);
    }
}

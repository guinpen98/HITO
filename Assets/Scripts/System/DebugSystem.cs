using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HITO.System
{
    public class DebugSystem : BaseSystem
    {
        public override void SetEvent()
        {
            _gameEvent.OnInput += InputLog;
        }

        public void InputLog(string text)
        {
            Debug.Log(text);
        }
    }
}

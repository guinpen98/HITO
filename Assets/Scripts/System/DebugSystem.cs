using System.Collections;
using System.Collections.Generic;
using NMeCab.Specialized;
using UnityEngine;

namespace HITO.System
{
    public class DebugSystem : BaseSystem
    {
        public override void SetEvent()
        {
            _gameEvent.OnInput += InputLog;
            _gameEvent.OnMorphologicalAnalyze += MorphologicalAnalyzeLog;
        }

        public void InputLog(string text)
        {
            Debug.Log(text);
        }

        public void MorphologicalAnalyzeLog(MeCabIpaDicNode[] nodes)
        {
            foreach (var item in nodes)
                Debug.Log($"{item.Surface}, {item.PartsOfSpeech}, {item.PartsOfSpeechSection1}, {item.PartsOfSpeechSection2}");
        }
    }
}

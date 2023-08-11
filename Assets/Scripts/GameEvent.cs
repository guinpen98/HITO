using System;
using NMeCab.Specialized;

namespace HITO
{
    /// <summary>
    /// イベント定義クラス
    /// </summary>
    public class GameEvent
    {
        public Action<string> OnInput;
        public Action<MeCabIpaDicNode[]> OnMorphologicalAnalyze;
    }
}
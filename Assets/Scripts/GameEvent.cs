using System;

namespace HITO
{
    /// <summary>
    /// イベント定義クラス
    /// </summary>
    public class GameEvent
    {
        public Action<string> OnInput;
    }
}
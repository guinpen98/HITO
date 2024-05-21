using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HITO.System
{
    /// <summary>
    /// System層のベースクラス
    /// </summary>
    public class BaseSystem
    {
        /// <summary>ゲームのデータ</summary>
        protected GameState _gameState;
        /// <summary>ゲームのイベント</summary>
        protected GameEvent _gameEvent;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(GameState gameState, GameEvent gameEvent)
        {
            _gameState = gameState;
            _gameEvent = gameEvent;
        }

        /// <summary>
        /// イベントをセットする
        /// </summary>
        public virtual void SetEvent() { }
    }
}
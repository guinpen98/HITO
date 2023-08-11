using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HITO.System
{
    /// <summary>
    /// 入力を受け取るシステム
    /// </summary>
    public class InputSystem : BaseSystem
    {
        public override void SetEvent()
        {
            _gameState.TextBox.onEndEdit.AddListener(Input);
        }

        /// <summary>
        /// Inputを取得する
        /// </summary>
        public void Input(string text)
        {
            if (_gameState.IsCharacterTalking) return;

            _gameEvent.OnInput.Invoke(text);
            _gameState.TextBox.text = "";
        }
    }
}
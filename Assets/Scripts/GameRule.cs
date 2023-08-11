using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HITO.System
{
    public class GameRule : BaseSystem
    {
        public override void SetEvent()
        {
            _gameEvent.OnInputText += InvokeOnInputToDialogSystem;
        }

        /// <summary>
        /// 会話システムに入力を渡す
        /// </summary>
        public void InvokeOnInputToDialogSystem(string input)
        {
            _gameEvent.OnInputToDialogSystem.Invoke(input);
        }
    }
}
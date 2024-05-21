using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace HITO
{
    /// <summary>
    /// ゲームのデータクラス
    /// </summary>
    [Serializable]
    public class GameState
    {
        #region UI
        [Header("UI")]
        public TMP_InputField TextBox;
        public TMP_Text CharacterDialog;
        public float TypingSpeed = 0.05f;
        #endregion

        #region System
        [Header("System")]
        public bool IsCharacterTalking;
        #endregion

    }
}
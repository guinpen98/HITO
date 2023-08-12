using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HITO.System
{
    public class UISystem : BaseSystem
    {
        public override void SetEvent()
        {
            _gameEvent.SetCharacterText += ShowCharacterMessage;
        }

        /// <summary>
        /// テキストをアニメーションで表示
        /// </summary>
        public void ShowCharacterMessage(string message)
        {
            _gameState.IsCharacterTalking = true;
            _gameState.CharacterDialog.text = "";
            _gameState.TextBox.gameObject.SetActive(false);

            // DOTweenを利用してテキストのタイピングアニメーションを追加
            _gameState.CharacterDialog.DOText(message, message.Length * _gameState.TypingSpeed).OnComplete(() =>
            {
                _gameState.IsCharacterTalking = false;
                _gameState.TextBox.gameObject.SetActive(true);
            });
        }
    }
}
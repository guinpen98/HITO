using System;

namespace Chat
{
    /// <summary>アバターコントローラーインターフェース</summary>
    public interface IAvatarController
    {
        /// <summary>アバターを表示する</summary>
        public void ShowAvatar();

        /// <summary>アバターを非表示にする</summary>
        public void HideAvatar();

        /// <summary>リップシンクを開始する</summary>
        public void StartLipSync();

        /// <summary>リップシンクを停止する</summary>
        public void StopLipSync();

        /// <summary>瞬きを開始する</summary>
        public void StartBlink();

        /// <summary>瞬きを停止する</summary>
        public void StopBlink();

        /// <summary>表情を設定する</summary>
        public void SetExpression(string expression);
    }
}

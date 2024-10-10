using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Chat
{
    [Serializable]
    public class OutputSystem : IDisposable
    {
        /// <summary>出力イベント</summary>
        public Action<string> OnOutput { get; set; }

        /// <summary>アバターを表示するかどうか</summary>
        public bool IsShowAvatar { get => _isShowAvatar; }
        [SerializeField] private bool _isShowAvatar;

        private IVoiceOutputSystem _voiceOutputSystem;

        public OutputSystem()
        {
            OnOutput += async (text) => await Output(text);
        }

        private async Task Output(string text)
        {
            Debug.Log($"Output: {text}");
            if (!_isShowAvatar)
            {
                return;
            }

            if (_voiceOutputSystem == null)
            {
                _voiceOutputSystem = new BoyomichanSystem();
            }

            try
            {
                await _voiceOutputSystem.PlayVoiceAsync(text);
            }
            catch (Exception ex)
            {
                Debug.LogError("音声出力エラー: " + ex.Message);
            }
        }

        public void Dispose()
        {
            if (_voiceOutputSystem != null)
            {
                _voiceOutputSystem.Dispose();
            }
        }
    }
}

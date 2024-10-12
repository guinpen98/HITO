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

        [SerializeField] private YukkuriController _yukkuriController;

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

            _yukkuriController.StartLipSync();

            try
            {
                await _voiceOutputSystem.PlayVoiceAsync(text);
            }
            catch (Exception ex)
            {
                Debug.LogError("音声出力エラー: " + ex.Message);
            }

            // 1秒後にリップシンクを止める
            await Task.Delay(TimeSpan.FromSeconds(1));
            _yukkuriController.StopLipSync();
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

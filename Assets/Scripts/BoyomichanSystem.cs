using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityBoyomichanClient;

namespace Chat
{
    class BoyomichanSystem : IVoiceOutputSystem
    {
        public async Task PlayVoiceAsync(string text)
        {
            Debug.Log($"PlayVoice: {text}");
            var _boyomiClient = new TcpBoyomichanClient("127.0.0.1", 50001);

            await _boyomiClient.TalkAsync(
                message: text,
                speed: -1, // -1で棒読みちゃん側の設定
                volume: -1,// -1で棒読みちゃん側の設定
                pitch: -1, // -1で棒読みちゃん側の設定
                voiceType: VoiceType.DefaultVoice,
                cancellationToken: default(CancellationToken));

            // 読み上げが終わるのをまつ
            while (await _boyomiClient.CheckNowPlayingAsync())
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5f)); //0.5sごとに確認
            }

            Debug.Log("読み上げ終了");
            _boyomiClient.Dispose();
        }

        public void Dispose()
        {
            // 何もしない
        }
    }
}

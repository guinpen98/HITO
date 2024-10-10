using System.Threading.Tasks;

namespace Chat
{
    interface IVoiceOutputSystem
    {
        /// <summary>音声を再生する</summary>
        public Task PlayVoiceAsync(string text);

        /// <summary>リソースを解放する</summary>
        public void Dispose();
    }
}

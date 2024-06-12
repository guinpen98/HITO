using System;

public interface IVoiceRecognizer
{
    /// <summary>音声認識結果を通知する</summary>
    public Action<string> OnRecognized { get; set; }

    public void Start();

    public void Dispose();
}

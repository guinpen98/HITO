using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognizer
{
    /// <summary>音声認識結果を通知する</summary>
    public Action<string> OnRecognized { get; set; }

    private DictationRecognizer _dictationRecognizer;

    public VoiceRecognizer()
    {
        _dictationRecognizer = new DictationRecognizer();

        _dictationRecognizer.DictationResult += InvokeOnRecognizedAction;

        _dictationRecognizer.DictationError += OnError;

        _dictationRecognizer.Start();
    }


    public void Dispose()
    {
        if (_dictationRecognizer != null)
        {
            _dictationRecognizer.Stop();
            _dictationRecognizer.Dispose();
        }
    }

    private void InvokeOnRecognizedAction(string text, ConfidenceLevel confidence)
    {
        OnRecognized.Invoke(text);
    }

    private void OnError(string error, int hresult)
    {
        Debug.LogWarning($"Error: {error}, hresult: {hresult}");
    }
}

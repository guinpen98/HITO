using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Speech : MonoBehaviour
{
    private DictationRecognizer _dictationRecognizer;

    private void Start()
    {
        _dictationRecognizer = new DictationRecognizer();

        _dictationRecognizer.DictationResult += DictationRecResult;
        _dictationRecognizer.DictationError += DictationRecError;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _dictationRecognizer.Status == SpeechSystemStatus.Stopped)
        {
            _dictationRecognizer.Start();
            Debug.Log("音声認識開始");
        }
        if (Input.GetKeyUp(KeyCode.Space) && _dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            _dictationRecognizer.Stop();
            Debug.Log("音声認識停止");
        }
    }

    private void DictationRecResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log($"認識した音声： {text}");
    }

    private void DictationRecError(string error, int hresult)
    {
        Debug.Log($"エラー：{error}, {hresult}");
    }

    private void OnDestroy()
    {
        if (_dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            _dictationRecognizer.Stop();
        }
        _dictationRecognizer.Dispose();
    }
}

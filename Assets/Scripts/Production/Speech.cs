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
            Debug.Log("�����F���J�n");
        }
        if (Input.GetKeyUp(KeyCode.Space) && _dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            _dictationRecognizer.Stop();
            Debug.Log("�����F����~");
        }
    }

    private void DictationRecResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log($"�F�����������F {text}");
    }

    private void DictationRecError(string error, int hresult)
    {
        Debug.Log($"�G���[�F{error}, {hresult}");
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

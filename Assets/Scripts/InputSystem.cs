using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class InputSystem
{
    /// <summary>入力イベント</summary>
    public Action<string> InputEvent { get; set; }

    /// <summary>音声入力かどうか</summary>
    [SerializeField] private bool _isVoiceInput;
    /// <summary>テキストボックス</summary>
    [SerializeField] private TMP_InputField _inputField;

    private VoiceRecognizer _voiceRecognizer;
    private string _inputBuffer;

    public void Start()
    {
        if (_isVoiceInput)
        {
            _voiceRecognizer = new VoiceRecognizer();
            _voiceRecognizer.OnRecognized += SetInput;
        }
        else
        {
            _inputField.onEndEdit.AddListener(KeyboardInput);
        }
    }

    public void Update()
    {
        if (string.IsNullOrEmpty(_inputBuffer))
        {
            return;
        }

        InputEvent.Invoke(_inputBuffer);
        Debug.Log("Input: " + _inputBuffer);
        _inputBuffer = null;
    }

    public void Dispose()
    {
        if (_isVoiceInput)
        {
            _voiceRecognizer.Dispose();
        }
    }

    /// <summary>
    /// 音声認識を開始する
    /// </summary>
    public void StartRecognizing(string text)
    {
        _voiceRecognizer.Start();
    }

    /// <summary>
    /// 入力を受け付ける
    /// </summary>
    private void SetInput(string text)
    {
        _inputBuffer = text;
    }

    /// <summary>
    /// 音声入力
    /// </summary>
    private void VoiceInput()
    {
        _voiceRecognizer.OnRecognized += SetInput;
    }

    /// <summary>
    /// キーボード入力
    /// </summary>
    private void KeyboardInput(string text)
    {
        SetInput(text);
        _inputField.text = string.Empty;
    }
}

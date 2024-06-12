using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMain : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    private DialogueSystem _dialogueSystem;

    private void Start()
    {
        _dialogueSystem = new DialogueSystem();
        _inputSystem.InputEvent += _dialogueSystem.OnInput;

        if (_inputSystem.IsVoiceInput)
        {
            _dialogueSystem.OnResponse += _inputSystem.StartRecognizing;
        }

        _inputSystem.Start();
    }

    private void Update()
    {
        _inputSystem.Update();
    }

    private void OnDestroy()
    {
        _inputSystem.Dispose();
        AppData.Instance.Dispose();
    }
}

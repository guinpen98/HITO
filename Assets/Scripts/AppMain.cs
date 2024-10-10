using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chat
{
    public class AppMain : MonoBehaviour
    {
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private OutputSystem _outputSystem;
        private DialogueSystem _dialogueSystem;

        private void Start()
        {
            _dialogueSystem = new DialogueSystem();
            _inputSystem.InputEvent += _dialogueSystem.OnInput;
            _dialogueSystem.OnResponse += _outputSystem.OnOutput;

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
}

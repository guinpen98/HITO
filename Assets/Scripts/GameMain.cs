using System.Collections;
using System.Collections.Generic;
using HITO.NLP;
using HITO.System;
using UnityEngine;

namespace HITO
{
    /// <summary>
    /// エントリーポイント
    /// </summary>
    public class GameMain : MonoBehaviour
    {
        [Header("GameSystem")]
        [SerializeField] private GameState _gameState;
        private GameEvent _gameEvent = new GameEvent();

        [Header("NLP")]
        [SerializeField] private NLPApp _NLPApp;

        private List<BaseSystem> _systemList;
        private List<IPreUpdateSystem> _preUpdateSystemList;
        private List<IOnUpdateSystem> _onUpdateSystemList;

        private void Awake()
        {
            // システムの初期化
            _systemList = new List<BaseSystem>()
            {
                new GameRule(),
                new InputSystem(),
                new UISystem(),
                new DebugSystem(),
                new DialogSystem()
            };

            foreach (BaseSystem system in _systemList)
            {
                system.Init(_gameState, _gameEvent);
                system.SetEvent();
                if (system is IPreUpdateSystem) _preUpdateSystemList.Add(system as IPreUpdateSystem);
                if (system is IOnUpdateSystem) _onUpdateSystemList.Add(system as IOnUpdateSystem);
            }

            // NLP
            _gameEvent.RequestNLU += InvokeNLU;
        }

        private void Update()
        {

        }

        private void InvokeNLU(NLURequest request)
        {
            _gameEvent.ResponseNLU(_NLPApp.NLU(request));
        }
    }
}

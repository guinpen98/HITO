using System.Collections;
using System.Collections.Generic;
using HITO;
using HITO.System;
using UnityEngine;

/// <summary>
/// エントリーポイント
/// </summary>
public class GameMain : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    private GameEvent _gameEvent = new GameEvent();

    private List<BaseSystem> _systemList;
    private List<IPreUpdateSystem> _preUpdateSystemList;
    private List<IOnUpdateSystem> _onUpdateSystemList;

    private void Awake()
    {
        _systemList = new List<BaseSystem>()
        {
            new InputSystem(),
            new UISystem(),
            new DebugSystem(),
            new MorphologicalAnalysisSystem()
        };

        foreach (BaseSystem system in _systemList)
        {
            system.Init(_gameState, _gameEvent);
            system.SetEvent();
            if (system is IPreUpdateSystem) _preUpdateSystemList.Add(system as IPreUpdateSystem);
            if (system is IOnUpdateSystem) _onUpdateSystemList.Add(system as IOnUpdateSystem);
        }

    }

    private void Update()
    {

    }
}

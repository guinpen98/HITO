using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppData : Singleton<AppData>
{
    private List<DialogueData> _dialogueDataList = new List<DialogueData>();

    public void InsertDialogueData(uint characterId, string text)
    {
        if (_dialogueDataList == null)
        {
            _dialogueDataList = new List<DialogueData>();
        }
        uint id = (uint)_dialogueDataList.Count + 1;
        _dialogueDataList.Add(new DialogueData(id, characterId, text));
        Debug.Log($"InsertDialogueData: {id}, {Enum.GetName(typeof(CharacterId), characterId)}, {text}");
    }

    public void ClearDialogueData()
    {
        _dialogueDataList.Clear();
    }

    public override void Dispose()
    {
        base.Dispose();
        ClearDialogueData();
    }
}

public record DialogueData(uint Id, uint CharacterId, string Text);

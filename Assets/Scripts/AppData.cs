using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppData : Singleton<AppData>
{
    public List<DialogueData> DialogueDataList { get; private set; }

    public AppData()
    {
        DialogueDataList = new List<DialogueData>();
    }

    public void InsertDialogueData(uint characterId, string text)
    {
        if (DialogueDataList == null)
        {
            DialogueDataList = new List<DialogueData>();
        }
        var id = (uint)DialogueDataList.Count + 1;
        DialogueDataList.Add(new DialogueData(id, characterId, text));
        Debug.Log($"InsertDialogueData: {id}, {Enum.GetName(typeof(CharacterId), characterId)}, {text}");
    }

    public void ClearDialogueData()
    {
        DialogueDataList.Clear();
    }

    public override void Dispose()
    {
        base.Dispose();
        ClearDialogueData();
    }
}

public record DialogueData(uint Id, uint CharacterId, string Text);

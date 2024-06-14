using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueSystem
{
    /// <summary>入力を通知する</summary>
    public Action<string> OnInput { get; set; }
    ///<summary>応答を通知する</summary>
    public Action<string> OnResponse { get; set; }

    public DialogueSystem()
    {
        OnInput += async (text) => await Dialogue(text);
    }

    /// <summary>
    /// 対話
    /// </summary>
    private async Task Dialogue(string text)
    {
        // 会話データを保存
        AppData.Instance.InsertDialogueData((uint)CharacterId.Player, text);

        // 入力を解析する
        // var morphemes = MorphologicalAnalyzer.Analyze(text);
        // foreach (var morpheme in morphemes)
        // {
        //     Debug.Log($"MorphologicalAnalyzer: {morpheme.Surface}, {morpheme.PartsOfSpeech}, {morpheme.PartsOfSpeechSection}");
        // }

        // JUMANとKNPで解析する
        Clause[] knpResult = null;
        try
        {
            knpResult = await JumanKNPParser.ParseTextAsync(text);
        }
        catch (Exception ex)
        {
            Debug.LogError("解析エラー: " + ex.Message);
            OnResponse?.Invoke("解析エラーが発生しました。");
        }

        // TODO: 解析結果に基づいて応答を生成する
        // var response = string.Empty;
        // AppData.Instance.InsertDialogueData((uint)CharacterId.NPC, response);
        // OnResponse.Invoke(response);

        OnResponse?.Invoke("応答が生成されました。");
    }
}

public enum CharacterId
{
    Player = 1,
    NPC = 2,
}

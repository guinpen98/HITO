using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem
{
    /// <summary>入力を通知する</summary>
    public Action<string> OnInput { get; set; }

    public DialogueSystem()
    {
        OnInput += Dialogue;
    }

    /// <summary>
    /// 対話
    /// </summary>
    private void Dialogue(string text)
    {
        // 入力を解析する

        // 解析結果に基づいて応答を生成する

    }
}

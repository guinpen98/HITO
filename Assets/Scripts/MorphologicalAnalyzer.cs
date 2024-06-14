using NMeCab.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MorphologicalAnalyzer
{
    private const string DictionaryDir = @"Assets/AssetStoreTools/dic/ipadic";

    /// <summary>
    /// 形態素解析
    /// </summary>
    /// <param name="text">解析する対象の文字列</param>
    public static MeCabMorpheme[] Analyze(string text)
    {
        using (var tagger = MeCabIpaDicTagger.Create(DictionaryDir))
        {
            return tagger.Parse(text).Select(x => new MeCabMorpheme
            (
                x.Surface,
                x.PartsOfSpeech,
                x.PartsOfSpeechSection1
            )).ToArray();
        }
    }
}

/// <summary>
/// 形態素
/// </summary>
public record MeCabMorpheme(string Surface, string PartsOfSpeech, string PartsOfSpeechSection);

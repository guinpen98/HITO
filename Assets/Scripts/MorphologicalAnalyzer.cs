using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NMeCab.Specialized;

public static class MorphologicalAnalyzer
{
    private const string DictionaryDir = @"Assets/AssetStoreTools/dic/ipadic";

    /// <summary>
    /// 形態素解析
    /// </summary>
    /// <param name="text">解析する対象の文字列</param>
    public static List<MeCabMorpheme> Analyze(string text)
    {
        List<MeCabMorpheme> nodes;
        using (var tagger = MeCabIpaDicTagger.Create(DictionaryDir))
        {
            nodes = tagger.Parse(text).Select(x => new MeCabMorpheme
            (
                x.Surface,
                x.PartsOfSpeech,
                x.PartsOfSpeechSection1
            )).ToList();
        }
        return nodes;
    }
}

/// <summary>
/// 形態素
/// </summary>
public record MeCabMorpheme(string Surface, string PartsOfSpeech, string PartsOfSpeechSection);

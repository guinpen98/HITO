using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NMeCab.Specialized;

namespace HITO.NLP.NLU
{
    /// <summary>
    /// 形態素解析
    /// </summary>
    public static class MorphologicalAnalysis
    {
        private const string DictionaryDir = @"Assets/AssetStoreTools/dic/ipadic";

        /// <summary>
        /// 解析する
        /// </summary>
        /// <param name="text">解析する対象の文字列</param>
        public static MeCabIpaDicNode[] Analyze(string text)
        {
            MeCabIpaDicNode[] nodes;
            using (var tagger = MeCabIpaDicTagger.Create(DictionaryDir))
            {
                nodes = tagger.Parse(text);
            }
            return nodes;
        }
    }
}
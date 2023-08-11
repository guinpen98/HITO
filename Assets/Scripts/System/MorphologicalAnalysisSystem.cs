using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NMeCab.Specialized;

namespace HITO.System
{
    /// <summary>
    /// 形態素解析
    /// </summary>
    public class MorphologicalAnalysisSystem : BaseSystem
    {
        private const string DictionaryDir = @"Assets/AssetStoreTools/dic/ipadic";

        public override void SetEvent()
        {
            _gameEvent.OnInput += Analyze;
        }

        /// <summary>
        /// 解析する
        /// </summary>
        /// <param name="text">解析する対象の文字列</param>
        public void Analyze(string text)
        {
            using (var tagger = MeCabIpaDicTagger.Create(DictionaryDir))
            {
                var nodes = tagger.Parse(text);

                _gameEvent.OnMorphologicalAnalyze.Invoke(nodes);
            }
        }
    }
}
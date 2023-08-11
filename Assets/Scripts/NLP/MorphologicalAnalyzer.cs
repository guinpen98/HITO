using System.Collections.Generic;
using System.Linq;
using NMeCab.Specialized;

namespace HITO.NLP.NLU
{
    /// <summary>
    /// 形態素解析器
    /// </summary>
    public static class MorphologicalAnalyzer
    {
        private const string DictionaryDir = @"Assets/AssetStoreTools/dic/ipadic";

        /// <summary>
        /// 形態素解析
        /// </summary>
        /// <param name="text">解析する対象の文字列</param>
        public static List<Morpheme> Analyze(string text)
        {
            List<Morpheme> nodes;
            using (var tagger = MeCabIpaDicTagger.Create(DictionaryDir))
            {
                nodes = tagger.Parse(text).Select(x => new Morpheme()
                {
                    Surface = x.Surface,
                    PartsOfSpeech = x.PartsOfSpeech,
                    PartsOfSpeechSection1 = x.PartsOfSpeechSection1
                }).ToList();
            }
            return nodes;
        }
    }
}
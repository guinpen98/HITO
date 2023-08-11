using System.Collections.Generic;
using System.Linq;

namespace HITO.NLP
{
    /// <summary>
    /// 形態素
    /// </summary>
    public class Morpheme
    {
        public string Surface { get; set; }
        public string PartsOfSpeech { get; set; }
        public string PartsOfSpeechSection1 { get; set; }
    }

    /// <summary>
    /// 文章
    /// </summary>
    public class Sentence
    {
        private class TypePair
        {
            public string Forward { get; init; }
            public string Backward { get; init; }
        }

        private static readonly List<string> _removeTypes = new List<string> { "副詞", "接続詞", "感動詞", "記号", "フィラー", "その他", "未知語" };
        private static readonly List<string> _combineTypes = new List<string> { "連体詞", "接頭詞" };
        private static readonly List<TypePair> _removeTypePairs = new List<TypePair> { };
        private static readonly List<TypePair> _combineTypePairs = new List<TypePair>
        {
            new TypePair { Forward = "名詞", Backward = "名詞" },
            new TypePair { Forward = "助動詞", Backward = "助詞" },
            new TypePair { Forward = "助動詞", Backward = "助動詞" },
            new TypePair { Forward = "動詞", Backward = "助詞" },
            new TypePair { Forward = "動詞", Backward = "動詞" },
            new TypePair { Forward = "形容詞", Backward = "助動詞" },
            new TypePair { Forward = "形容詞", Backward = "助詞" },
        };

        private List<Morpheme> _nodes;

        public Sentence(List<Morpheme> nodes)
        {
            this._nodes = nodes;
        }

        /// <summary>
        /// 前処理
        /// </summary>
        public Sentence Preprocess()
        {
            DealUnnecessaryTypes();
            return this;
        }

        public bool IsRemove(string type)
            => _removeTypes.Contains(type);
        public bool IsRemove(string PartsOfSpeech, string PartsOfSpeechSection1)
            => _removeTypePairs.Any(pair => pair.Forward == PartsOfSpeech && pair.Backward == PartsOfSpeechSection1);

        public bool IsCombine(string type)
            => _combineTypes.Contains(type);
        public bool IsCombine(string PartsOfSpeech, string PartsOfSpeechSection1)
            => _combineTypePairs.Any(pair => pair.Forward == PartsOfSpeech && pair.Backward == PartsOfSpeechSection1);

        /// <summary>
        /// 形態素の数
        /// </summary>
        public int Size => _nodes.Count;

        public bool IsThisType(string type)
        {
            if (_nodes.Count != 1) return false;
            return _nodes[0].PartsOfSpeech == type || _nodes[0].PartsOfSpeechSection1 == type;
        }
        public bool IsThisType(string type, int index)
        {
            if (_nodes.Count <= index) return false;
            return _nodes[index].PartsOfSpeech == type || _nodes[index].PartsOfSpeechSection1 == type;
        }

        public override string ToString()
            => string.Join("", _nodes.Select(m => m.Surface));

        public string GetMorphemes(int index)
            => _nodes[index].Surface;

        public (Sentence, Sentence) SplitSentence(int n)
        {
            var _nodes1 = _nodes.Take(n).ToList();
            var _nodes2 = _nodes.Skip(n).ToList();

            return (new Sentence(_nodes1), new Sentence(_nodes2));
        }

        /// <summary>
        /// 必要のない形態素を除外
        /// </summary>
        private void DealUnnecessaryTypes()
        {
            for (int i = 0; i < _nodes.Count; ++i)
            {
                if (IsRemove(_nodes[i].PartsOfSpeech))
                {
                    _nodes.RemoveAt(i);
                    i--;
                }
                else if (IsCombine(_nodes[i].PartsOfSpeech) && i + 1 < _nodes.Count)
                {
                    _nodes[i + 1].Surface = _nodes[i].Surface + _nodes[i + 1].Surface;
                    _nodes.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < _nodes.Count - 1; ++i)
            {
                if (IsRemove(_nodes[i].PartsOfSpeech, _nodes[i + 1].PartsOfSpeech))
                {
                    _nodes.RemoveAt(i + 1);
                    i--;
                }
                else if (IsCombine(_nodes[i].PartsOfSpeech, _nodes[i + 1].PartsOfSpeech))
                {
                    _nodes[i].Surface += _nodes[i + 1].Surface;
                    _nodes.RemoveAt(i + 1);
                    i--;
                }
            }
        }

    }
}

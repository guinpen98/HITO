using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HITO.NLP.NLU
{
    /// <summary>
    /// 句構造タイプ
    /// </summary>
    public enum PhraseStructureType
    {
        S, NP, VP, N, V, VA, AP, PP
    }

    /// <summary>
    /// 句構造タイプノード
    /// </summary>
    public class PhraseStructureTypeNode
    {
        public PhraseStructureType Type { get; init; }
        public PhraseStructureTypeNode TypeNode1 { get; init; }
        public PhraseStructureTypeNode TypeNode2 { get; init; }
    }

    /// <summary>
    /// 句構造解析器
    /// </summary>
    public static class PhraseStructureAnalyzer
    {
        public static string Analyze(Sentence sentence)
            => IsS(sentence);

        private static string Check(Sentence sentence, Func<Sentence, string> f1, Func<Sentence, string> f2, int n, string name)
        {
            var splitResults = sentence.SplitSentence(n);
            var sentence1 = splitResults.Item1;
            var sentence2 = splitResults.Item2;

            var output1 = f1(sentence1);
            if (string.IsNullOrEmpty(output1)) return output1;

            var output2 = f2(sentence2);
            if (string.IsNullOrEmpty(output2)) return output2;

            return '(' + name + ' ' + output1 + ' ' + output2 + ')';
        }

        private static string IsV(Sentence sentence)
        {
            if (sentence.Size != 1 && sentence.Size != 2) return string.Empty;
            if (sentence.Size == 1)
                if (sentence.IsThisType("動詞")) return "(V " + sentence.ToString() + ")";
            if (sentence.Size == 2)
            {
                var (sentence1, sentence2) = sentence.SplitSentence(1);
                if (sentence1.IsThisType("動詞") && sentence2.IsThisType("助動詞")) return "(V " + sentence.ToString() + ")";
            }
            return string.Empty;
        }

        private static string IsVA(Sentence sentence)
        {
            if (sentence.Size != 1) return string.Empty;
            if (sentence.IsThisType("助動詞")) return "(VA " + sentence.ToString() + ")";
            return string.Empty;
        }

        private static string IsPP(Sentence sentence)
        {
            if (sentence.Size != 1) return string.Empty;
            if (sentence.IsThisType("助詞")) return "(PP " + sentence.ToString() + ")";
            return string.Empty;
        }

        private static string IsVP(Sentence sentence)
        {
            if (sentence.Size < 2) return string.Empty;

            string output = Check(sentence, IsN, IsVA, sentence.Size - 1, "VP");
            if (!string.IsNullOrEmpty(output)) return output;

            return string.Empty;
        }

        private static string IsNP(Sentence sentence)
        {
            if (sentence.Size < 2) return string.Empty;

            string output = Check(sentence, IsN, IsPP, sentence.Size - 1, "NP");
            if (!string.IsNullOrEmpty(output)) return output;
            return string.Empty;
        }

        private static string IsAP(Sentence sentence)
        {
            if (sentence.Size == 0) return string.Empty;
            if (sentence.Size == 1)
            {
                if (sentence.IsThisType("形容詞")) return "(AP " + sentence.ToString() + ")";
                return string.Empty;
            }

            string output = Check(sentence, IsN, IsPP, sentence.Size - 1, "AP"); // 連体化の確認
            if (!string.IsNullOrEmpty(output)) return output;

            output = Check(sentence, IsAP, IsAP, 1, "AP");
            if (!string.IsNullOrEmpty(output)) return output;

            for (int i = 2; i <= sentence.Size - 1; i++)
            {
                output = Check(sentence, IsNP, IsV, i, "AP");
                if (!string.IsNullOrEmpty(output)) return output;
            }
            return string.Empty;
        }

        private static string IsN(Sentence sentence)
        {
            if (sentence.Size == 0) return string.Empty;
            if (sentence.Size == 1)
            {
                if (sentence.IsThisType("名詞")) return "(N " + sentence.ToString() + ")";
                return string.Empty;
            }

            string output = Check(sentence, IsAP, IsN, sentence.Size - 1, "N");
            if (!string.IsNullOrEmpty(output)) return output;

            output = Check(sentence, IsV, IsN, 1, "N");
            if (!string.IsNullOrEmpty(output)) return output;

            return string.Empty;
        }

        private static string IsS(Sentence sentence)
        {
            string output;

            for (int i = 2; i <= sentence.Size - 1; i++)
            {
                output = Check(sentence, IsNP, IsAP, i, "S");
                if (!string.IsNullOrEmpty(output)) return output;
            }

            for (int i = 2; i <= sentence.Size - 1; i++)
            {
                output = Check(sentence, IsNP, IsN, i, "S");
                if (!string.IsNullOrEmpty(output)) return output;
            }

            for (int i = 2; i <= sentence.Size - 2; i++)
            {
                output = Check(sentence, IsNP, IsVP, i, "S");
                if (!string.IsNullOrEmpty(output)) return output;
            }

            output = IsV(sentence);
            if (!string.IsNullOrEmpty(output)) return output;

            output = IsAP(sentence);
            if (!string.IsNullOrEmpty(output)) return output;

            output = IsVP(sentence);
            if (!string.IsNullOrEmpty(output)) return output;

            output = IsN(sentence);
            if (!string.IsNullOrEmpty(output)) return output;

            if (sentence.Size < 2) return sentence.ToString();
            output = Check(sentence, IsNP, IsV, sentence.Size - 1, "S");
            if (!string.IsNullOrEmpty(output)) return output;

            if (sentence.Size < 3) return sentence.ToString();
            output = Check(sentence, IsNP, IsV, sentence.Size - 2, "S");
            if (!string.IsNullOrEmpty(output)) return output;

            return string.Empty;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Chat
{
    public static class JumanKNPParser
    {
        public static async Task<Clause[]> ParseTextAsync(string text)
        {
            // 文字列をShift_JISにエンコード
            byte[] bytes = Encoding.GetEncoding("shift_jis").GetBytes(text);
            string shiftJisString = Encoding.GetEncoding("shift_jis").GetString(bytes);

            // 一時ファイルのパス
            string tempFilePath = Path.GetTempFileName();

            // 一時ファイルにShift_JISエンコードされたテキストを書き込み
            File.WriteAllText(tempFilePath, shiftJisString, Encoding.GetEncoding("shift_jis"));

            try
            {
                // JUMANを呼び出すプロセス情報
                var jumanOutputPath = $"{tempFilePath}.juman";
                var processJuman = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c juman < \"{tempFilePath}\" > \"{jumanOutputPath}\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                var jumanProcess = Process.Start(processJuman);
                await Task.Run(() => jumanProcess.WaitForExit());

                // エラーの確認
                string jumanErrors = await jumanProcess.StandardError.ReadToEndAsync();
                if (!string.IsNullOrEmpty(jumanErrors))
                {
                    throw new Exception("JUMAN Error: " + jumanErrors);
                }

                // KNPを呼び出すプロセス情報
                var processKNP = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c knp -tab < \"{jumanOutputPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.GetEncoding("shift_jis")
                };

                var knpProcess = Process.Start(processKNP);
                string knpOutput = await knpProcess.StandardOutput.ReadToEndAsync();
                string knpErrors = await knpProcess.StandardError.ReadToEndAsync();

                await Task.Run(() => knpProcess.WaitForExit());

                if (!string.IsNullOrEmpty(knpErrors))
                {
                    throw new Exception("KNP Error: " + knpErrors);
                }

                // return knpOutput;

                // parse KNP output
                UnityEngine.Debug.Log(knpOutput);
                Clause[] clauses = ParseResult(knpOutput);
                foreach (var clause in clauses)
                {
                    UnityEngine.Debug.Log(clause);
                }
                return clauses;
            }
            finally
            {
                // 一時ファイルの削除
                File.Delete(tempFilePath);
                File.Delete($"{tempFilePath}.juman");
            }
        }

        public static Clause[] ParseResult(string knpOutput)
        {
            List<Clause> clauses = new List<Clause>();
            Clause currentClause = null;

            // 最初と最後の行は削除

            foreach (string line in knpOutput.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("*"))
                {
                    if (currentClause != null)
                    {
                        clauses.Add(currentClause);
                    }
                    currentClause = new Clause();
                    string mainRep = line.Split(new[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries).Reverse().ElementAt(1).Split(':')[1];
                    currentClause.SetMainRepresentation(mainRep);
                }
                else if (line.StartsWith("+"))
                {
                    // 追加の構文解析情報をここで扱う
                }
                else if (!line.StartsWith("EOS") && !line.StartsWith("#"))
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 6)
                    {
                        var morpheme = new Morpheme(
                            parts[0],  // Surface
                            parts[1],  // Reading
                            parts[2],  // Base form
                            ParsePartOfSpeech(parts[4]),  // Part of Speech
                            new Features(string.Join(" ", parts[6..]))  // Features
                        );
                        currentClause?.AddMorpheme(morpheme);
                    }
                }
            }

            if (currentClause != null)
            {
                clauses.Add(currentClause);
            }

            return clauses.ToArray();
        }

        private static PartOfSpeech ParsePartOfSpeech(string posCode)
        {
            return (PartOfSpeech)int.Parse(posCode);
        }
    }

    /// <summary>
    /// 形態素
    /// </summary>
    [System.Serializable]
    public class Morpheme
    {
        /// <summary>表層形</summary>
        public string Surface { get; init; }
        /// <summary>読み</summary>
        public string Reading { get; init; }
        /// <summary>基本形</summary>
        public string BaseForm { get; init; }
        /// <summary>品詞</summary>
        public PartOfSpeech PartOfSpeech { get; init; }
        /// <summary>活用形</summary>
        public Features Features { get; set; }

        public Morpheme(string surface, string reading, string baseForm, PartOfSpeech partOfSpeech, Features features)
        {
            Surface = surface;
            Reading = reading;
            BaseForm = baseForm;
            PartOfSpeech = partOfSpeech;
            Features = features;
        }

        public override string ToString()
        {
            return $"Surface: {Surface}, Base: {BaseForm}, POS: {PartOfSpeech}, Features: {Features}";
        }
    }

    /// <summary>
    /// 文節
    /// </summary>
    [System.Serializable]
    public class Clause
    {
        /// <summary>形態素</summary>
        public List<Morpheme> Morphemes { get; set; }
        /// <summary>主辞表記</summary>
        public string MainRepresentation { get; set; }

        public Clause()
        {
            Morphemes = new List<Morpheme>();
        }

        public void AddMorpheme(Morpheme morpheme)
        {
            Morphemes.Add(morpheme);
        }

        public void SetMainRepresentation(string mainRep)
        {
            MainRepresentation = mainRep;
        }

        public override string ToString()
        {
            return $"Main Rep: {MainRepresentation}, Morphemes: {string.Join(", ", Morphemes)}";
        }
    }

    /// <summary>
    /// 形態素の特徴
    /// </summary>
    [System.Serializable]
    public class Features
    {
        /// <summary>代表表記</summary>
        public string RepresentativeNotation { get; init; }
        /// <summary>カテゴリ</summary>
        public string Category { get; init; }
        /// <summary>自立</summary>
        public bool IsIndependent { get; init; }
        /// <summary>内容語</summary>
        public bool IsContentWord { get; init; }
        /// <summary>タグ単位始</summary>
        public bool IsStartOfTagUnit { get; init; }
        /// <summary>文節始</summary>
        public bool IsStartOfPhrase { get; init; }
        /// <summary>文節主辞</summary>
        public bool IsMainOfPhrase { get; init; }

        public Features(string featureStr)
        {
            RepresentativeNotation = Regex.Match(featureStr, "代表表記:([^\\s<]+)").Groups[1].Value;
            Category = Regex.Match(featureStr, "カテゴリ:([^\\s<]+)").Groups[1].Value;
            IsIndependent = featureStr.Contains("<自立>");
            IsContentWord = featureStr.Contains("<内容語>");
            IsStartOfTagUnit = featureStr.Contains("<タグ単位始>");
            IsStartOfPhrase = featureStr.Contains("<文節始>");
            IsMainOfPhrase = featureStr.Contains("<文節主辞>");
        }

        public override string ToString()
        {
            return $"Rep: {RepresentativeNotation}, Cat: {Category}, Ind: {IsIndependent}, Cont: {IsContentWord}, TagStart: {IsStartOfTagUnit}, PhraseStart: {IsStartOfPhrase}, MainPhrase: {IsMainOfPhrase}";
        }
    }

    /// <summary>
    /// 品詞
    /// </summary>
    public enum PartOfSpeech
    {
        Special = 1, // 特殊
        Verb = 2, // 動詞
        Adjective = 3, // 形容詞
        Copula = 4, // 判定詞
        AuxiliaryVerb = 5, // 助動詞
        Noun = 6, // 名詞
        Demonstrative = 7, // 指示詞
        Adverb = 8, // 副詞
        Particle = 9, // 助詞
        Conjunction = 10, // 接続詞
        Adnominal = 11, // 連体詞
        Interjection = 12, // 感動詞
        Prefix = 13, // 接頭辞
        Suffix = 14, // 接尾辞
        Undefined = 15 // 未定義語
    }
}

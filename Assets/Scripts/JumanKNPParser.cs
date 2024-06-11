using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class JumanKNPParser
{
    public static async Task<string> ParseTextAsync(string text)
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
            var sentence = ParseResult(knpOutput);
            var sentenceJson = JsonUtility.ToJson(sentence);
            return sentenceJson;
        }
        finally
        {
            // 一時ファイルの削除
            File.Delete(tempFilePath);
            File.Delete($"{tempFilePath}.juman");
        }
    }

    public static Sentence ParseResult(string knpOutput)
    {
        Sentence sentence = new Sentence();
        Phrase currentPhrase = new Phrase();

        foreach (string line in knpOutput.Split('\n'))
        {
            if (line.StartsWith("+ "))
            {
                // 新しい基本句の開始
                if (currentPhrase.Morphemes.Count > 0)
                {
                    sentence.Phrases.Add(currentPhrase);
                    currentPhrase = new Phrase();
                }
            }
            else if (line.StartsWith("* "))
            {
                // 新しい文節の開始（ここでは無視）
            }
            else if (!line.StartsWith("#") && !string.IsNullOrWhiteSpace(line))
            {
                // "#" で始まる行や空行は無視する
                string[] parts = line.Split(' ');
                if (parts.Length >= 4)
                {
                    // 形態素の情報が正しく4部分に分割されているか確認
                    currentPhrase.Morphemes.Add(new Morpheme(parts[0], parts[1], parts[2], parts[3]));
                }
            }
        }

        if (currentPhrase.Morphemes.Count > 0)
        {
            sentence.Phrases.Add(currentPhrase);
        }

        return sentence;
    }
}

[System.Serializable]
public record Morpheme
{
    /// <summary>表層形</summary>
    public string Surface;
    /// <summary>品詞</summary>
    public string PartOfSpeech;
    /// <summary>活用形</summary>
    public string Inflection;
    /// <summary>基本形</summary>
    public string BaseForm;

    public Morpheme(string surface, string partOfSpeech, string inflection, string baseForm)
    {
        Surface = surface;
        PartOfSpeech = partOfSpeech;
        Inflection = inflection;
        BaseForm = baseForm;
    }
}

[System.Serializable]
public class Phrase
{
    public List<Morpheme> Morphemes;

    public Phrase()
    {
        Morphemes = new List<Morpheme>();
    }
}

[System.Serializable]
public class Sentence
{
    public List<Phrase> Phrases;

    public Sentence()
    {
        Phrases = new List<Phrase>();
    }
}

using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class JumanKNP : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CallJumanKNP("これはテストの文です。"));
    }

    private IEnumerator CallJumanKNP(string text)
    {
        // 文字列をShift_JISにエンコード
        byte[] bytes = Encoding.GetEncoding("shift_jis").GetBytes(text);
        string shiftJisString = Encoding.GetEncoding("shift_jis").GetString(bytes);

        // 一時ファイルのパス
        string tempFilePath = Path.GetTempFileName();

        // 一時ファイルにShift_JISエンコードされたテキストを書き込み
        File.WriteAllText(tempFilePath, shiftJisString, Encoding.GetEncoding("shift_jis"));

        var processJuman = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c juman < \"{tempFilePath}\" > \"{tempFilePath}.juman\"",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var jumanProcess = Process.Start(processJuman);
        jumanProcess.WaitForExit();

        string jumanErrors = jumanProcess.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(jumanErrors))
        {
            UnityEngine.Debug.LogError("JUMAN Error: " + jumanErrors);
            yield break;
        }

        var processKNP = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c knp -tab < \"{tempFilePath}.juman\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.GetEncoding("shift_jis")
        };

        var knpProcess = Process.Start(processKNP);
        string knpOutput = knpProcess.StandardOutput.ReadToEnd();
        string knpErrors = knpProcess.StandardError.ReadToEnd();

        knpProcess.WaitForExit();

        // 一時ファイルの確認
        if (File.Exists($"{tempFilePath}.juman"))
        {
            UnityEngine.Debug.Log("JUMAN Output: " + File.ReadAllText($"{tempFilePath}.juman", Encoding.GetEncoding("shift_jis")));
        }
        else
        {
            UnityEngine.Debug.LogError("JUMAN output file does not exist.");
        }

        // 一時ファイルの削除
        File.Delete(tempFilePath);
        File.Delete($"{tempFilePath}.juman");

        if (!string.IsNullOrEmpty(knpErrors))
        {
            UnityEngine.Debug.LogError($"KNP Error: {knpErrors}");
        }
        else
        {
            UnityEngine.Debug.Log($"KNP Output: {knpOutput}");
        }

        yield return null;
    }
}

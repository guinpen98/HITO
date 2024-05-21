using HITO.NLP.NLU;
using UnityEngine;

namespace HITO.NLP
{
    /// <summary>
    /// NLPアプリケーション
    /// </summary>
    public class NLPApp : MonoBehaviour, INLU, INLG
    {

        public NLUResponse NLU(NLURequest request)
        {
            var morphologicalAnalysisResult = MorphologicalAnalyzer.Analyze(request.Input);

            var sentence = new Sentence(morphologicalAnalysisResult).Preprocess();

            var result = PhraseStructureAnalyzer.Analyze(sentence);

            return new NLUResponse(request.Input, result);
        }

        public NLGResponse NLG(NLGRequest request)
        {
            return new NLGResponse(request.Input);
        }


    }
}

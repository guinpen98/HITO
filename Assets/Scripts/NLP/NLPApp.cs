using HITO.NLP.NLU;
using UnityEngine;

namespace HITO.NLP
{
    public class NLPApp : MonoBehaviour, INLU, INLG
    {

        public NLUResponse NLU(NLURequest request)
        {
            var morphologicalAnalysisResult = MorphologicalAnalysis.Analyze(request.Input);

            var sentence = new Sentence(morphologicalAnalysisResult).Preprocess();

            return new NLUResponse(sentence.ToString());
        }

        public NLGResponse NLG(NLGRequest request)
        {
            return new NLGResponse(request.Input);
        }


    }
}

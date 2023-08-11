using System.Linq;
using HITO.NLP.NLU;
using UnityEngine;
using static HITO.NLP.Sentence;

namespace HITO.NLP
{
    public class NLPApp : MonoBehaviour, INLU, INLG
    {

        public NLUResponse NLU(NLURequest request)
        {
            //var morphologicalAnalysisResult = MorphologicalAnalysis.Analyze(request.Input);
            //var nodes = morphologicalAnalysisResult.Select(x => new Morpheme()
            //{
            //    Surface = x.Surface,
            //    PartsOfSpeech = x.PartsOfSpeech,
            //    PartsOfSpeechSection1 = x.PartsOfSpeechSection1
            //}).ToList();
            //var sentence = new Sentence(nodes).Preprocess();

            //return new NLUResponse(sentence.ToString());
            return new NLUResponse(request.Input);
        }

        public NLGResponse NLG(NLGRequest request)
        {
            return new NLGResponse();
        }


    }
}

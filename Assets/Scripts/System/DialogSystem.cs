using System.Collections;
using System.Collections.Generic;
using HITO.NLP;
using UnityEngine;

namespace HITO.System
{
    public class DialogSystem : BaseSystem
    {
        public override void SetEvent()
        {
            _gameEvent.OnInputToDialogSystem += UnderstandText;
            _gameEvent.ResponseNLU += DealNLUResponse;
        }

        public void UnderstandText(string input)
        {
            var NLURequest = new NLURequest(input);
            _gameEvent.RequestNLU.Invoke(NLURequest);
        }

        public void DealNLUResponse(NLUResponse response)
        {
            Debug.Log($"NLU result, Input:{response.Input}");
        }
    }
}
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
            _gameEvent.ResponseNLG += DealNLGResponse;
        }

        public void UnderstandText(string input)
        {
            var NLURequest = new NLURequest(input);
            _gameEvent.RequestNLU.Invoke(NLURequest);
        }

        public void DealNLUResponse(NLUResponse response)
        {
            var NLGRequest = new NLGRequest(response.Result);
            _gameEvent.RequestNLG.Invoke(NLGRequest);
        }

        public void DealNLGResponse(NLGResponse response)
        {
            _gameEvent.SetCharacterText.Invoke(response.Input);
        }
    }
}
using System;

namespace HITO
{
    /// <summary>
    /// イベント定義クラス
    /// </summary>
    public class GameEvent
    {
        /// <summary>文章を入力として受け取る</summary>
        public Action<string> OnInputText;
        /// <summary>会話システムに文章を入力</summary>
        public Action<string> OnInputToDialogSystem;
        /// <summary>言語理解をリクエスト</summary>
        public Action<NLURequest> RequestNLU;
        /// <summary>言語理解結果を返す</summary>
        public Action<NLUResponse> ResponseNLU;
        /// <summary>言語生成をリクエスト</summary>
        public Action<NLGRequest> RequestNLG;
        /// <summary>言語生成結果を返す</summary>
        public Action<NLGResponse> ResponseNLG;
        /// <summary>キャラクターのテキストを設定する</summary>
        public Action<string> SetCharacterText;
    }
}
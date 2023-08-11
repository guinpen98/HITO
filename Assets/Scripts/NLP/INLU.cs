namespace HITO.NLP
{
    /// <summary>
    /// 自然言語生成のインターフェイス
    /// </summary>
    interface INLU
    {
        /// <summary>自然言語生成</summary>
        NLUResponse NLU(NLURequest request);
    }

    public record NLURequest(string Input);
    public record NLUResponse(string Input);
}

namespace HITO
{
    /// <summary>
    /// 自然言語理解のインターフェイス
    /// </summary>
    interface INLG
    {
        /// <summary>自然言語理解</summary>
        NLGResponse NLG(NLGRequest request);
    }

    public record NLGRequest(string Input);
    public record NLGResponse(string Input);
}

namespace TT.Models.Util
{
    public struct TempFileScheme
    {
        public TempFileType Type;
        public string TempPath;
        public string NameScheme;
    }

    public enum TempFileType { ReportPreview, OxyPlot, Image }
}

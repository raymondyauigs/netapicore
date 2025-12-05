namespace dotapiBase.Common
{
    public class Result
    {
        public string InputCmdFile { get; set; } = string.Empty;
        public bool Success { get; set; } = false;

        public bool Fail { get { return !Success; } }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

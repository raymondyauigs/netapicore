namespace dotapiBase.Common
{
    public class AuthSetting
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public List<string> CorsHosts { get; set; } = new List<string>();

        public List<string> CmdFilePaths { get; set;} = new List<string>();

        public List<string> OutFilePaths { get; set; } = new List<string>();    
    }
}






using BootlegRealists.Reporting;

namespace dotapiBase.DocUtils
{
    public class ConvertHelper
    {
        
        public ConvertHelper()
        {
            


        }

        public bool Convert(string cmdfile,out string error)
        {
            var cmdinout = new string[] { }.ToList();
            error = string.Empty;
            try
            {
                if (File.Exists(cmdfile))
                {
                    foreach (var line in File.ReadLines(cmdfile))
                    {
                        if (string.IsNullOrEmpty(line))
                            continue;
                        cmdinout = line.Split('|').ToList();

                        using var docxStream = new FileStream(cmdinout[0], FileMode.Open, FileAccess.Read, FileShare.Read);
                        using var pdfStream = new FileStream(cmdinout[1], FileMode.Create, FileAccess.Write, FileShare.Write);
                        var docxToPdf = new DocxToPdf();
                        var runProperties = new Dictionary<string, string> { ["Title"] = "title", ["UserName"] = "userName" };
                        docxToPdf.Execute(docxStream, pdfStream, runProperties);
                        break;
                    }
                }


              

                return true;
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}

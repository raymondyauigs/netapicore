


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

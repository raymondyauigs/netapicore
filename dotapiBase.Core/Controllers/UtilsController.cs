using dotapiBase.Common;
using dotapiBase.Core.Model;
using dotapiBase.DocUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace dotapiBase.Core.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UtilsController : ControllerBase
    {
        private readonly ILogger<UtilsController> itlogger;
        private readonly ConvertHelper itconverthelper;
        private readonly AuthSetting itSetting;

        public UtilsController(ILogger<UtilsController> logger,ConvertHelper converthelper,IOptions<AuthSetting> setting)
        {
            itlogger = logger;
            itconverthelper = converthelper;
            itSetting = setting.Value;
        }
        [HttpGet("convertfile")]
        public Result Get([FromQuery]string filename, [FromQuery]string target=Constants.DefaultTarget)
        {
            string cmdfile = string.Empty;
            var targetIndex = itSetting.CmdTargets.FindIndex(e => e == target);
            if(targetIndex <0)
            {
                return new Result { Success = false, ErrorMessage = $"Target '{target}' is not supported." };
            }
            cmdfile = Path.Combine(itSetting.CmdFilePaths[targetIndex], filename);


            var result = new Result { Success=true , InputCmdFile = cmdfile};
            var converted = itconverthelper.Convert(result.InputCmdFile,out var error);
            if (!converted.IsError) {

                var outfile = converted.Value;

                result.DownloadUrl= Url.Action("Donwload", "Utils", new { target = itSetting.CmdTargets[targetIndex], filename = Path.GetFileName(outfile) }, Request.Scheme);


                return result;
            }
            
            result.ErrorMessage = converted.FirstError.ToString();

            //itlogger.LogError("Convert file error:{0}", error);

            return result;

        }
        [HttpGet("download")]
        public IActionResult Donwload(string target,string filename,bool inlineForced=false)
        {
            string downloadfile = string.Empty;
            string subpath = string.Empty;
            string filetype = string.Empty;
            bool isinline = false;
            var targetIndex = itSetting.CmdTargets.FindIndex(e => e == target);
            if (targetIndex < 0)
            {
                throw new DomainException($"Target '{target}' is not supported.", "504");
            }
            switch(Path.GetExtension(filename))
            {
                case ".pdf":
                    subpath = "pdf";
                    filetype = "application/pdf";
                    isinline = true;
                    break;
                case ".docx":
                case ".doc":
                    subpath = "word";
                    filetype = "application/vnd.ms-word";
                    break;
                    case ".xlsx":
                    case ".xls":
                    subpath = "excel";
                    filetype = "application/vnd.ms-excel";
                    break;
                case ".zip":
                    subpath = "zip";
                    filetype = "application/zip";
                    break;
                default:
                    throw new DomainException($"File type '{Path.GetExtension(filename)}' is not supported.", "505");

            }
            downloadfile = Path.Combine(itSetting.OutFilePaths[targetIndex],subpath, filename);
            if (!System.IO.File.Exists(downloadfile))
            {
                throw new DomainException($"File '{filename}' is not found.", "506");
            }
            // Open the file stream asynchronously
            var stream = new FileStream(downloadfile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);

            if(isinline && inlineForced)
            {
                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = isinline  // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Append("Content-Disposition", cd.ToString());
                Response.Headers.Append("X-Content-Type-Options", "nosniff");
                return File(stream, filetype);
            }
            
            // Return the file stream with appropriate content type and disposition

            return File(stream, filetype, filename,enableRangeProcessing:true);
        }

    }
}

using dotapiBase.Common;
using dotapiBase.Core.Model;
using dotapiBase.DocUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        public Result Get([FromQuery]string filename)
        {
            string cmdfile = string.Empty;
            foreach(var cmd in itSetting.CmdFilePaths.Select(e=> Path.Combine(e,filename)))
            {
                if(System.IO.File.Exists(cmd))
                {
                    cmdfile = cmd;
                    break;
                }
            }

            var result = new Result { Success=true , InputCmdFile = cmdfile};
            var success = itconverthelper.Convert(result.InputCmdFile,out var error);
            if (success) {


                return result;
            }
            
            result.ErrorMessage = error;

            //itlogger.LogError("Convert file error:{0}", error);

            return result;

        }

    }
}

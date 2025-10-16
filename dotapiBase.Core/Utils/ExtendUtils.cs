using dotapiBase.Core.Middleware;

namespace dotapiBase.Core.Utils
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExHandleMiddleware>();
        }
    }
    public static class ExtendUtils
    {


    }

    public static class TypeHelper
    {
        public static string? ToNameString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }
    }
}

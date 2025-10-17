namespace dotapiBase.Core.Middleware
{
    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExHandleMiddleware>();
        }
    }
}

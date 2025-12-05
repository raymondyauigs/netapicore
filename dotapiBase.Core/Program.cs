using dotapiBase.Common;
using dotapiBase.Core.Model;
using dotapiBase.Common.Utils;
using dotapiBase.DocUtils;
using Serilog;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;

/*Bootstrap logger
 */
Log.Logger = new LoggerConfiguration().MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.GetName().Name,
    ContentRootPath = Directory.GetCurrentDirectory(),
    //WebRootPath = "wwwroot",

});

var authsetting = builder.Configuration.GetSection(Constants.WebKey.AuthSetting.ToNameString()!);
var encryptionService = new StringEncrypService();
authsetting[nameof(AuthSetting.Secret)] = encryptionService.EncryptString(authsetting[nameof(AuthSetting.SecretKey)] ?? "");

builder.Services.Configure<AuthSetting>(authsetting);

builder.Services.Configure<FormOptions>(opt =>
{

    opt.BufferBodyLengthLimit = 512 * 1024 * 1024;

    //it needs
    opt.MultipartBodyLengthLimit = 512 * 1024 * 1024;

});

builder.Services.Configure<IISServerOptions>(opt =>
{
    opt.MaxRequestBodySize = 512 * 1024 * 1024;

});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddSingleton(_ => new StringEncrypService());
builder.Services.AddSingleton(_ => new ConvertHelper());
builder.Services.AddScoped<ITokenService, TokenService>();


/*UseSerilog configuration
 */
builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
.ReadFrom.Configuration(context.Configuration)
.ReadFrom.Services(services)
//.WriteTo.Console(new ExpressionTemplate(
//    // Include trace and span ids when present.
//    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
//
);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Security
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(authsetting.GetSection(nameof(AuthSetting.CorsHosts)).Get<string[]>() ?? Array.Empty<string>())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("Content-Disposition");
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*Use SerilogRequestLogging
 */
app.UseSerilogRequestLogging(option =>
{
    option.EnrichDiagnosticContext = (diagnostic, http) =>
    {
        diagnostic.Set("LocalTime", DateTime.Now.ToString("yyyyMMdd+HHmmss"));

    };
});

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();

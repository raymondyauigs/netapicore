using dotapiBase.Core.Model;
using dotapiBase.Core.Utils;

var builder = WebApplication.CreateBuilder(args);
var authsetting = builder.Configuration.GetSection(Constants.WebKey.AuthSetting.ToNameString()!);
var encryptionService = new StringEncrypService();
authsetting[nameof(AuthSetting.Secret)] = encryptionService.EncryptString(authsetting[nameof(AuthSetting.SecretKey)] ?? "");

builder.Services.Configure<AuthSetting>(authsetting);



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
        .AllowCredentials();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();

using AIAssistanceAPI;
using DBUtility;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Utility;

var builder = WebApplication.CreateBuilder(args);
// configure services
//builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;    
});

//builder.Logging.AddLog4Net("log4net.config");
XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HISServiceAPI", Version = "v1" });
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<AppLocalSetting>(builder.Configuration.GetSection("AppLocalSettings"));
builder.Services.AddSingleton<IAppLocalSetting>(sp => sp.GetRequiredService<IOptions<AppLocalSetting>>().Value);

// Add App Glocal Settings 
var filePath = @"C:\AppConfigs";
builder.Configuration.SetBasePath(filePath)
                        .AddJsonFile("appglobalsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
builder.Services.Configure<AppGlobalSetting>(builder.Configuration.GetSection("AppGlobalSettings"));
builder.Services.AddSingleton<IAppGlobalSetting>(sp => sp.GetRequiredService<IOptions<AppGlobalSetting>>().Value);

var app = builder.Build();

Logger.WriteInfoLog("API Started");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

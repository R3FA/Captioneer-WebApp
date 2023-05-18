using API.Data;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UtilityService.Utils;
using Microsoft.AspNetCore.StaticFiles;
using API.Hubs;

var builder = WebApplication.CreateBuilder(args);

LoggerManager.LoadConfiguration();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
LoggerManager.GetInstance().LogInfo("Loaded Nlog configuration");

var vaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
var clientId = builder.Configuration["AzureKeyVault:ClientId"];
var tenantId = builder.Configuration["AzureKeyVault:TenantId"];
var clientSecret = builder.Configuration["AzureKeyVault:ClientSecret"];

var corsPolicyName = "corsPolicy";

builder.Host.ConfigureAppConfiguration(builder =>
{
    var credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
    var client = new SecretClient(new Uri(vaultUrl), credentials);
    builder.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
    LoggerManager.GetInstance().LogInfo("Added Azure Key Vault to app configuration");
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionDev");

if (builder.Environment.IsProduction())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionProd");
    connectionString += "sslmode=Required;sslca=cert/DigiCertGlobalRootCA.crt.pem;";
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

var serverVersion = ServerVersion.AutoDetect(connectionString);

// Add services to the container.
// Add CORS
builder.Services.AddCors(p => p.AddPolicy(corsPolicyName, build =>
{
    if (builder.Environment.IsProduction())
        build.WithOrigins("https://captioneerfrontend.azurewebsites.net").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    else
        build.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adds the already made DB context and configures the connection string and server version
builder.Services.AddDbContext<CaptioneerDBContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);
LoggerManager.GetInstance().LogInfo("Added DB context");

var app = builder.Build();

app.UseCors(corsPolicyName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ChatHub>("/chat");
});

app.Run();

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Captioneer.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var vaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
var clientId = builder.Configuration["AzureKeyVault:ClientId"];
var tenantId = builder.Configuration["AzureKeyVault:TenantId"];
var clientSecret = builder.Configuration["AzureKeyVault:ClientSecret"];

builder.Host.ConfigureAppConfiguration(builder =>
{
    var credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
    var client = new SecretClient(new Uri(vaultUrl), credentials);
    builder.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionDev");

if (builder.Environment.IsProduction())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionProd");
    connectionString += "sslmode=Required;sslca=cert/DigiCertGlobalRootCA.crt.pem;";
}

var serverVersion = ServerVersion.AutoDetect(connectionString);

// Add services to the container.
// Add CORS
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adds the already made DB context and configures the connection string and server version
builder.Services.AddDbContext<CaptioneerDBContext>(options =>
    options.UseMySql(connectionString, serverVersion)
    );

var app = builder.Build();

// Allow any header, origin and method to be called
app.UseCors (builder => {
    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

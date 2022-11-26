using Captioneer.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var serverVersion = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"));

// Add services to the container.
// Add CORS
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adds the already made DB context and configures the connection string and server version
builder.Services.AddDbContext<CaptioneerDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion)
    );

var app = builder.Build();

// Allow any header, origin and method to be called
app.UseCors ( builder => {
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

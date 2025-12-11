using Microsoft.OpenApi.Models;
using App.Business;
using App.DAL;
using App.API;
using App.DAL.Presistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS - Allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "App.API", Version = "v1" });
});


builder.Services
    .AddDataAccess(builder.Configuration)
    .AddBusiness();

builder.Services.AddSwagger();
builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

// Add new user
using var scope = app.Services.CreateScope();
await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Serve static files from wwwroot
app.UseStaticFiles();

// Use CORS - Allow all origins
app.UseCors("AllowAll");

// Add Middlewares
app.AddMiddlewares();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

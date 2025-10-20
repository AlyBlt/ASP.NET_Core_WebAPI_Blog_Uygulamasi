using App.Api.Repositories.Implementations;
using App.Api.Repositories.Interfaces;
using App.Api.Services.Implementations;
using App.Api.Services.Interfaces;
using App.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // varsa öncekileri temizle
builder.Logging.AddConsole();     // console log ekle

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

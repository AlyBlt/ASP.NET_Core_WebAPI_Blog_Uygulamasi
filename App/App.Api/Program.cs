using App.Api.Data;
using App.Api.Helpers;
using App.Api.Mappings;
using App.Api.Middlewares;
using App.Api.Models;
using App.Api.Repositories.Implementations;
using App.Api.Repositories.Interfaces;
using App.Api.Services.Implementations;
using App.Api.Services.Interfaces;
using App.Api.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;



var builder = WebApplication.CreateBuilder(args);
// Configuration dosyas�n� al�yoruz
var configuration = builder.Configuration;
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configuration'� DI container'�na ekliyoruz
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"], // JWT Issuer'� al�yoruz
            ValidAudience = builder.Configuration["JWTSettings:Audience"], // JWT Audience'� al�yoruz
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:SecretKey"])), // SecretKey'i al�yoruz
            ClockSkew = TimeSpan.Zero // Token'�n ge�erlili�i i�in tolerans� s�f�rl�yoruz
        };
    });

builder.Logging.ClearProviders(); // varsa �ncekileri temizle
builder.Logging.AddConsole();     // console log ekle

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddFluentValidationAutoValidation(); // Middleware i�in
builder.Services.AddFluentValidationClientsideAdapters(); // Opsiyonel, Swagger / UI taraf� i�in
builder.Services.AddValidatorsFromAssemblyContaining<ArticleValidator>();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();


// PasswordHasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher>();  // PasswordHasher'� IPasswordHasher<User> olarak kaydediyoruz


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger/OpenAPI s�r�m�n� a��k�a belirtiyoruz
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1",  // Burada versiyon bilgisini belirtiyoruz (OpenAPI 3.x.x format�nda olabilir)
    });

    // JWT Bearer Security tan�m�n� ekliyoruz
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// CORS'� yap�land�rma
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
       policy =>
       {
          policy.WithOrigins("http://localhost:7079")  // Burada frontend URL'mizi belirtiyoruz
                .AllowAnyHeader()
                 .AllowAnyMethod();
      });
});

var app = builder.Build();

// CORS Middleware'ini ekliyoruz
app.UseCors("AllowLocalhost");  // "AllowLocalhost" burada ekledi�imiz policy ad�


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // AddSwaggerGen i�inde kulland���m�z isim ("v1") ile e�le�melidir.
        // Bu, UI'�n do�ru JSON dosyas� kayna��n� bulmas�n� sa�lar.
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1");

    });

}

app.UseHttpsRedirection();

app.UseAuthentication();  // Bu sat�r, JWT do�rulamas� yapacak

app.UseAuthorization();

app.MapControllers();

app.Run();

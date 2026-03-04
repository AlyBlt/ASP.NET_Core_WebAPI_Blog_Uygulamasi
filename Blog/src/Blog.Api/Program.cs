using Blog.Application.Mappings; 
using Blog.Api.Middlewares;
using Blog.Application.Interfaces.Services;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Validators; // Application katmaný
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Blog.Application.Services;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Configuration dosyasýný alýyoruz
var configuration = builder.Configuration;
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configuration'ý DI container'ýna ekliyoruz
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
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"], // JWT Issuer'ý alýyoruz
            ValidAudience = builder.Configuration["JWTSettings:Audience"], // JWT Audience'ý alýyoruz
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:SecretKey"])), // SecretKey'i alýyoruz
            ClockSkew = TimeSpan.Zero // Token'ýn geçerliliði için toleransý sýfýrlýyoruz
        };
    });

builder.Logging.ClearProviders(); // varsa öncekileri temizle
builder.Logging.AddConsole();     // console log ekle

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddFluentValidationAutoValidation(); // Middleware için
builder.Services.AddFluentValidationClientsideAdapters(); // Opsiyonel, Swagger / UI tarafý için
builder.Services.AddValidatorsFromAssemblyContaining<ArticleValidator>();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();


// PasswordHasher
builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>(); // PasswordHasher'ý IPasswordHasher<User> olarak kaydediyoruz


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger/OpenAPI sürümünü açýkça belirtiyoruz
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1",  // Burada versiyon bilgisini belirtiyoruz (OpenAPI 3.x.x formatýnda olabilir)
    });

    // JWT Bearer Security tanýmýný ekliyoruz
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

// CORS'ý yapýlandýrma
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
app.UseCors("AllowLocalhost");  // "AllowLocalhost" burada eklediðimiz policy adý


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // AddSwaggerGen içinde kullandýðýmýz isim ("v1") ile eþleþmelidir.
        // Bu, UI'ýn doðru JSON dosyasý kaynaðýný bulmasýný saðlar.
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1");

    });

}

app.UseHttpsRedirection();

app.UseAuthentication();  // Bu satýr, JWT doðrulamasý yapacak

app.UseAuthorization();

app.MapControllers();

app.Run();

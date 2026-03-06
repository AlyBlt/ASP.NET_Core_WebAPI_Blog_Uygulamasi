using Blog.Api.Middlewares;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Interfaces.Services;
using Blog.Application.Services;
using Blog.Application.Validators.Article;
using Blog.Application.Validators.Auth;
using Blog.Application.Validators.Category;
using Blog.Application.Validators.Comment;
using Blog.Application.Validators.Tag;
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
// Configuration dosyasýný alýyoruz
var configuration = builder.Configuration;


// Configuration'ý DI container'ýna ekliyoruz
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = configuration["JWTSettings:SecretKey"];

        if (string.IsNullOrEmpty(secretKey))
            throw new Exception("JWT SecretKey is missing in configuration!");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"], // JWT Issuer'ý alýyoruz
            ValidAudience = builder.Configuration["JWTSettings:Audience"], // JWT Audience'ý alýyoruz
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // SecretKey'i alýyoruz
            ClockSkew = TimeSpan.Zero // Token'ýn geçerliliði için toleransý sýfýrlýyoruz
        };
    });

builder.Logging.ClearProviders(); // varsa öncekileri temizle
builder.Logging.AddConsole();     // console log ekle

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddFluentValidationAutoValidation(); // Middleware için
builder.Services.AddFluentValidationClientsideAdapters(); // Opsiyonel, Swagger / UI tarafý için
builder.Services.AddValidatorsFromAssemblyContaining<ArticleCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ArticleUpdateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryUpdateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TagCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TagUpdateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CommentCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CommentUpdateDtoValidator>();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();



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
           policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                 .AllowAnyHeader()
                 .AllowAnyMethod();
       });
});

// 1. Servis Kaydý (HEALTHCHECKS)
builder.Services.AddHealthChecks()
    .AddCheck("Self", () => HealthCheckResult.Healthy("API is running")) // API'nin kendisi
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "Database",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql", "sqlserver" }
    );

var app = builder.Build();

// CORS Middleware'ini ekliyoruz
app.UseCors("AllowLocalhost");  // "AllowLocalhost" burada eklediðimiz policy adý


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Configuration["ShowSwagger"] == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1");
    });
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();  // Bu satýr, JWT doðrulamasý yapacak

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    // CANLIYA ÇIKIÞ ÝÇÝN KRÝTÝK: Tablolar yoksa oluþturur, varsa günceller.
    await context.Database.MigrateAsync();
    await SeedData.SeedAsync(context);
}

// 2. Endpoint Tanýmý 
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            environment = app.Environment.EnvironmentName, // Ortam bilgisi (Development/Production)
            checks = report.Entries.Select(x => new
            {
                component = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                duration = x.Value.Duration.ToString()
            }),
            totalDuration = report.TotalDuration
        };

        await context.Response.WriteAsJsonAsync(response);
    }
});

app.Run();

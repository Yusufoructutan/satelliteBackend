using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;  // Add this using directive for OpenApi
using MyAspNetCoreProject.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// **Database Context** ekleniyor
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// **HttpClient** servisi ekleniyor
builder.Services.AddHttpClient();

// **Service Layer** ba��ml�l�klar� ekleniyor
builder.Services.AddScoped<ILocationService,LocationService>();  // LocationService

// **UserService**'i ekleme
builder.Services.AddScoped<IUserService, UserService>();  // IUserService -> UserService ba�lan�yor

builder.Services.AddHttpClient<OpenMeteoForecastService>();


// **JWT Authentication** yap�land�rmas�
var key = builder.Configuration.GetValue<string>("Jwt:Key");
var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
var audience = builder.Configuration.GetValue<string>("Jwt:Audience");

if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new ArgumentNullException("JWT ayarlar�ndan biri null ya da bo�.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Controllerlar ekleniyor
builder.Services.AddControllers();

// **CORS** yap�land�rmas�
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// **Swagger** yap�land�rmas�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // JWT Bearer i�in destek ekleniyor
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \n\n"
                    + "Enter 'Bearer' [space] and then your token in the text input below. \n\n"
                    + "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
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

var app = builder.Build();

// **Swagger** konfig�rasyonu (Development ortam�nda)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";  // Swagger UI yol ayar�
    });
}

app.UseRouting();
app.UseCors("AllowAllOrigins");  // CORS politikas� uygulan�yor
app.UseHttpsRedirection();
app.UseAuthentication();  // JWT kimlik do�rulama ekleniyor
app.UseAuthorization();

// **Controllerlar haritalan�yor**
app.MapControllers();

// **Uygulama �al��t�r�l�yor**
app.Run();

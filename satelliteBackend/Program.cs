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

// **Service Layer** baðýmlýlýklarý ekleniyor
builder.Services.AddScoped<ILocationService,LocationService>();  // LocationService

// **UserService**'i ekleme
builder.Services.AddScoped<IUserService, UserService>();  // IUserService -> UserService baðlanýyor

builder.Services.AddHttpClient<OpenMeteoForecastService>();


// **JWT Authentication** yapýlandýrmasý
var key = builder.Configuration.GetValue<string>("Jwt:Key");
var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
var audience = builder.Configuration.GetValue<string>("Jwt:Audience");

if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new ArgumentNullException("JWT ayarlarýndan biri null ya da boþ.");
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

// **CORS** yapýlandýrmasý
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// **Swagger** yapýlandýrmasý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // JWT Bearer için destek ekleniyor
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

// **Swagger** konfigürasyonu (Development ortamýnda)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";  // Swagger UI yol ayarý
    });
}

app.UseRouting();
app.UseCors("AllowAllOrigins");  // CORS politikasý uygulanýyor
app.UseHttpsRedirection();
app.UseAuthentication();  // JWT kimlik doðrulama ekleniyor
app.UseAuthorization();

// **Controllerlar haritalanýyor**
app.MapControllers();

// **Uygulama çalýþtýrýlýyor**
app.Run();

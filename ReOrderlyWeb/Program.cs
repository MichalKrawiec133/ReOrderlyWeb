using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.Migrations;
using Pomelo.EntityFrameworkCore.MySql;
using ReOrderlyWeb.ViewModels;

using ReOrderlyWeb.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.


//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("ReOrderlyWebDbContext");
builder.Services.AddDbContext<ReOrderlyWebDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .LogTo(Console.WriteLine, LogLevel.Information)
    );
builder.Services.AddTransient<DatabaseSeed>();
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "API", 
        ValidAudience = "USER", 
        ClockSkew = TimeSpan.Zero,//TODO: usuwa domyslny czas waznosci tokenu - 5min
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KLUCZDOTESTOWTESTTESTTESTTESTTESTTEST")) // DO ZMIANY PRZY PUBLIKOWANIU
    };
});
    
    

builder.Services.AddCors();

builder.Services.AddHostedService<SubscriptionOrderService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    
    c.MapType<LoginViewModel>(() => new OpenApiSchema
    {
        Type = "object",
        Properties =
        {
            ["emailAddress"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("mariannowak@wp.pl") },
            ["password"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("nowak") }
        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Podaj token JWT w formacie: Bearer {token}"
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

// Configure the HTTP request pipeline.


using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ReOrderlyWebDbContext>();
    var databaseSeed = serviceScope.ServiceProvider.GetRequiredService<DatabaseSeed>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        databaseSeed.Seed();
    
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowCredentials();
    policy.AllowAnyMethod();
    policy.WithOrigins("http://localhost:4200");

});
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
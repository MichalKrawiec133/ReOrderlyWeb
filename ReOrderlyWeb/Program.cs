using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.Migrations;
using Pomelo.EntityFrameworkCore.MySql;
using ReOrderlyWeb.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.


//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("ReOrderlyWebDbContext");
builder.Services.AddDbContext<ReOrderlyWebDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        //.LogTo(Console.WriteLine, LogLevel.Information)
    );
builder.Services.AddTransient<DatabaseSeed>();
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = "Cookies";
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie("Cookies", options =>
{
    options.Cookie.Name = "auth_cookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = redirectContext =>
        {
            redirectContext.HttpContext.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    };
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "API", 
        ValidAudience = "USER", 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KLUCZDOTESTOWTESTTESTTESTTESTTESTTEST")) // DO ZMIANY PRZY PUBLIKOWANIU
    };
});
    
    ;

builder.Services.AddCors();

builder.Services.AddHostedService<SubscriptionOrderService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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
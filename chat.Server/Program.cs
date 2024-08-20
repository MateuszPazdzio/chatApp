using chat.Server.Data;
using chat.Server.mappers;
using chat.Server.models;
using chat.Server.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders(); // Optionally clear existing providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Set the minimum log level to Debug

// Add services to the container.

builder.Services.AddDbContext<ChatDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("connectionDb")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ChatDbContext>()
    .AddDefaultTokenProviders();

var authenticationSettings = builder.Configuration.GetSection("Authentication").Get<AuthenticationSettings>();

builder.Services.AddAuthentication(options =>
{
    //options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
    options.DefaultScheme = "Bearer";
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

builder.Services.AddSingleton<AuthenticationSettings>(authenticationSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:4200") // Your Angular app URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Allow credentials
        });
});
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 1024000000000000000;
    e.EnableDetailedErrors = true;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(UserProfile));


builder.Services.AddHttpClient();
builder.Services.AddSingleton<IUserHttpContext, UserHttpContext>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<ChatHub>();


builder.Services.ConfigureApplicationCookie(options =>
{
    //options.Cookie.Name = "YourAppNameCookie";
    //options.Cookie.HttpOnly = true;
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Adjust expiration time as needed
    options.LoginPath = "/api/auth/login"; // Specify your login path
    //options.AccessDeniedPath = "/Account/AccessDenied"; // Specify your access denied path
    //options.SlidingExpiration = true;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.MapHub<ChatHub>("/chatHub"); ;
//app.UseCors();
app.Run();

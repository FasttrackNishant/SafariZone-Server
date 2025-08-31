using System.Text;
using AuthService.Core.Interfaces;
using AuthService.Core.Services;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var azure = builder.Configuration.GetSection("AzureAd");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// this is for AD Users
builder.Services.AddAuthentication()
    .AddJwtBearer("AzureAd", options =>
    {
        options.Authority = $"{azure["Instance"]}{azure["TenantId"]}/v2.0";
        options.Audience = azure["Audience"];
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuers =
            [
                $"https://sts.windows.net/{azure["TenantId"]}/", // for v1 tokens
                $"https://login.microsoftonline.com/{azure["TenantId"]}/v2.0" // for v2 tokens
            ],
        };
    });


// Ths is for Tourists
var internalJwt = builder.Configuration.GetSection("InternalJwt");
var internalKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(internalJwt["Key"]!));

builder.Services.AddAuthentication()
    .AddJwtBearer("Internal", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = internalJwt["Issuer"],
            ValidAudience = internalJwt["Audience"],
            IssuerSigningKey = internalKey
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FromAAD", policy => policy.AddAuthenticationSchemes("AzureAd").RequireAuthenticatedUser());
    options.AddPolicy("Internal",
        configurePolicy: policy => policy.AddAuthenticationSchemes("Internal").RequireAuthenticatedUser());
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, TouristAuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddScoped<IAadAuthService, AadAuthService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository > ();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SafariZone Auth API V1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
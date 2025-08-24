using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                     ?? ["http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)   // allow only your frontend(s)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();           // needed if you send cookies
    });
});

builder.Services.AddOcelot();

var app = builder.Build();

app.UseCors("AllowFrontend");  
await app.UseOcelot();

app.Run();
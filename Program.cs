

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task6.BE;
using Task6.BE.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();

    });
});

builder.Services.AddDbContext<MailDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Task6")));

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());
builder.Services.AddControllers();
var app = builder.Build();
app.UseRouting();
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MailHub>("/mailhub");
    endpoints.MapControllers();
});

app.MapGet("/", () => "Hello World!");

app.Run();

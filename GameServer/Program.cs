using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GameServer.Middleware;
using GameServer.Model.IoC;
using GameServer.Services;
using GameHub = GameServer.Presenter.Socket.GameHub;

namespace GameServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Model
        builder.Services.AddSingleton<IoCManager>();
        //
        
        
        // ASP NET Controllers
        builder.Services.AddControllers();
        builder.Services.AddSignalR(options =>
        {
            options.MaximumReceiveMessageSize = 1024 * 1024; // 1Mb
        }).AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            
            options.PayloadSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            
            options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        //
        
        // Health checks
        builder.Services.AddHealthChecks();
        builder.Services.AddHostedService<PrototypeLoaderService>();
        //
        
        
        var app = builder.Build();
        
        
        // Middleware
        app.UseStaticFiles();
        app.UseRouting();
        app.UseWhen(
            context => context.Request.Path.StartsWithSegments("/api"),
            appBuilder => appBuilder.UseMiddleware<AppTokenMiddleware>()
        );
        
        
        // Endpoints
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready");
        app.MapControllers();
        app.MapHub<GameHub>("/game");

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/index.html");
            return Task.CompletedTask;
        });
        
        app.Run();
    }
}
using GameServer.Middleware;
using GameServer.Model.IoC;
using GameServer.Services;

namespace GameServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // ECS
        builder.Services.AddSingleton<IoCManager>();
        
        // ASP NET Controllers
        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        //
        
        // Health checks
        builder.Services.AddHealthChecks();
        builder.Services.AddHostedService<PrototypeLoaderService>();
        //
        
        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //
        
        var app = builder.Build();
        
        
        // Middleware --
        app.UseRouting();
        app.UseWhen(
            context => context.Request.Path.StartsWithSegments("/api"),
            appBuilder => appBuilder.UseMiddleware<AppTokenMiddleware>()
        );
        
        
        // Endpoints --- 
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready");
        app.MapControllers();
        // app.MapHub<GameHub>("/game", options =>
        // {
        //     options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
        //     options.ApplicationMaxBufferSize = 64 * 1024;
        //     options.TransportMaxBufferSize = 64 * 1024;
        // });
        
        // Swagger in dev env
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapSwagger();
        }
        //

        app.Run();
    }
}
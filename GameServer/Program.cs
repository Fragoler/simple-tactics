using GameServer.Model;
using GameServer.Model.Games;
using GameServer.View;

namespace GameServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        builder.Services.AddScoped<GameService>();
        builder.Services.AddSingleton<GamesManager>();
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        // Middleware ---
        app.UseRouting();

        // Endpoints ---
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready");

        app.MapHub<GameHub>("/game", options =>
        {
            options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
            options.ApplicationMaxBufferSize = 64 * 1024;
            options.TransportMaxBufferSize = 64 * 1024;
        });

        // Run ---
        app.Run();
    }
}

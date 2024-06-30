using GameLeaderBoard.Extension;
using Infrastructure.Service.Implementation;
using Infrastructure.Service.Interface;
using Infrastructure.Utility.Caching;
using Microsoft.AspNetCore.SignalR;
using MovieManiaSignalr;
using StackExchange.Redis;

namespace GameLeaderBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });

            var configuration = builder.Configuration;

            builder.Services.AddCors(
                options => options.AddDefaultPolicy(
                    builder => builder.SetIsOriginAllowed(x => true)
                  .WithOrigins("null")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()));

            // Add services to the container.
            builder.Services.AddDbContextAndConfigurations(builder.Environment, configuration);

            builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
                    ConnectionMultiplexer.Connect(configuration["RedisSettings:ConnectionString"]));

            builder.Services.AddScoped<ICacheDistribution, CacheDistribution>();
            builder.Services.AddScoped<MovieManiaService>();

            builder.Services.AddAuthenticationConfig(configuration);

            builder.Services.AddHttpClient();

            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IUserIdProvider, CurrentUserIdProvider>();
            builder.Services.AddTransient<IRampageArena, RampageArena>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors();

            app.MapHub<ChatHub>("/chatHub").RequireAuthorization("RequireAuthenticatedUser");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
using GameLeaderBoard.Extension;
using GameLeaderBoard.Service.Implementation;
using GameLeaderBoard.Service.Interface;

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

            // Add services to the container.
            builder.Services.AddDbContextAndConfigurations(builder.Environment, configuration);

            builder.Services.AddTransient<IRampageArena, RampageArena>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
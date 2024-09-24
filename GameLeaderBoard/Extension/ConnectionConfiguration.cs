using GameLeaderBoard.Context;
using Microsoft.EntityFrameworkCore;

namespace GameLeaderBoard.Extension
{
    public static class ConnectionConfiguration
    {
        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddDbContextPool<LeaderBoardContext>(options =>
            {
                string? connStr = config["ConnectionStrings:DefaultConnection"];

                if (env.IsProduction())
                {
                    connStr = Environment.GetEnvironmentVariable("ConnectionStrings");
                }
                else
                {
                    connStr = config["ConnectionStrings:DefaultConnection"];
                }
                options.UseSqlServer(connStr);
            });

            /*if (env.IsDevelopment())
            {
                 redisConnectionString = config["RedisSettings:ConnectionString"];
            }
            else
            {
                redisConnectionString = config["RedisSettings:ProdConnection"];
            }

            var options = ConfigurationOptions.Parse(redisConnectionString);
            *//*options.Ssl = true;
            options.AbortOnConnectFail = false;*//*

            services.AddSingleton<IConnectionMultiplexer>(opt =>
                    ConnectionMultiplexer.Connect(options));*/
        }
    }
}

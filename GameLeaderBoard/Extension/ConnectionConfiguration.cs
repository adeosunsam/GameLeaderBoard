using GameLeaderBoard.Context;
using Microsoft.EntityFrameworkCore;

namespace GameLeaderBoard.Extension
{
    public static class ConnectionConfiguration
    {
        /*private static string GetHerokuConnectionString()
        {
            // Get the Database URL from the ENV variables in Heroku
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            // parse the connection string
            var databaseUri = new Uri(connectionUrl);
            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port=5432;" +
            $"Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";

        }*/

        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddDbContextPool<LeaderBoardContext>(options =>
            {
                string connStr = config["ConnectionStrings:DefaultConnection"];

                /*if (env.IsProduction())
                {
                    connStr = GetHerokuConnectionString();
                }
                else
                {
                    connStr = config["ConnectionStrings:DefaultConnection"];
                }*/
                options.UseNpgsql(connStr);
            });
        }
    }
}

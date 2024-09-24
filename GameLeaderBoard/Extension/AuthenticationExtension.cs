using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameLeaderBoard.Extension
{
    public static class AuthenticationExtension
    {
        public static TokenValidation TokenData { get; set; }

        public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            GetEnvironmentVariable(env, configuration);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters

                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = TokenData.Audience,
                    ValidIssuer = TokenData.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(TokenData.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
            });
        }

        public static void GetEnvironmentVariable(IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsProduction())
            {
                TokenData = new TokenValidation
                {
                    Audience = Environment.GetEnvironmentVariable("ValidAudience"),
                    Issuer = Environment.GetEnvironmentVariable("ValidIssuer"),
                    SecretKey = Environment.GetEnvironmentVariable("SecretKey")
                };
                return;
            }
            TokenData = new TokenValidation
            {
                Audience = configuration["JwtSettings:ValidAudience"],
                Issuer = configuration["JwtSettings:ValidIssuer"],
                SecretKey = configuration["JwtSettings:SecretKey"]
            };
        }

        public struct TokenValidation
        {
            public string Audience { get; set; }
            public string Issuer { get; set; }
            public string SecretKey { get; set; }
        }
    }
}

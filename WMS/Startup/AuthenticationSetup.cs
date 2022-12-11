using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WMS.Startup
{
    public static class AuthenticationSetup
    {
        public static WebApplicationBuilder RegisterAuthentication(this WebApplicationBuilder builder)
        {

            var authenticationSetttings = new AuthenticationSettings();
            
            builder.Configuration.GetSection("Authentication").Bind(authenticationSetttings);
            builder.Services.AddSingleton(authenticationSetttings);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = authenticationSetttings.JwtIssuer,
                    ValidAudience = authenticationSetttings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSetttings.JwtKey)),
                };
            });

            return builder;
        }
    }
}

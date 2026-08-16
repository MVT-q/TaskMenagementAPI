using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.Middleware;
using TaskMenagementAPI.Models;
using TaskMenagementAPI.Services;
using TaskMenagementAPI.Settings;

namespace TaskMenagementAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var jwtOptions = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtOptions>()!;

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.Configure<AdminSettings>(
                builder.Configuration.GetSection("Admin"));

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("Jwt"));

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<ProjectService>();

            builder.Services.AddScoped<ProjectTaskService>();

            builder.Services.AddScoped<ProjectAccessService>();

            builder.Services.AddScoped<INotificationService, ConsoleNotificationService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandlingMiddleware();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            using var scope = app.Services.CreateScope();

            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            await userService.CreateAdmin();

            app.Run();
        }
    }
}

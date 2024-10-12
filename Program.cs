
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatientManagment.Behaviours.AdminBehaviour;
using PatientManagment.Behaviours.ObservationBehaviour;
using PatientManagment.Behaviours.PatientBehaviour;
using PatientManagment.Data;
using PatientManagment.DataAccess.AdminDataAccess;
using PatientManagment.DataAccess.ObservationDataAccess;
using PatientManagment.DataAccess.PatientDataAccess;
using PatientManagment.Middleware;
using PatientManagment.Models.Encryption;
using System.Text;

namespace PatientManagment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);
            RegisterServices(builder.Services);

            var app = builder.Build();
            ConfigureMiddleWare(app);

            app.Run();
        }
        public static void ConfigureMiddleWare(WebApplication app)
        {

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("defaultPolicy");
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
        }
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("myConnection"))
            );
            ConfigureAtheticationService(builder);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
        public static void ConfigureAtheticationService(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("AdminAndUser", policy => policy.RequireRole("User", "Admin"));
            });

            builder.Services.AddSingleton<JwtTokenGenerator>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                return new JwtTokenGenerator(
                    configuration["Jwt:Key"],
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"]
                );
            });
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("defaultPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7118")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyHeader();
                });
            });
        }
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IObservationBehaviour, ObservationBehaviour>();
            services.AddScoped<IObservationDataAccess, ObservationDataAccess>();

            services.AddTransient<IPatientBehaviour, PatientBehaviour>();
            services.AddScoped<IPatientDataAccess, PatientDataAccess>();

            services.AddTransient<IAdminBehaviour, AdminBehaviour>();
            services.AddScoped<IAdminDataAccess, AdminDataAccess>();
        }
    }
}

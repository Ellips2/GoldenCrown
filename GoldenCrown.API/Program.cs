using FluentValidation;
using GoldenCrown.API.BackgroundServices;
using GoldenCrown.API.Dtos.User;
using GoldenCrown.API.Middlewares;
using GoldenCrown.Application.Features.User.UserLogin;
using GoldenCrown.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace GoldenCrown.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionStrnig = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionStrnig));

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserLoginCommandHandler).Assembly));

            builder.Services.AddValidatorsFromAssemblyContaining<LoginRequest>();
            builder.Services.AddAutoMapper(_ => { }, typeof(Program).Assembly);

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKey",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddAuthorization();

            builder.Services.AddHostedService<SessionCleanupService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<AuthorizationMiddleware>();

            app.MapControllers();

            MigrateDatabase(app);

            app.Run();
        }

        private static void MigrateDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }
    }
}

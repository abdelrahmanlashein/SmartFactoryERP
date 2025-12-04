using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using SmartFactoryERP.Application; // 1. ·≈÷«›… Œœ„«  Application
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Infrastructure;
using SmartFactoryERP.Persistence; // 2. ·≈÷«›… Œœ„«  Persistence
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.Persistence.Services;
using System.Text;
using System.Text.Json.Serialization;
namespace SmartFactoryERP.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            QuestPDF.Settings.License = LicenseType.Community;
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", // «Õ›Ÿ «·«”„ œÂ ﬂÊÌ”
                    b => b.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // «” œ⁄«¡ «·„ÌÀÊœ“ „‰ „·›«  DependencyInjection
            builder.Services.AddApplicationServices(); // ”‰‰‘∆ Â–Â «·„ÌÀÊœ
            builder.Services.AddPersistenceServices(builder.Configuration); // ”‰‰‘∆ Â–Â «·„ÌÀÊœ
            builder.Services.AddInfrastructureServices(builder.Configuration);
            // Add services to the container.
            //builder.Services.AddControllers();
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Â–« «·”ÿ— Ì„‰⁄ «·œÊ«∆— «·„›—€… «· Ì  ”»» «·‹ Crash
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
            builder.Services.AddEndpointsApiExplorer();
           // builder.Services.AddSwaggerGen();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // 1.  ”ÃÌ· Œœ„… «·‹ Auth
            builder.Services.AddScoped<IAuthService, AuthService>();

            // 2. ≈⁄œ«œ«  «·‹ JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });


            var app = builder.Build();
            // --- 3. ≈⁄œ«œ «·‹ HTTP Pipeline ---
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication(); 
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

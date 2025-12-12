using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using SmartFactoryERP.Application;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Common;
using SmartFactoryERP.Infrastructure;
using SmartFactoryERP.Persistence;
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.Persistence.Seeds;
using SmartFactoryERP.Persistence.Services;
using SmartFactoryERP.WebAPI.Hubs;
using SmartFactoryERP.WebAPI.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartFactoryERP.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var builder = WebApplication.CreateBuilder(args);

            // ✅ CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // ✅ SignalR
            builder.Services.AddSignalR();
            
            // ✅ Application Layer Services (includes MediatR)
            builder.Services.AddApplicationServices();
            
            // ✅ Persistence Layer Services (includes DbContext, Identity, Repositories)
            builder.Services.AddPersistenceServices(builder.Configuration);
            
            // ✅ Infrastructure Layer Services (includes Email, PDF, ML)
            builder.Services.AddInfrastructureServices(builder.Configuration);
            
            // ✅ HttpContextAccessor for accessing user context in handlers
            builder.Services.AddHttpContextAccessor();
            
            // ✅ Background Services
            builder.Services.AddHostedService<MachineSimulationService>();

            // ✅ Controllers with JSON options
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();

            // ✅ JWT Authentication
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
                
                // ✅ SignalR JWT support
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/machineHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // ✅ Authorization Policies (IMPORTANT!)
            builder.Services.AddAuthorization(options =>
            {
                // HR Management policies
                options.AddPolicy("CanManageHR", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin, Roles.HRManager));
                
                options.AddPolicy("CanViewHR", policy =>
                    policy.RequireAuthenticatedUser());
                
                options.AddPolicy("CanManageAttendance", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin, Roles.HRManager, Roles.Manager));
                
                // User Management policies
                options.AddPolicy("CanManageUsers", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin));
                
                options.AddPolicy("CanDeleteUsers", policy =>
                    policy.RequireRole(Roles.SuperAdmin));
                
                // Inventory Management policies
                options.AddPolicy("CanManageInventory", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin, Roles.InventoryManager));
                
                // Production Management policies
                options.AddPolicy("CanManageProduction", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin, Roles.ProductionManager));
                
                // Purchasing Management policies
                options.AddPolicy("CanManagePurchasing", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin, Roles.PurchasingManager));
                
                // Sales Management policies
                options.AddPolicy("CanManageSales", policy =>
                    policy.RequireRole(Roles.SuperAdmin, Roles.Admin, Roles.SalesManager));
            });

            var app = builder.Build();

            // ✅ Seed Roles on startup
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleSeeder.SeedRoles(roleManager);
            }

            // ✅ Development-only features
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // ✅ Middleware Pipeline (ORDER MATTERS!)
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();  // Must be before UseAuthorization
            app.UseAuthorization();   // Must be after UseAuthentication

            app.MapControllers();
            app.MapHub<MachineHub>("/machineHub");

            app.Run();
        }
    }
}

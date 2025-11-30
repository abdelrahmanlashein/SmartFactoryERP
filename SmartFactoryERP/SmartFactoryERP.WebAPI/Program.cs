using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Application; // 1. ·≈÷«›… Œœ„«  Application
using SmartFactoryERP.Persistence; // 2. ·≈÷«›… Œœ„«  Persistence
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.Infrastructure;

using System.Text.Json.Serialization;
using QuestPDF.Infrastructure;
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
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

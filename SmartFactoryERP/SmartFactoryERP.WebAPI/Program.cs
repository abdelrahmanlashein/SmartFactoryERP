using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Application; // 1. ·≈÷«›… Œœ„«  Application
using SmartFactoryERP.Persistence; // 2. ·≈÷«›… Œœ„«  Persistence
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.WebAPI.Hubs;
using SmartFactoryERP.WebAPI.Services;
using System.Text.Json.Serialization;
namespace SmartFactoryERP.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // «” œ⁄«¡ «·„ÌÀÊœ“ „‰ „·›«  DependencyInjection
            builder.Services.AddApplicationServices(); // ”‰‰‘∆ Â–Â «·„ÌÀÊœ
            builder.Services.AddPersistenceServices(builder.Configuration); // ”‰‰‘∆ Â–Â «·„ÌÀÊœ

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
            // ######### Register SignalR and the Simulation Service #########
            builder.Services.AddSignalR();
            builder.Services.AddHostedService<MachineSimulationService>();


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

            app.UseAuthorization();


            app.MapControllers();
            //  ######### Map the SignalR Hub here (before app.Run()) #########
            app.MapHub<MachineHub>("/machineHub");


            app.Run();
        }
    }
}

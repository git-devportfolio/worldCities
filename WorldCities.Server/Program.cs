
using Microsoft.EntityFrameworkCore;
using WorldCities.Server.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace WorldCities.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adds Serilog support
            builder.Host.UseSerilog((ctx, lc) => lc
             .ReadFrom.Configuration(ctx.Configuration)
             .WriteTo.MSSqlServer(connectionString:
             ctx.Configuration.GetConnectionString("DefaultConnection"),
             restrictedToMinimumLevel: LogEventLevel.Information,
             sinkOptions: new MSSqlServerSinkOptions
             {
                 TableName = "LogEvents",
                 AutoCreateSqlTable = true
             }
             ).WriteTo.Console());

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}

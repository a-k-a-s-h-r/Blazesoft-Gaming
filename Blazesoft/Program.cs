using Blazesoft.Data;
using Blazesoft.Services;
using Microsoft.Extensions.Configuration;

namespace Blazesoft
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IDbContext,DbContext>();
            builder.Services.AddSingleton<IPlayerService,PlayerService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                await playerService.SeedInitialPlayersAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            await app.RunAsync();
        }
    }
}
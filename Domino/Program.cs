
using Domino.Application.Contract;
using Domino.Application.Services;
using Domino.Infraestructure.Contracs;
using Domino.Infraestructure.Repositories;
using Domino.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Domino
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
             b => b.MigrationsAssembly("Domino.Persistence")
             ));



            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
            builder.Services.AddScoped<ITournamentRegistrationRepository, TournamentRegistrationRepository>();
            builder.Services.AddScoped<IRoundRepository, RoundRepository>();
            builder.Services.AddScoped<ITableRepository, TableRepository>();
            builder.Services.AddScoped<IResultRepository, ResultRepository>();
            builder.Services.AddScoped<IClassificationRepository, ClassificationRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<ITournamentService, TournamentService>();
            builder.Services.AddScoped<IRoundService, RoundService>();
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<IResultService, ResultService>();
            builder.Services.AddScoped<IClassificationService, ClassificationService>();
            builder.Services.AddScoped<ITournamentRegistrationService, TournamentRegistrationService>();



            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.MapControllers();

            app.Run();
        }
    }
}

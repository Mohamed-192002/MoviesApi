using Microsoft.EntityFrameworkCore;
using Movies;
using MoviesApi.AutoMapper;
using MoviesCore.Services;
using MoviesEF.Repository;
using System.Reflection;

namespace MoviesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            var connection = builder.Configuration.GetConnectionString("DefalutConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connection)
                );

            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
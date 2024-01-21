using Dynatron.Api.Controllers.Commands;
using Dynatron.Infrastructure;
using Dynatron.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Dynatron.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerDb")));
            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            builder.Services.AddValidatorsFromAssemblyContaining<CustomerCommandValidator>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
                        context.Database.EnsureCreated();

                        var isDatabaseEmpty = context.Customers.FirstOrDefault() == null;
                        if (isDatabaseEmpty)
                        {
                            context.AddRange(SeedData.GetCustomers());
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        var logger = app.Services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }
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
        }
    }
}

using Dynatron.Api.Controllers.Commands;
using Dynatron.Api.HttpPipeline;
using Dynatron.Domain;
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
            builder.Services.AddScoped<TransactionMiddleware>();
            builder.Services.AddSingleton<ISeedData<Customer>, SeedData>();

            builder.Services.AddValidatorsFromAssemblyContaining<CustomerCommandValidator>();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(c => c.AddDefaultPolicy(p => p.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));

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
                            var seedData = scope.ServiceProvider.GetRequiredService<ISeedData<Customer>>();
                            context.AddRange(seedData.GetData());
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
                app.UseCors();
            }

            app.UseHttpsRedirection();
            app.UseTransactionMiddleware();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

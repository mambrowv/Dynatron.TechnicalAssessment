using Dynatron.Domain;
using Dynatron.Infrastructure;
using Dynatron.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dynatron.Api.IntegrationTests
{
    public class ApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<CustomerDbContext>>();
                services.RemoveAll<CustomerDbContext>();
                services.RemoveAll<ISeedData<Customer>>();

                services.AddSingleton<ISeedData<Customer>, TestData>();
                services.AddDbContext<CustomerDbContext>(c => c.UseInMemoryDatabase("CustomerDb").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            });
        }
    }
}

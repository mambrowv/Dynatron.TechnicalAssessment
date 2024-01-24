
using Dynatron.Infrastructure;

namespace Dynatron.Api.HttpPipeline
{
    public static class TransactionMiddlewareExts
    {
        public static IApplicationBuilder UseTransactionMiddleware(this IApplicationBuilder application)
        {
            return application.UseMiddleware<TransactionMiddleware>();
        }
    }

    public class TransactionMiddleware : IMiddleware
    {
        private readonly CustomerDbContext _dbContext;

        public TransactionMiddleware(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string[] writeMethods = ["POST", "PUT"];

            if(writeMethods.Contains(context.Request.Method))
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                
                await next(context);

                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}

﻿using Dynatron.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dynatron.Api.Filters
{
    public class Validate<T> : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var commands = context.ActionArguments
                .Where(a => a.Value is T)
                .Cast<T>()
                .ToList();

            foreach(var command in commands)
            {
                var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
                var result = await validator.ValidateAsync(command);

                if(!result.IsValid)
                {
                    context.Result = new BadRequestObjectResult(new ValidationErrorModel(result.Errors));
                    return;
                }
            }

            await next();
        }
    }
}

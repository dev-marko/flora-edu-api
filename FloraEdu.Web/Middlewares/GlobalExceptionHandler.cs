using System.Text.Json;
using FloraEdu.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Middlewares;

public static class GlobalExceptionHandler
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorAppBuilder =>
        {
            errorAppBuilder.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;
                var errorCode = ErrorCodes.InternalServerError;
                var statusCode = 500;

                switch (exception)
                {
                    case ApiException apiException:
                    {
                        errorCode = apiException.ErrorCode;
                        statusCode = (int)ApiExceptionExtension.GetStatusCode(apiException.ErrorCode);
                        break;
                    }
                    case ArgumentException or ArgumentNullException:
                    {
                        errorCode = ErrorCodes.BadRequest;
                        statusCode = StatusCodes.Status400BadRequest;
                        break;
                    }
                }

                var problemDetails = new ProblemDetails
                {
                    Title = errorCode,
                    Status = statusCode
                };

                var responseDetailsJson = JsonSerializer.Serialize(problemDetails);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsync(responseDetailsJson);
            });
        });
    }
}

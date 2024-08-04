using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Interfaces;
using Serilog;
using Serilog.Context;

namespace MultiMart.Infrastructure.Middleware;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUser _currentUser;
    private readonly IStringLocalizer _t;
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ICurrentUser currentUser,
        IStringLocalizer<ExceptionMiddleware> t,
        ISerializerService jsonSerializer)
    {
        _currentUser = currentUser;
        _t = t;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string email = _currentUser.GetUserEmail() is { } userEmail ? userEmail : "Anonymous";
            var userId = _currentUser.GetUserId();
            string tenant = _currentUser.GetTenant() ?? string.Empty;
            if (userId != DefaultIdType.Empty) LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
            if (!string.IsNullOrEmpty(tenant)) LogContext.PushProperty("Tenant", tenant);
            string errorId = DefaultIdType.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = _t["Provide the ErrorId {0} to the support team for further analysis.", errorId]
            };

            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            if (exception is FluentValidation.ValidationException fluentException)
            {
                errorResult.Exception = "One or More Validations failed.";
                foreach (var error in fluentException.Errors)
                {
                    errorResult.Messages.Add(
                        new
                        {
                            error.PropertyName, error.ErrorMessage
                        }.ToString() ?? _t["Validation failed"]);
                }
            }

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case FluentValidation.ValidationException:
                    errorResult.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error("{ErrorResultException} Request failed with Status Code {ErrorResultStatusCode} and Error Id {ErrorId}", errorResult.Exception, errorResult.StatusCode, errorId);
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started");
            }
        }
    }
}
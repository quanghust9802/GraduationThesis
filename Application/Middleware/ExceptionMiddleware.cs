using Domain.Exceptions;
using Domain.Resource;
using Microsoft.AspNetCore.Http;

namespace Application.Middleware
{
    public class ExceptionMiddleware
    {
        #region Fields


        private readonly RequestDelegate _next;

        #endregion

        #region Constructors
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Methods


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DataInvalidException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status404NotFound);

            }

            catch (ConflictException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status409Conflict);

            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (NotAllowedException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status405MethodNotAllowed);
            }

            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, ResourceENG.Error_Exception, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task HandleExceptionAsync<T>(HttpContext context, T exception, string userMessage, int statusCode) where T : Exception
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = statusCode;

            var err = new BaseException()
            {
                ErrorCode = statusCode,
                UserMessage = userMessage,
                DevMessage = exception.Message,
                TraceId = context.TraceIdentifier,
                MoreInfo = exception.HelpLink,
                Errors = exception.Data
            };

            await context.Response.WriteAsync(text: err.ToString() ?? "");
        }

        #endregion
    }
}

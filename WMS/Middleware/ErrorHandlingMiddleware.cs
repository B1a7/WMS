using WMS.Exceptions;

namespace WMS.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (NotAcceptableException notAcceptable)
            {
                context.Response.StatusCode = 406;
                await context.Response.WriteAsync(notAcceptable.Message);
            }
            catch (IncorrectInputException incorrect)
            {
                context.Response.StatusCode = 406;
                await context.Response.WriteAsync(incorrect.Message);
            }
            catch (NoEmptySpotException noEmptySpot)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(noEmptySpot.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Some error occured");
            }
        }
    }
}

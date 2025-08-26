namespace Library.Application.Common.Behaviors
{
    public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid();

            logger.LogInformation("Iniciando request {RequestName} con ID {RequestId}", requestName, requestId);
            logger.LogDebug("Request {RequestName} ({RequestId}) - Contenido: {RequestData}", requestName, requestId, JsonSerializer.Serialize(request));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next(ct);

                stopwatch.Stop();
                logger.LogInformation("Request {RequestName} ({RequestId}) completado exitosamente en {ElapsedMs}ms", requestName, requestId, stopwatch.ElapsedMilliseconds);

                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("Response {RequestName} ({RequestId}): {ResponseData}", requestName, requestId, JsonSerializer.Serialize(response));
                }

                if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    logger.LogWarning("Request lento detectado: {RequestName} ({RequestId}) tomó {ElapsedMs}ms", requestName, requestId, stopwatch.ElapsedMilliseconds);
                }

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "Error procesando request {RequestName} ({RequestId}) después de {ElapsedMs}ms - Error: {ErrorMessage}", requestName, requestId, stopwatch.ElapsedMilliseconds, ex.Message);

                throw;
            }
        }
    }
}

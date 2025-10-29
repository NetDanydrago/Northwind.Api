using System.Text.Json;

namespace Api.Northwind.Middleware;

public class HandlerRequestResultMiddleware
{
    private readonly RequestDelegate Next;
    private readonly ILogger<HandlerRequestResultMiddleware> Logger;

    public HandlerRequestResultMiddleware(RequestDelegate next, ILogger<HandlerRequestResultMiddleware> logger)
    {
        Next = next;
        Logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await Next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        // Verificar si la respuesta contiene HandlerRequestResult con Success = false
        if (!string.IsNullOrEmpty(responseText) &&
            context.Response.ContentType?.Contains("application/json") == true &&
            context.Response.StatusCode == 200)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseText);

                // Verificar si tiene las propiedades de HandlerRequestResult
                if (jsonDocument.RootElement.TryGetProperty("success", out var successProperty) &&
                    jsonDocument.RootElement.TryGetProperty("errorMessage", out var errorMessageProperty))
                {
                    bool success = successProperty.GetBoolean();

                    if (!success)
                    {
                        string errorMessage = errorMessageProperty.GetString() ?? "Error desconocido";

                        // Log del error
                        Logger.LogWarning("HandlerRequestResult returned with Success=false. Error: {ErrorMessage}", errorMessage);

                        // Cambiar el status code a 400 (Bad Request) o el que prefieras
                        context.Response.StatusCode = 400;

                        // Opcionalmente, puedes modificar la respuesta
                        var errorResponse = new
                        {
                            success = false,
                            errorMessage
                        };

                        var modifiedResponse = JsonSerializer.Serialize(errorResponse);
                        responseText = modifiedResponse;
                    }
                }
            }
            catch (JsonException ex)
            {
                Logger.LogWarning(ex, "No se pudo parsear la respuesta como JSON");
            }
        }

        context.Response.Body = originalBodyStream;
        await context.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(responseText));
    }
}

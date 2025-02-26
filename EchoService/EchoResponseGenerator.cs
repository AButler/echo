using System.Net.Mime;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http.Extensions;

namespace EchoService;

public class EchoResponseGenerator
{
    public async Task<EchoResponse> Create(HttpRequest request)
    {
        using var bodyReader = new StreamReader(request.Body);
        var body = await bodyReader.ReadToEndAsync();

        var bodyJson = GetJsonObject(request.ContentType, body);

        return new EchoResponse
        {
            Method = request.Method,
            Path = "/" + request.RouteValues["path"],
            Headers = request.Headers,
            Body = body,
            BodyJson = bodyJson,
            Url = request.GetDisplayUrl(),
            Query = request.Query,
            Origin = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
        };
    }

    private static JsonObject? GetJsonObject(string? contentTypeHeader, string body)
    {
        if (string.IsNullOrEmpty(contentTypeHeader))
        {
            return null;
        }

        if (!IsJsonContentType(contentTypeHeader))
        {
            return null;
        }

        try
        {
            return JsonNode.Parse(body) as JsonObject;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsJsonContentType(string contentTypeHeader)
    {
        var contentType = new ContentType(contentTypeHeader);
        if (
            string.Equals(
                contentType.MediaType,
                "application/json",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            return true;
        }

        if (
            contentType.MediaType.StartsWith("application/", StringComparison.OrdinalIgnoreCase)
            && contentType.MediaType.EndsWith("+json", StringComparison.OrdinalIgnoreCase)
        )
        {
            return true;
        }

        return false;
    }
}

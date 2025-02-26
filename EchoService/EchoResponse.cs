using System.Text.Json.Nodes;

namespace EchoService;

public record EchoResponse
{
    public string Method { get; init; } = "";
    public string Path { get; init; } = "";
    public IHeaderDictionary Headers { get; init; } = new HeaderDictionary();
    public string? Body { get; init; }
    public JsonObject? BodyJson { get; init; } = null;
    public string Origin { get; init; } = "";
    public string Url { get; init; } = "";
    public IQueryCollection Query { get; init; } = new QueryCollection();
}

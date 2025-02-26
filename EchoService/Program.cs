using EchoService;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddSingleton<EchoResponseGenerator>();

var app = builder.Build();

app.Map(
    "/{*path}",
    async (HttpRequest request, EchoResponseGenerator echoResponseGenerator) =>
        await echoResponseGenerator.Create(request)
);

app.Run();

using AlphaService;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string serviceName = builder.Environment.ApplicationName;
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(x => x
        .AddSource(serviceName)
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter());
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));
builder.Services.AddScoped<Module1>();
builder.Services.AddScoped<Module2>();
builder.Services.AddScoped<Module3>();
builder.Services.AddScoped<Module4>();

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/method", async () =>
{
    using HttpClient httpClient = new();
    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:5002/api/method");
    response.EnsureSuccessStatusCode();
    return Results.Ok();
});

app.MapGet("/api/modular", (Module1 module1) =>
{
    module1.Run();
    return Results.Ok();
});

app.MapPost("/api/kafka", async ([FromBody] string message) =>
{
    Producer producer = new();
    await producer.SendMessage(message);
    return Results.Ok();
});

app.Run();
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
        .AddOtlpExporter(o => o.Endpoint = new Uri("http://jaeger:4317")));

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/method", async () =>
{
    using HttpClient httpClient = new();
    
    HttpResponseMessage response = await httpClient.GetAsync("http://gamma-service:8080/api/method");
    response.EnsureSuccessStatusCode();
    
    response = await httpClient.GetAsync("http://delta-service:8080/api/method");
    response.EnsureSuccessStatusCode();

    return Results.Ok();
});

app.Run();
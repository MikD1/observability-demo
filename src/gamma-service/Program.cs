using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
        .AddOtlpExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithTracing(x => x
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter());

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/method", async () =>
{
    HttpClient httpClient = new() { BaseAddress = new Uri("http://localhost:5004") };
    HttpResponseMessage response = await httpClient.GetAsync("api/method");
    string responseText = await response.Content.ReadAsStringAsync();
    
    string result = $"GammaService - OK. {responseText}";
    return Results.Ok(result);
});

app.Run();
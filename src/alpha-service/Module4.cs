using OpenTelemetry.Trace;

namespace AlphaService;

public class Module4(Tracer tracer)
{
    public void Run()
    {
        using TelemetrySpan parentSpan = tracer.StartActiveSpan("module-4-span");
    }
}
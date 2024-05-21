using OpenTelemetry.Trace;

namespace AlphaService;

public class Module3(Tracer tracer)
{
    public void Run()
    {
        using TelemetrySpan parentSpan = tracer.StartActiveSpan("module-3-span");
    }
}
using OpenTelemetry.Trace;

namespace AlphaService;

public class Module2(Tracer tracer)
{
    public void Run()
    {
        using TelemetrySpan parentSpan = tracer.StartActiveSpan("module-2-span");
    }
}
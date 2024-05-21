using OpenTelemetry.Trace;

namespace AlphaService;

public class Module1(Tracer tracer, Module2 module2, Module3 module3, Module4 module4)
{
    public void Run()
    {
        using TelemetrySpan parentSpan = tracer.StartActiveSpan("module-1-span");
        module2.Run();
        module3.Run();
        module4.Run();
    }
}
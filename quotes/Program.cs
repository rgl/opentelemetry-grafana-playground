using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure telemetry.
if (Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") != null)
{
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter()
            .AddConsoleExporter()
        );
}

builder.Logging.Configure(options =>
    {
        Console.WriteLine($"Logging ActivityTrackingOptions: {options.ActivityTrackingOptions}");
    }
);

var app = builder.Build();

// enable the swagger document endpoint and swagger ui.
app.UseSwagger();   // make the swagger available at /swagger/v1/swagger.json
app.UseSwaggerUI(); // make the swagger UI available at /swagger

app.MapControllers();

app.Run();

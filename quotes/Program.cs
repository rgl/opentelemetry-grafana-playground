using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Configuration.GetValue<string>("Otlp:ServiceName")!))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(oeo =>
            {
                // NB This overrides the OTEL_EXPORTER_OTLP_ENDPOINT environment variable.
                oeo.Endpoint = new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint")!);
                // NB This overrides the OTEL_EXPORTER_OTLP_PROTOCOL environment variable.
                oeo.Protocol = OtlpExportProtocol.Grpc;
                // NB OtlpExportProtocol.HttpProtobuf does not seem to be supported by Tempo.
                // see https://github.com/grafana/tempo/discussions/1172
                //oeo.Protocol = OtlpExportProtocol.HttpProtobuf;
            }
        )
        .AddConsoleExporter()
    );

builder.Logging.Configure(options =>
    {
        Console.WriteLine($"Logging ActivityTrackingOptions: {options.ActivityTrackingOptions}");
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // make the swagger available at /swagger/v1/swagger.json
    app.UseSwaggerUI(); // make the swagger UI available at /swagger
}
app.MapControllers();

app.Run();

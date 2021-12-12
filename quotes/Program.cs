using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// Allow HTTP/2 over TCP (to be able to use insecure gRPC to communicate with Tempo).
// NB this is required to connect to our test Tempo instance.
// NB you should not do this in production.
// See https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-6.0#call-insecure-grpc-services-with-net-core-client
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetryTracing(tpb => tpb
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Configuration.GetValue<string>("Otlp:ServiceName")))
    .AddOtlpExporter(oeo =>
        {
            // NB This overrides the OTEL_EXPORTER_OTLP_ENDPOINT environment variable.
            oeo.Endpoint = new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint"));
            // NB This overrides the OTEL_EXPORTER_OTLP_PROTOCOL environment variable.
            oeo.Protocol = OtlpExportProtocol.Grpc;
            // NB OtlpExportProtocol.HttpProtobuf does not seem to be supported by Tempo.
            // see https://github.com/grafana/tempo/discussions/1172
            //oeo.Protocol = OtlpExportProtocol.HttpProtobuf;
        }
    )
    .AddConsoleExporter()
    .AddAspNetCoreInstrumentation()
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
